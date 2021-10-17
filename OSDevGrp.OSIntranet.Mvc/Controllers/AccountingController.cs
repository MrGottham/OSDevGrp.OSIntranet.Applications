using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Common.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Core.Validators;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;
using OSDevGrp.OSIntranet.Mvc.Models.Common;

namespace OSDevGrp.OSIntranet.Mvc.Controllers
{
    [Authorize(Policy = "Accounting")]
    public class AccountingController : Controller
    {
        #region Private variables

        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly IClaimResolver _claimResolver;
        private readonly IConverter _accountingViewModelConverter = new AccountingViewModelConverter();
        private readonly IConverter _commonViewModelConverter = new CommonViewModelConverter();

        #endregion

        #region Constructor

        public AccountingController(ICommandBus commandBus, IQueryBus queryBus, IClaimResolver claimResolver)
        {
            NullGuard.NotNull(commandBus, nameof(commandBus))
                .NotNull(queryBus, nameof(queryBus))
                .NotNull(claimResolver, nameof(claimResolver));

            _commandBus = commandBus;
            _queryBus = queryBus;
            _claimResolver = claimResolver;
        }

        #endregion

        #region Methods

        [HttpGet]
        public IActionResult Accountings(int? accountingNumber = null)
        {
            return View("Accountings", CreateAccountingOptionsViewModel(accountingNumber ?? _claimResolver.GetAccountingNumber()));
        }

        [HttpGet]
        public IActionResult StartLoadingAccountings(int? accountingNumber = null)
        {
            return PartialView("_LoadingAccountingsPartial", CreateAccountingOptionsViewModel(accountingNumber));
        }

        [HttpGet]
        public async Task<IActionResult> LoadAccountings(int? accountingNumber = null)
        {
            IEnumerable<IAccounting> accountings = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IAccounting>>(new EmptyQuery());

            IEnumerable<AccountingIdentificationViewModel> accountingIdentificationViewModels = accountings.AsParallel()
                .Select(accounting => _accountingViewModelConverter.Convert<IAccounting, AccountingIdentificationViewModel>(accounting))
                .OrderBy(accountingIdentificationViewModel => accountingIdentificationViewModel.AccountingNumber)
                .ToList();

            if (accountingNumber.HasValue)
            {
                ViewData.Add("AccountingNumber", accountingNumber);
            }

            return PartialView("_AccountingCollectionPartial", accountingIdentificationViewModels);
        }

        [HttpGet]
        public IActionResult StartLoadingAccounting(int accountingNumber)
        {
            return PartialView("_LoadingAccountingPartial", CreateAccountingIdentificationViewModel(accountingNumber));
        }

        [HttpGet]
        public async Task<IActionResult> LoadAccounting(int accountingNumber)
        {
            Task<string> getPostingJournalKeyTask = GetPostingJournalKey(accountingNumber);
            Task<string> getPostingJournalResultKeyTask = GetPostingJournalResultKey(accountingNumber);
            await Task.WhenAll(
                getPostingJournalKeyTask,
                getPostingJournalResultKeyTask);

            string postingJournalKey = getPostingJournalKeyTask.GetAwaiter().GetResult();
            string postingJournalResultKey = getPostingJournalResultKeyTask.GetAwaiter().GetResult();

            Task<IEnumerable<LetterHeadViewModel>> getLetterHeadViewModelCollectionTask = GetLetterHeadViewModels();
            Task<IAccounting> getAccountingTask = GetAccounting(accountingNumber);
            Task<ApplyPostingJournalViewModel> getPostingJournalTask = GetPostingJournal(accountingNumber, postingJournalKey);
            Task<ApplyPostingJournalResultViewModel> getPostingJournalResultTask = GetPostingJournalResult(postingJournalResultKey);
            await Task.WhenAll(
                getLetterHeadViewModelCollectionTask,
                getAccountingTask,
                getPostingJournalTask,
                getPostingJournalResultTask);

            IAccounting accounting = getAccountingTask.GetAwaiter().GetResult();
            if (accounting == null)
            {
                return BadRequest();
            }

            AccountingViewModel accountingViewModel = _accountingViewModelConverter.Convert<IAccounting, AccountingViewModel>(accounting);
            accountingViewModel.PostingJournalKey = postingJournalKey;
            accountingViewModel.PostingJournal = getPostingJournalTask.GetAwaiter().GetResult();
            accountingViewModel.PostingJournalResultKey = postingJournalResultKey;
            accountingViewModel.PostingJournalResult = getPostingJournalResultTask.GetAwaiter().GetResult();
            accountingViewModel.LetterHeads = getLetterHeadViewModelCollectionTask.GetAwaiter().GetResult().ToList();

            return PartialView("_PresentAccountingPartial", accountingViewModel);
        }

        [HttpGet]
        public IActionResult StartCreatingAccounting()
        {
            return PartialView("_CreatingAccountingPartial", CreateAccountingOptionsViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> CreateAccounting()
        {
            List<LetterHeadViewModel> letterHeadViewModelCollection = (await GetLetterHeadViewModels()).ToList();

            AccountingViewModel accountingViewModel = new AccountingViewModel
            {
                LetterHead =  letterHeadViewModelCollection.FirstOrDefault(),
                BalanceBelowZero = BalanceBelowZeroType.Creditors,
                BackDating = 30,
                LetterHeads = letterHeadViewModelCollection,
                EditMode = EditMode.Create
            };

            return PartialView("_PresentAccountingPartial", accountingViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccounting(AccountingViewModel accountingViewModel)
        {
            NullGuard.NotNull(accountingViewModel, nameof(accountingViewModel));

            if (ModelState.IsValid == false)
            {
                return RedirectToAction("Accountings", "Accounting");
            }

            ICreateAccountingCommand command = _accountingViewModelConverter.Convert<AccountingViewModel, CreateAccountingCommand>(accountingViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("Accountings", "Accounting");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAccounting(AccountingViewModel accountingViewModel)
        {
            NullGuard.NotNull(accountingViewModel, nameof(accountingViewModel));

            if (ModelState.IsValid == false)
            {
                return RedirectToAction("Accountings", "Accounting", new {accountingViewModel.AccountingNumber});
            }

            IUpdateAccountingCommand command = _accountingViewModelConverter.Convert<AccountingViewModel, UpdateAccountingCommand>(accountingViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("Accountings", "Accounting", new {accountingViewModel.AccountingNumber});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccounting(int accountingNumber)
        {
            IDeleteAccountingCommand command = new DeleteAccountingCommand
            {
                AccountingNumber = accountingNumber
            };
            await _commandBus.PublishAsync(command);

            return RedirectToAction("Accountings", "Accounting");
        }

        [HttpGet]
        public IActionResult StartCreatingAccount(int accountingNumber)
        {
            return PartialView("_CreatingAccountPartial", CreateAccountingIdentificationViewModel(accountingNumber));
        }

        [HttpGet]
        public async Task<IActionResult> CreateAccount(int accountingNumber)
        {
            Task<IEnumerable<AccountGroupViewModel>> getAccountGroupViewModelCollectionTask = GetAccountGroupViewModels();
            Task<IAccounting> getAccountingTask = GetAccounting(accountingNumber);
            await Task.WhenAll(
                getAccountGroupViewModelCollectionTask,
                getAccountingTask);

            IAccounting accounting = getAccountingTask.Result;
            if (accounting == null)
            {
                return BadRequest();
            }

            AccountViewModel accountViewModel = CreateAccountViewModelForCreation<AccountViewModel>(_accountingViewModelConverter.Convert<IAccounting, AccountingIdentificationViewModel>(accounting));
            accountViewModel.CreditInfos = CreateInfoDictionaryViewModelForCreation<CreditInfoDictionaryViewModel, CreditInfoCollectionViewModel, CreditInfoViewModel>(DateTime.Today);
            accountViewModel.AccountGroups = getAccountGroupViewModelCollectionTask.Result.ToArray();

            return PartialView("_EditAccountPartial", accountViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount(AccountViewModel accountViewModel)
        {
            NullGuard.NotNull(accountViewModel, nameof(accountViewModel));

            HandleModelValidationForAccountViewModel(ModelState, accountViewModel);

            ICreateAccountCommand command = _accountingViewModelConverter.Convert<AccountViewModel, CreateAccountCommand>(accountViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("Accountings", "Accounting", new {accountingNumber = accountViewModel.Accounting.AccountingNumber});
        }

        [HttpGet]
        public IActionResult StartUpdatingAccount(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return PartialView("_UpdatingAccountPartial", CreateAccountIdentificationViewModel(accountingNumber, accountNumber));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAccount(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            Task<IEnumerable<AccountGroupViewModel>> getAccountGroupViewModelCollectionTask = GetAccountGroupViewModels();
            Task<IAccount> getAccountTask = GetAccount<GetAccountQuery, IAccount>(accountingNumber, accountNumber, DateTime.Today);
            await Task.WhenAll(
                getAccountGroupViewModelCollectionTask,
                getAccountTask);

            IAccount account = getAccountTask.Result;
            if (account == null)
            {
                return BadRequest();
            }

            AccountViewModel accountViewModel = _accountingViewModelConverter.Convert<IAccount, AccountViewModel>(account);
            accountViewModel.EditMode = EditMode.Edit;
            accountViewModel.AccountGroups = getAccountGroupViewModelCollectionTask.Result.ToArray();

            return PartialView("_EditAccountPartial", accountViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAccount(AccountViewModel accountViewModel)
        {
            NullGuard.NotNull(accountViewModel, nameof(accountViewModel));

            HandleModelValidationForAccountViewModel(ModelState, accountViewModel);

            IUpdateAccountCommand command = _accountingViewModelConverter.Convert<AccountViewModel, UpdateAccountCommand>(accountViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("Accountings", "Accounting", new {accountingNumber = accountViewModel.Accounting.AccountingNumber});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            IDeleteAccountCommand command = new DeleteAccountCommand
            {
                AccountingNumber = accountingNumber,
                AccountNumber = accountNumber
            };
            await _commandBus.PublishAsync(command);

            return RedirectToAction("Accountings", "Accounting", new {accountingNumber});
        }

        [HttpGet]
        public IActionResult StartCreatingBudgetAccount(int accountingNumber)
        {
            return PartialView("_CreatingBudgetAccountPartial", CreateAccountingIdentificationViewModel(accountingNumber));
        }

        [HttpGet]
        public async Task<IActionResult> CreateBudgetAccount(int accountingNumber)
        {
            Task<IEnumerable<BudgetAccountGroupViewModel>> getBudgetAccountGroupViewModelCollectionTask = GetBudgetAccountGroupViewModels();
            Task<IAccounting> getAccountingTask = GetAccounting(accountingNumber);
            await Task.WhenAll(
                getBudgetAccountGroupViewModelCollectionTask,
                getAccountingTask);

            IAccounting accounting = getAccountingTask.Result;
            if (accounting == null)
            {
                return BadRequest();
            }

            BudgetAccountViewModel budgetAccountViewModel = CreateAccountViewModelForCreation<BudgetAccountViewModel>(_accountingViewModelConverter.Convert<IAccounting, AccountingIdentificationViewModel>(accounting));
            budgetAccountViewModel.BudgetInfos= CreateInfoDictionaryViewModelForCreation<BudgetInfoDictionaryViewModel, BudgetInfoCollectionViewModel, BudgetInfoViewModel>(DateTime.Today);
            budgetAccountViewModel.BudgetAccountGroups = getBudgetAccountGroupViewModelCollectionTask.Result.ToArray();

            return PartialView("_EditBudgetAccountPartial", budgetAccountViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBudgetAccount(BudgetAccountViewModel budgetAccountViewModel)
        {
            NullGuard.NotNull(budgetAccountViewModel, nameof(budgetAccountViewModel));

            HandleModelValidationForAccountViewModel(ModelState, budgetAccountViewModel);

            ICreateBudgetAccountCommand command = _accountingViewModelConverter.Convert<BudgetAccountViewModel, CreateBudgetAccountCommand>(budgetAccountViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("Accountings", "Accounting", new {accountingNumber = budgetAccountViewModel.Accounting.AccountingNumber});
        }

        [HttpGet]
        public IActionResult StartUpdatingBudgetAccount(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return PartialView("_UpdatingBudgetAccountPartial", CreateAccountIdentificationViewModel(accountingNumber, accountNumber));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateBudgetAccount(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            Task<IEnumerable<BudgetAccountGroupViewModel>> getBudgetAccountGroupViewModelCollectionTask = GetBudgetAccountGroupViewModels();
            Task<IBudgetAccount> getBudgetAccountTask = GetAccount<GetBudgetAccountQuery, IBudgetAccount>(accountingNumber, accountNumber, DateTime.Today);
            await Task.WhenAll(
                getBudgetAccountGroupViewModelCollectionTask,
                getBudgetAccountTask);

            IBudgetAccount budgetAccount = getBudgetAccountTask.Result;
            if (budgetAccount == null)
            {
                return BadRequest();
            }

            BudgetAccountViewModel budgetAccountViewModel = _accountingViewModelConverter.Convert<IBudgetAccount, BudgetAccountViewModel>(budgetAccount);
            budgetAccountViewModel.EditMode = EditMode.Edit;
            budgetAccountViewModel.BudgetAccountGroups = getBudgetAccountGroupViewModelCollectionTask.Result.ToArray();

            return PartialView("_EditBudgetAccountPartial", budgetAccountViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBudgetAccount(BudgetAccountViewModel budgetAccountViewModel)
        {
            NullGuard.NotNull(budgetAccountViewModel, nameof(budgetAccountViewModel));

            HandleModelValidationForAccountViewModel(ModelState, budgetAccountViewModel);

            IUpdateBudgetAccountCommand command = _accountingViewModelConverter.Convert<BudgetAccountViewModel, UpdateBudgetAccountCommand>(budgetAccountViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("Accountings", "Accounting", new {accountingNumber = budgetAccountViewModel.Accounting.AccountingNumber});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBudgetAccount(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            IDeleteBudgetAccountCommand command = new DeleteBudgetAccountCommand
            {
                AccountingNumber = accountingNumber,
                AccountNumber = accountNumber
            };
            await _commandBus.PublishAsync(command);

            return RedirectToAction("Accountings", "Accounting", new {accountingNumber});
        }

        [HttpGet]
        public IActionResult StartCreatingContactAccount(int accountingNumber)
        {
            return PartialView("_CreatingContactAccountPartial", CreateAccountingIdentificationViewModel(accountingNumber));
        }

        [HttpGet]
        public async Task<IActionResult> CreateContactAccount(int accountingNumber)
        {
            Task<IEnumerable<PaymentTermViewModel>> getPaymentTermViewModelCollectionTask = GetPaymentTermViewModels();
            Task<IAccounting> getAccountingTask = GetAccounting(accountingNumber);
            await Task.WhenAll(
                getPaymentTermViewModelCollectionTask,
                getAccountingTask);

            IAccounting accounting = getAccountingTask.Result;
            if (accounting == null)
            {
                return BadRequest();
            }

            ContactAccountViewModel contactAccountViewModel = CreateAccountViewModelForCreation<ContactAccountViewModel>(_accountingViewModelConverter.Convert<IAccounting, AccountingIdentificationViewModel>(accounting));
            contactAccountViewModel.BalanceInfos = CreateInfoDictionaryViewModelForCreation<BalanceInfoDictionaryViewModel, BalanceInfoCollectionViewModel, BalanceInfoViewModel>(DateTime.Today);
            contactAccountViewModel.PaymentTerms = getPaymentTermViewModelCollectionTask.Result.ToArray();

            return PartialView("_EditContactAccountPartial", contactAccountViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateContactAccount(ContactAccountViewModel contactAccountViewModel)
        {
            NullGuard.NotNull(contactAccountViewModel, nameof(contactAccountViewModel));

            HandleModelValidationForAccountViewModel(ModelState, contactAccountViewModel);

            ICreateContactAccountCommand command = _accountingViewModelConverter.Convert<ContactAccountViewModel, CreateContactAccountCommand>(contactAccountViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("Accountings", "Accounting", new {accountingNumber = contactAccountViewModel.Accounting.AccountingNumber});
        }

        [HttpGet]
        public IActionResult StartUpdatingContactAccount(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return PartialView("_UpdatingContactAccountPartial", CreateAccountIdentificationViewModel(accountingNumber, accountNumber));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateContactAccount(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            Task<IEnumerable<PaymentTermViewModel>> getPaymentTermViewModelCollectionTask = GetPaymentTermViewModels();
            Task<IContactAccount> getContactAccountTask = GetAccount<GetContactAccountQuery, IContactAccount>(accountingNumber, accountNumber, DateTime.Today);
            await Task.WhenAll(
                getPaymentTermViewModelCollectionTask,
                getContactAccountTask);

            IContactAccount contactAccount = getContactAccountTask.Result;
            if (contactAccount == null)
            {
                return BadRequest();
            }

            ContactAccountViewModel contactAccountViewModel = _accountingViewModelConverter.Convert<IContactAccount, ContactAccountViewModel>(contactAccount);
            contactAccountViewModel.EditMode = EditMode.Edit;
            contactAccountViewModel.PaymentTerms = getPaymentTermViewModelCollectionTask.Result.ToArray();

            return PartialView("_EditContactAccountPartial", contactAccountViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateContactAccount(ContactAccountViewModel contactAccountViewModel)
        {
            NullGuard.NotNull(contactAccountViewModel, nameof(contactAccountViewModel));

            HandleModelValidationForAccountViewModel(ModelState, contactAccountViewModel);

            IUpdateContactAccountCommand command = _accountingViewModelConverter.Convert<ContactAccountViewModel, UpdateContactAccountCommand>(contactAccountViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("Accountings", "Accounting", new {accountingNumber = contactAccountViewModel.Accounting.AccountingNumber});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteContactAccount(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            IDeleteContactAccountCommand command = new DeleteContactAccountCommand
            {
                AccountingNumber = accountingNumber,
                AccountNumber = accountNumber
            };
            await _commandBus.PublishAsync(command);

            return RedirectToAction("Accountings", "Accounting", new {accountingNumber});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> ApplyPostingJournal(ApplyPostingJournalViewModel applyPostingJournalViewModel)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> AccountGroups()
        {
            return View("AccountGroups", await GetAccountGroupViewModels());
        }

        [HttpGet]
        public IActionResult CreateAccountGroup()
        {
            AccountGroupViewModel accountGroupViewModel = new AccountGroupViewModel
            {
                EditMode = EditMode.Create
            };

            return View("CreateAccountGroup", accountGroupViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccountGroup(AccountGroupViewModel accountGroupViewModel)
        {
            NullGuard.NotNull(accountGroupViewModel, nameof(accountGroupViewModel));

            if (ModelState.IsValid == false)
            {
                return View("CreateAccountGroup", accountGroupViewModel);
            }

            ICreateAccountGroupCommand command = _accountingViewModelConverter.Convert<AccountGroupViewModel, CreateAccountGroupCommand>(accountGroupViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("AccountGroups", "Accounting");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAccountGroup(int number)
        {
            IGetAccountGroupQuery query = new GetAccountGroupQuery
            {
                Number = number
            };
            IAccountGroup accountGroup = await _queryBus.QueryAsync<IGetAccountGroupQuery, IAccountGroup>(query);

            AccountGroupViewModel accountGroupViewModel = _accountingViewModelConverter.Convert<IAccountGroup, AccountGroupViewModel>(accountGroup);
            accountGroupViewModel.EditMode = EditMode.Edit;

            return View("UpdateAccountGroup", accountGroupViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAccountGroup(AccountGroupViewModel accountGroupViewModel)
        {
            NullGuard.NotNull(accountGroupViewModel, nameof(accountGroupViewModel));

            if (ModelState.IsValid == false)
            {
                return View("UpdateAccountGroup", accountGroupViewModel);
            }

            IUpdateAccountGroupCommand command = _accountingViewModelConverter.Convert<AccountGroupViewModel, UpdateAccountGroupCommand>(accountGroupViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("AccountGroups", "Accounting");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccountGroup(int number)
        {
            IDeleteAccountGroupCommand command = new DeleteAccountGroupCommand
            {
                Number = number
            };
            await _commandBus.PublishAsync(command);

            return RedirectToAction("AccountGroups", "Accounting");
        }

        [HttpGet]
        public async Task<IActionResult> BudgetAccountGroups()
        {
            return View("BudgetAccountGroups", await GetBudgetAccountGroupViewModels());
        }

        [HttpGet]
        public IActionResult CreateBudgetAccountGroup()
        {
            BudgetAccountGroupViewModel budgetAccountGroupViewModel = new BudgetAccountGroupViewModel
            {
                EditMode = EditMode.Create
            };

            return View("CreateBudgetAccountGroup", budgetAccountGroupViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBudgetAccountGroup(BudgetAccountGroupViewModel budgetAccountGroupViewModel)
        {
            NullGuard.NotNull(budgetAccountGroupViewModel, nameof(budgetAccountGroupViewModel));

            if (ModelState.IsValid == false)
            {
                return View("CreateBudgetAccountGroup", budgetAccountGroupViewModel);
            }

            ICreateBudgetAccountGroupCommand command = _accountingViewModelConverter.Convert<BudgetAccountGroupViewModel, CreateBudgetAccountGroupCommand>(budgetAccountGroupViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("BudgetAccountGroups", "Accounting");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateBudgetAccountGroup(int number)
        {
            IGetBudgetAccountGroupQuery query = new GetBudgetAccountGroupQuery
            {
                Number = number
            };
            IBudgetAccountGroup budgetAccountGroup = await _queryBus.QueryAsync<IGetBudgetAccountGroupQuery, IBudgetAccountGroup>(query);

            BudgetAccountGroupViewModel budgetAccountGroupViewModel = _accountingViewModelConverter.Convert<IBudgetAccountGroup, BudgetAccountGroupViewModel>(budgetAccountGroup);
            budgetAccountGroupViewModel.EditMode = EditMode.Edit;

            return View("UpdateBudgetAccountGroup", budgetAccountGroupViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBudgetAccountGroup(BudgetAccountGroupViewModel budgetAccountGroupViewModel)
        {
            NullGuard.NotNull(budgetAccountGroupViewModel, nameof(budgetAccountGroupViewModel));

            if (ModelState.IsValid == false)
            {
                return View("UpdateBudgetAccountGroup", budgetAccountGroupViewModel);
            }

            IUpdateBudgetAccountGroupCommand command = _accountingViewModelConverter.Convert<BudgetAccountGroupViewModel, UpdateBudgetAccountGroupCommand>(budgetAccountGroupViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("BudgetAccountGroups", "Accounting");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBudgetAccountGroup(int number)
        {
            IDeleteBudgetAccountGroupCommand command = new DeleteBudgetAccountGroupCommand
            {
                Number = number
            };
            await _commandBus.PublishAsync(command);

            return RedirectToAction("BudgetAccountGroups", "Accounting");
        }

        [HttpGet]
        public async Task<IActionResult> PaymentTerms()
        {
            return View("PaymentTerms", await GetPaymentTermViewModels());
        }

        [HttpGet]
        public IActionResult CreatePaymentTerm()
        {
            PaymentTermViewModel paymentTermViewModel = new PaymentTermViewModel
            {
                EditMode = EditMode.Create
            };

            return View("CreatePaymentTerm", paymentTermViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePaymentTerm(PaymentTermViewModel paymentTermViewModel)
        {
            NullGuard.NotNull(paymentTermViewModel, nameof(paymentTermViewModel));

            if (ModelState.IsValid == false)
            {
                return View("CreatePaymentTerm", paymentTermViewModel);
            }

            ICreatePaymentTermCommand command = _accountingViewModelConverter.Convert<PaymentTermViewModel, CreatePaymentTermCommand>(paymentTermViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("PaymentTerms", "Accounting");
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePaymentTerm(int number)
        {
            IGetPaymentTermQuery query = new GetPaymentTermQuery
            {
                Number = number
            };
            IPaymentTerm paymentTerm = await _queryBus.QueryAsync<IGetPaymentTermQuery, IPaymentTerm>(query);

            PaymentTermViewModel paymentTermViewModel =  _accountingViewModelConverter.Convert<IPaymentTerm, PaymentTermViewModel>(paymentTerm);
            paymentTermViewModel.EditMode = EditMode.Edit;

            return View("UpdatePaymentTerm", paymentTermViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePaymentTerm(PaymentTermViewModel paymentTermViewModel)
        {
            NullGuard.NotNull(paymentTermViewModel, nameof(paymentTermViewModel));

            if (ModelState.IsValid == false)
            {
                return View("UpdatePaymentTerm", paymentTermViewModel);
            }

            IUpdatePaymentTermCommand command =  _accountingViewModelConverter.Convert<PaymentTermViewModel, UpdatePaymentTermCommand>(paymentTermViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("PaymentTerms", "Accounting");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePaymentTerm(int number)
        {
            IDeletePaymentTermCommand command = new DeletePaymentTermCommand
            {
                Number = number
            };
            await _commandBus.PublishAsync(command);

            return RedirectToAction("PaymentTerms", "Accounting");
        }

        [HttpGet("api/accountings/{accountingNumber}/accounts/{accountNumber}")]
        public async Task<IActionResult> ResolveAccount(int accountingNumber, string accountNumber, DateTimeOffset statusDate)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                return BadRequest();
            }

            IAccount account = await GetAccount<GetAccountQuery, IAccount>(accountingNumber, accountNumber, statusDate.LocalDateTime.Date);
            if (account == null)
            {
                return BadRequest();
            }

            return Ok(_accountingViewModelConverter.Convert<IAccount, AccountViewModel>(account));
        }

        [HttpGet("api/accountings/{accountingNumber}/budgetaccounts/{accountNumber}")]
        public async Task<IActionResult> ResolveBudgetAccount(int accountingNumber, string accountNumber, DateTimeOffset statusDate)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                return BadRequest();
            }

            IBudgetAccount budgetAccount = await GetAccount<GetBudgetAccountQuery, IBudgetAccount>(accountingNumber, accountNumber, statusDate.LocalDateTime.Date);
            if (budgetAccount == null)
            {
                return BadRequest();
            }

            return Ok(_accountingViewModelConverter.Convert<IBudgetAccount, BudgetAccountViewModel>(budgetAccount));
        }

        [HttpGet("api/accountings/{accountingNumber}/contactaccounts/{accountNumber}")]
        public async Task<IActionResult> ResolveContactAccount(int accountingNumber, string accountNumber, DateTimeOffset statusDate)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                return BadRequest();
            }

            IContactAccount contactAccount = await GetAccount<GetContactAccountQuery, IContactAccount>(accountingNumber, accountNumber, statusDate.LocalDateTime.Date);
            if (contactAccount == null)
            {
                return BadRequest();
            }

            return Ok(_accountingViewModelConverter.Convert<IContactAccount, ContactAccountViewModel>(contactAccount));
        }

        [HttpPost("api/accountings/{accountingNumber}/postingjournals")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPostingLineToPostingJournal(int accountingNumber, string postingJournalKey, string postingLine, string postingJournalHeader = null)
        {
            if (string.IsNullOrWhiteSpace(postingJournalKey) || string.IsNullOrWhiteSpace(postingLine))
            {
                return BadRequest();
            }

            ApplyPostingLineViewModel applyPostingLineViewModel;
            try
            {
                applyPostingLineViewModel = JsonSerializer.Deserialize<ApplyPostingLineViewModel>(postingLine);
            }
            catch (JsonException)
            {
                return BadRequest();
            }

            try
            {
                SchemaValidator.Validate(applyPostingLineViewModel);
            }
            catch (IntranetValidationException ex)
            {
                return BadRequest(ex.Message);
            }

            ApplyPostingJournalViewModel applyPostingJournalViewModel = await GetPostingJournal(accountingNumber, postingJournalKey);
            applyPostingJournalViewModel.ApplyPostingLines.Add(applyPostingLineViewModel);

            return GetPartialViewForPostingJournal(await SavePostingJournal(postingJournalKey, applyPostingJournalViewModel), postingJournalKey, postingJournalHeader);
        }

        [HttpDelete("api/accountings/{accountingNumber}/postingjournals/{postingLineIdentifier}")]
        [ValidateAntiForgeryToken]
        public  Task<IActionResult> RemovePostingLineFromPostingJournal(int accountingNumber, string postingJournalKey, Guid postingLineIdentifier, string postingJournalHeader = null)
        {
            throw new NotImplementedException();
        }

        private Task<IAccounting> GetAccounting(int accountingNumber)
        {
            IGetAccountingQuery query = new GetAccountingQuery
            {
                AccountingNumber = accountingNumber,
                StatusDate = DateTime.Today
            };

            return _queryBus.QueryAsync<IGetAccountingQuery, IAccounting>(query);
        }

        private Task<TAccount> GetAccount<TGetAccountQuery, TAccount>(int accountingNumber, string accountNumber, DateTime statusDate) where TGetAccountQuery : class, IAccountIdentificationQuery, new() where TAccount : IAccountBase
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            TGetAccountQuery query = new TGetAccountQuery
            {
                AccountingNumber = accountingNumber,
                AccountNumber = accountNumber,
                StatusDate = statusDate
            };

            return _queryBus.QueryAsync<TGetAccountQuery, TAccount>(query);
        }

        private async Task<IEnumerable<AccountGroupViewModel>> GetAccountGroupViewModels()
        {
            IEnumerable<IAccountGroup> accountGroups = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IAccountGroup>>(new EmptyQuery());

            return accountGroups.AsParallel()
                .Select(accountGroup => _accountingViewModelConverter.Convert<IAccountGroup, AccountGroupViewModel>(accountGroup))
                .OrderBy(accountGroupViewModel => accountGroupViewModel.Number)
                .ToList();
        }

        private async Task<IEnumerable<BudgetAccountGroupViewModel>> GetBudgetAccountGroupViewModels()
        {
            IEnumerable<IBudgetAccountGroup> budgetAccountGroups = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IBudgetAccountGroup>>(new EmptyQuery());

            return budgetAccountGroups.AsParallel()
                .Select(budgetAccountGroup => _accountingViewModelConverter.Convert<IBudgetAccountGroup, BudgetAccountGroupViewModel>(budgetAccountGroup))
                .OrderBy(budgetAccountGroupViewModel => budgetAccountGroupViewModel.Number)
                .ToList();
        }

        private async Task<IEnumerable<PaymentTermViewModel>> GetPaymentTermViewModels()
        {
            IEnumerable<IPaymentTerm> paymentTerms = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IPaymentTerm>>(new EmptyQuery());

            return paymentTerms.AsParallel()
                .Select(paymentTerm => _accountingViewModelConverter.Convert<IPaymentTerm, PaymentTermViewModel>(paymentTerm))
                .OrderBy(paymentTermViewModel => paymentTermViewModel.Number)
                .ToList();
        }

        private async Task<IEnumerable<LetterHeadViewModel>> GetLetterHeadViewModels()
        {
            IEnumerable<ILetterHead> letterHeads = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<ILetterHead>>(new EmptyQuery());

            return letterHeads.AsParallel()
                .Select(letterHead => _commonViewModelConverter.Convert<ILetterHead, LetterHeadViewModel>(letterHead))
                .OrderBy(letterHeadViewModel => letterHeadViewModel.Number)
                .ToList();
        }

        private PartialViewResult GetPartialViewForPostingJournal(ApplyPostingJournalViewModel applyPostingJournalViewModel, string postingJournalKey, string postingJournalHeader = null)
        {
            NullGuard.NotNull(applyPostingJournalViewModel, nameof(applyPostingJournalViewModel))
                .NotNullOrWhiteSpace(postingJournalKey, nameof(postingJournalKey));

            ViewData.Add("PostingJournalKey", postingJournalKey);
            if (string.IsNullOrWhiteSpace(postingJournalHeader) == false)
            {
                ViewData.Add("Header", postingJournalHeader);
            }

            return PartialView("_PostingJournalPartial", applyPostingJournalViewModel);
        }

        private Task<string> GetPostingJournalKey(int accountingNumber)
        {
            IGetUserSpecificKeyQuery query = new GetUserSpecificKeyQuery
            {
                KeyElementCollection = new[] { nameof(ApplyPostingJournalViewModel), Convert.ToString(accountingNumber) }
            };
            return _queryBus.QueryAsync<IGetUserSpecificKeyQuery, string>(query);
        }

        private async Task<ApplyPostingJournalViewModel> GetPostingJournal(int accountingNumber, string key)
        {
            NullGuard.NotNullOrWhiteSpace(key, nameof(key));

            ApplyPostingJournalViewModel postingJournal = await GetObjectFromKeyValueEntry<ApplyPostingJournalViewModel>(key);
            if (postingJournal != null)
            {
                return postingJournal;
            }

            return new ApplyPostingJournalViewModel
            {
                AccountingNumber = accountingNumber,
                ApplyPostingLines = new ApplyPostingLineCollectionViewModel()
            };
        }

        private async Task<ApplyPostingJournalViewModel> SavePostingJournal(string key, ApplyPostingJournalViewModel applyPostingJournalViewModel)
        {
            NullGuard.NotNullOrWhiteSpace(key, nameof(key))
                .NotNull(applyPostingJournalViewModel, nameof(applyPostingJournalViewModel));

            ApplyPostingLineViewModel[] orderedPostingLines = applyPostingJournalViewModel.ApplyPostingLines
                .OrderByDescending(applyPostingLine => applyPostingLine.PostingDate.UtcDateTime.Date)
                .ThenByDescending(applyPostingLine => applyPostingLine.SortOrder ?? 0)
                .ToArray();

            applyPostingJournalViewModel.ApplyPostingLines = new ApplyPostingLineCollectionViewModel();
            applyPostingJournalViewModel.ApplyPostingLines.AddRange(orderedPostingLines);

            IPushKeyValueEntryCommand command = new PushKeyValueEntryCommand
            {
                Key = key,
                Value = applyPostingJournalViewModel
            };
            await _commandBus.PublishAsync(command);

            return applyPostingJournalViewModel;
        }

        private Task<string> GetPostingJournalResultKey(int accountingNumber)
        {
            IGetUserSpecificKeyQuery query = new GetUserSpecificKeyQuery
            {
                KeyElementCollection = new[] { nameof(ApplyPostingJournalResultViewModel), Convert.ToString(accountingNumber) }
            };
            return _queryBus.QueryAsync<IGetUserSpecificKeyQuery, string>(query);
        }

        private async Task<ApplyPostingJournalResultViewModel> GetPostingJournalResult(string key)
        {
            NullGuard.NotNullOrWhiteSpace(key, nameof(key));

            ApplyPostingJournalResultViewModel postingJournalResult = await GetObjectFromKeyValueEntry<ApplyPostingJournalResultViewModel>(key);
            if (postingJournalResult != null)
            {
                return postingJournalResult;
            }

            return new ApplyPostingJournalResultViewModel
            {
                PostingLines = new PostingLineCollectionViewModel(),
                PostingWarnings = new PostingWarningCollectionViewModel()
            };
        }

        private async Task<T> GetObjectFromKeyValueEntry<T>(string key) where T : class
        {
            NullGuard.NotNullOrWhiteSpace(key, nameof(key));

            IPullKeyValueEntryQuery query = new PullKeyValueEntryQuery
            {
                Key = key
            };
            IKeyValueEntry keyValueEntry = await _queryBus.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(query);

            return keyValueEntry?.ToObject<T>();
        }

        private void HandleModelValidationForAccountViewModel<TAccountViewModel>(ModelStateDictionary modelState, TAccountViewModel accountViewModel) where TAccountViewModel : AccountIdentificationViewModel
        {
            NullGuard.NotNull(modelState, nameof(modelState))
                .NotNull(accountViewModel, nameof(accountViewModel));

            if (modelState.ValidationState == ModelValidationState.Unvalidated)
            {
                modelState.Clear();
                if (TryValidateModel(accountViewModel))
                {
                    return;
                }
            }

            if (modelState.IsValid)
            {
                return;
            }

            string errorMessages = string.Join(Environment.NewLine, modelState.Values.SelectMany(modelStateEntry => modelStateEntry.Errors).Select(modelError => modelError.ErrorMessage));
            throw new IntranetExceptionBuilder(ErrorCode.InternalError, errorMessages).Build();
        }

        private static AccountingOptionsViewModel CreateAccountingOptionsViewModel(int? accountingNumber = null)
        {
            return new AccountingOptionsViewModel
            {
                DefaultAccountingNumber = accountingNumber
            };
        }

        private static AccountingIdentificationViewModel CreateAccountingIdentificationViewModel(int accountingNumber)
        {
            return new AccountingIdentificationViewModel
            {
                AccountingNumber = accountingNumber
            };
        }

        private static AccountIdentificationViewModel CreateAccountIdentificationViewModel(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return new AccountIdentificationViewModel
            {
                Accounting = CreateAccountingIdentificationViewModel(accountingNumber),
                AccountNumber = accountNumber
            };
        }

        private static TAccountCoreDataViewModel CreateAccountViewModelForCreation<TAccountCoreDataViewModel>(AccountingIdentificationViewModel accountingIdentificationViewModel) where TAccountCoreDataViewModel : AccountCoreDataViewModel, new()
        {
            NullGuard.NotNull(accountingIdentificationViewModel, nameof(accountingIdentificationViewModel));

            return new TAccountCoreDataViewModel
            {
                EditMode = EditMode.Create,
                Accounting = accountingIdentificationViewModel
            };
        }

        private static TInfoDictionaryViewModelBase CreateInfoDictionaryViewModelForCreation<TInfoDictionaryViewModelBase, TInfoCollectionViewModel, TInfoViewModel>(DateTime creationDate) where TInfoDictionaryViewModelBase : InfoDictionaryViewModelBase<TInfoCollectionViewModel, TInfoViewModel>, new() where TInfoCollectionViewModel : InfoCollectionViewModelBase<TInfoViewModel>, new() where TInfoViewModel : InfoViewModelBase, new()
        {
            IDictionary<short, TInfoCollectionViewModel> dictionary = new Dictionary<short, TInfoCollectionViewModel>
            {
                {(short) creationDate.Year, CreateInfoCollectionViewModelForCreation<TInfoCollectionViewModel, TInfoViewModel>(creationDate, 12 - creationDate.Month + 1)}
            };

            if (creationDate.Month > 1)
            {
                dictionary.Add((short) (creationDate.Year + 1), CreateInfoCollectionViewModelForCreation<TInfoCollectionViewModel, TInfoViewModel>(new DateTime(creationDate.Year + 1, 1, 1), creationDate.Month - 1));
            }

            return new TInfoDictionaryViewModelBase
            {
                Items = new ConcurrentDictionary<short, TInfoCollectionViewModel>(dictionary)
            };
        }

        private static TInfoCollectionViewModel CreateInfoCollectionViewModelForCreation<TInfoCollectionViewModel, TInfoViewModel>(DateTime creationDate, int numberOfMonths) where TInfoCollectionViewModel : InfoCollectionViewModelBase<TInfoViewModel>, new() where TInfoViewModel : InfoViewModelBase, new()
        {
            IList<TInfoViewModel> collection = new List<TInfoViewModel>();
            for (short month = (short) creationDate.Month; month < creationDate.Month + numberOfMonths; month++)
            {
                collection.Add(new TInfoViewModel {Year = (short) creationDate.Year, Month = month, EditMode = EditMode.Create});
            }

            return new TInfoCollectionViewModel
            {
                Items = new ReadOnlyCollection<TInfoViewModel>(collection.OrderBy(infoViewModel => infoViewModel.Month).ToList())
            };
        }

        #endregion
    }
}