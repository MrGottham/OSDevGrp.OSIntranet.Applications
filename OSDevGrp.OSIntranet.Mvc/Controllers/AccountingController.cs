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
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;
using OSDevGrp.OSIntranet.Mvc.Models.Common;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using OSDevGrp.OSIntranet.Mvc.Security;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Mvc.Controllers
{
    [Authorize(Policy = Policies.AccountingPolicy)]
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
                .Select(_accountingViewModelConverter.Convert<IAccounting, AccountingIdentificationViewModel>)
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
        [Authorize(Policy = Policies.AccountingViewerPolicy)]
        public async Task<IActionResult> LoadAccounting(int accountingNumber)
        {
            string postingJournalKey = await GetPostingJournalKey(accountingNumber);
            string postingJournalResultKey = await GetPostingJournalResultKey(accountingNumber);

            List<LetterHeadViewModel> letterHeadViewModelCollection = (await GetLetterHeadViewModels()).ToList();
            IAccounting accounting = await GetAccounting(accountingNumber);

            bool canModifyAccounting = _claimResolver.CanModifyAccounting(accountingNumber);
            ApplyPostingJournalViewModel postingJournal = await GetPostingJournal(accountingNumber, postingJournalKey, canModifyAccounting);
            ApplyPostingJournalResultViewModel postingJournalResult = await GetPostingJournalResult(postingJournalResultKey, canModifyAccounting);

            if (accounting == null)
            {
                return BadRequest();
            }

            AccountingViewModel accountingViewModel = _accountingViewModelConverter.Convert<IAccounting, AccountingViewModel>(accounting);
            accountingViewModel.PostingJournalKey = postingJournalKey;
            accountingViewModel.PostingJournal = postingJournal;
            accountingViewModel.PostingJournalResultKey = postingJournalResultKey;
            accountingViewModel.PostingJournalResult = postingJournalResult;
            accountingViewModel.LetterHeads = letterHeadViewModelCollection;

            return PartialView("_PresentAccountingPartial", accountingViewModel);
        }

        [HttpGet]
        [Authorize(Policy = Policies.AccountingCreatorPolicy)]
        public IActionResult StartCreatingAccounting()
        {
            return PartialView("_CreatingAccountingPartial", CreateAccountingOptionsViewModel());
        }

        [HttpGet]
        [Authorize(Policy = Policies.AccountingCreatorPolicy)]
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
        [Authorize(Policy = Policies.AccountingCreatorPolicy)]
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
        [Authorize(Policy = Policies.AccountingModifierPolicy)]
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
        [Authorize(Policy = Policies.AccountingModifierPolicy)]
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
        [Authorize(Policy = Policies.AccountingModifierPolicy)]
        public IActionResult StartCreatingAccount(int accountingNumber)
        {
            return PartialView("_CreatingAccountPartial", CreateAccountingIdentificationViewModel(accountingNumber));
        }

        [HttpGet]
        [Authorize(Policy = Policies.AccountingModifierPolicy)]
        public async Task<IActionResult> CreateAccount(int accountingNumber)
        {
            List<AccountGroupViewModel> accountGroupViewModelCollection = (await GetAccountGroupViewModels()).ToList();
            IAccounting accounting = await GetAccounting(accountingNumber);
            if (accounting == null)
            {
                return BadRequest();
            }

            AccountViewModel accountViewModel = CreateAccountViewModelForCreation<AccountViewModel>(_accountingViewModelConverter.Convert<IAccounting, AccountingIdentificationViewModel>(accounting));
            accountViewModel.CreditInfos = CreateInfoDictionaryViewModelForCreation<CreditInfoDictionaryViewModel, CreditInfoCollectionViewModel, CreditInfoViewModel>(DateTime.Today);
            accountViewModel.AccountGroups = accountGroupViewModelCollection;

            return PartialView("_EditAccountPartial", accountViewModel);
        }

        [HttpPost]
        [Authorize(Policy = Policies.AccountingModifierPolicy)]
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
        [Authorize(Policy = Policies.AccountingViewerPolicy)]
        public IActionResult StartUpdatingAccount(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return PartialView("_UpdatingAccountPartial", CreateAccountIdentificationViewModel(accountingNumber, accountNumber));
        }

        [HttpGet]
        [Authorize(Policy = Policies.AccountingViewerPolicy)]
        public async Task<IActionResult> UpdateAccount(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            List<AccountGroupViewModel> accountGroupViewModelCollection = (await GetAccountGroupViewModels()).ToList();
            IAccount account = await GetAccount<GetAccountQuery, IAccount>(accountingNumber, accountNumber, DateTime.Today);
            if (account == null)
            {
                return BadRequest();
            }

            AccountViewModel accountViewModel = _accountingViewModelConverter.Convert<IAccount, AccountViewModel>(account);
            accountViewModel.EditMode = EditMode.Edit;
            accountViewModel.AccountGroups = accountGroupViewModelCollection;

            return PartialView("_EditAccountPartial", accountViewModel);
        }

        [HttpPost]
        [Authorize(Policy = Policies.AccountingModifierPolicy)]
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
        [Authorize(Policy = Policies.AccountingModifierPolicy)]
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
        [Authorize(Policy = Policies.AccountingModifierPolicy)]
        public IActionResult StartCreatingBudgetAccount(int accountingNumber)
        {
            return PartialView("_CreatingBudgetAccountPartial", CreateAccountingIdentificationViewModel(accountingNumber));
        }

        [HttpGet]
        [Authorize(Policy = Policies.AccountingModifierPolicy)]
        public async Task<IActionResult> CreateBudgetAccount(int accountingNumber)
        {
            List<BudgetAccountGroupViewModel> budgetAccountGroupViewModelCollection = (await GetBudgetAccountGroupViewModels()).ToList();
            IAccounting accounting = await GetAccounting(accountingNumber);
            if (accounting == null)
            {
                return BadRequest();
            }

            BudgetAccountViewModel budgetAccountViewModel = CreateAccountViewModelForCreation<BudgetAccountViewModel>(_accountingViewModelConverter.Convert<IAccounting, AccountingIdentificationViewModel>(accounting));
            budgetAccountViewModel.BudgetInfos= CreateInfoDictionaryViewModelForCreation<BudgetInfoDictionaryViewModel, BudgetInfoCollectionViewModel, BudgetInfoViewModel>(DateTime.Today);
            budgetAccountViewModel.BudgetAccountGroups = budgetAccountGroupViewModelCollection;

            return PartialView("_EditBudgetAccountPartial", budgetAccountViewModel);
        }

        [HttpPost]
        [Authorize(Policy = Policies.AccountingModifierPolicy)]
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
        [Authorize(Policy = Policies.AccountingViewerPolicy)]
        public IActionResult StartUpdatingBudgetAccount(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return PartialView("_UpdatingBudgetAccountPartial", CreateAccountIdentificationViewModel(accountingNumber, accountNumber));
        }

        [HttpGet]
        [Authorize(Policy = Policies.AccountingViewerPolicy)]
        public async Task<IActionResult> UpdateBudgetAccount(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            List<BudgetAccountGroupViewModel> budgetAccountGroupViewModelCollection = (await GetBudgetAccountGroupViewModels()).ToList();
            IBudgetAccount budgetAccount = await GetAccount<GetBudgetAccountQuery, IBudgetAccount>(accountingNumber, accountNumber, DateTime.Today);
            if (budgetAccount == null)
            {
                return BadRequest();
            }

            BudgetAccountViewModel budgetAccountViewModel = _accountingViewModelConverter.Convert<IBudgetAccount, BudgetAccountViewModel>(budgetAccount);
            budgetAccountViewModel.EditMode = EditMode.Edit;
            budgetAccountViewModel.BudgetAccountGroups = budgetAccountGroupViewModelCollection;

            return PartialView("_EditBudgetAccountPartial", budgetAccountViewModel);
        }

        [HttpPost]
        [Authorize(Policy = Policies.AccountingModifierPolicy)]
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
        [Authorize(Policy = Policies.AccountingModifierPolicy)]
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
        [Authorize(Policy = Policies.AccountingModifierPolicy)]
        public IActionResult StartCreatingContactAccount(int accountingNumber)
        {
            return PartialView("_CreatingContactAccountPartial", CreateAccountingIdentificationViewModel(accountingNumber));
        }

        [HttpGet]
        [Authorize(Policy = Policies.AccountingModifierPolicy)]
        public async Task<IActionResult> CreateContactAccount(int accountingNumber)
        {
            List<PaymentTermViewModel> paymentTermViewModelCollection = (await GetPaymentTermViewModels()).ToList();
            IAccounting accounting = await GetAccounting(accountingNumber);
            if (accounting == null)
            {
                return BadRequest();
            }

            ContactAccountViewModel contactAccountViewModel = CreateAccountViewModelForCreation<ContactAccountViewModel>(_accountingViewModelConverter.Convert<IAccounting, AccountingIdentificationViewModel>(accounting));
            contactAccountViewModel.BalanceInfos = CreateInfoDictionaryViewModelForCreation<BalanceInfoDictionaryViewModel, BalanceInfoCollectionViewModel, BalanceInfoViewModel>(DateTime.Today);
            contactAccountViewModel.PaymentTerms = paymentTermViewModelCollection;

            return PartialView("_EditContactAccountPartial", contactAccountViewModel);
        }

        [HttpPost]
        [Authorize(Policy = Policies.AccountingModifierPolicy)]
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
        [Authorize(Policy = Policies.AccountingViewerPolicy)]
        public IActionResult StartUpdatingContactAccount(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return PartialView("_UpdatingContactAccountPartial", CreateAccountIdentificationViewModel(accountingNumber, accountNumber));
        }

        [HttpGet]
        [Authorize(Policy = Policies.AccountingViewerPolicy)]
        public async Task<IActionResult> UpdateContactAccount(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            List<PaymentTermViewModel> paymentTermViewModelCollection = (await GetPaymentTermViewModels()).ToList();
            IContactAccount contactAccount = await GetAccount<GetContactAccountQuery, IContactAccount>(accountingNumber, accountNumber, DateTime.Today);
            if (contactAccount == null)
            {
                return BadRequest();
            }

            ContactAccountViewModel contactAccountViewModel = _accountingViewModelConverter.Convert<IContactAccount, ContactAccountViewModel>(contactAccount);
            contactAccountViewModel.EditMode = EditMode.Edit;
            contactAccountViewModel.PaymentTerms = paymentTermViewModelCollection;

            return PartialView("_EditContactAccountPartial", contactAccountViewModel);
        }

        [HttpPost]
        [Authorize(Policy = Policies.AccountingModifierPolicy)]
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
        [Authorize(Policy = Policies.AccountingModifierPolicy)]
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
        [Authorize(Policy = Policies.AccountingModifierPolicy)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyPostingJournal(ApplyPostingJournalViewModel applyPostingJournalViewModel)
        {
            NullGuard.NotNull(applyPostingJournalViewModel, nameof(applyPostingJournalViewModel));

            if (ModelState.IsValid == false)
            {
                throw new IntranetExceptionBuilder(ErrorCode.SubmittedMessageInvalid, BuildValidationErrorMessage(ModelState))
                    .WithValidatingType(applyPostingJournalViewModel.GetType())
                    .WithValidatingField(nameof(applyPostingJournalViewModel))
                    .Build();
            }

            bool canModifyAccounting = _claimResolver.CanModifyAccounting(applyPostingJournalViewModel.AccountingNumber);
            if (canModifyAccounting == false)
            {
                return Forbid();
            }

            string postingJournalKey = await GetPostingJournalKey(applyPostingJournalViewModel.AccountingNumber);
            string postingJournalResultKey = await GetPostingJournalResultKey(applyPostingJournalViewModel.AccountingNumber);

            ApplyPostingJournalViewModel postingJournal = await GetPostingJournal(applyPostingJournalViewModel.AccountingNumber, postingJournalKey, true);
            ApplyPostingJournalResultViewModel postingJournalResult = await GetPostingJournalResult(postingJournalResultKey, true);

            IApplyPostingJournalCommand applyPostingJournalCommand = _accountingViewModelConverter.Convert<ApplyPostingJournalViewModel, ApplyPostingJournalCommand>(applyPostingJournalViewModel);
            IPostingJournalResult applyPostingJournalResult = await _commandBus.PublishAsync<IApplyPostingJournalCommand, IPostingJournalResult>(applyPostingJournalCommand);

            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModel = _accountingViewModelConverter.Convert<IPostingJournalResult, ApplyPostingJournalResultViewModel>(applyPostingJournalResult);

            await HandlePostingJournalResult(applyPostingJournalResultViewModel, postingJournalKey, postingJournal);
            await HandlePostingJournalResult(applyPostingJournalResultViewModel, postingJournalResultKey, postingJournalResult);

            return RedirectToAction("Accountings", "Accounting", new { accountingNumber = applyPostingJournalViewModel.AccountingNumber });
        }

        [HttpGet]
        public async Task<IActionResult> AccountGroups()
        {
            return View("AccountGroups", await GetAccountGroupViewModels());
        }

        [HttpGet]
        [Authorize(Policy = Policies.AccountingAdministratorPolicy)]
        public IActionResult CreateAccountGroup()
        {
            AccountGroupViewModel accountGroupViewModel = new AccountGroupViewModel
            {
                EditMode = EditMode.Create
            };

            return View("CreateAccountGroup", accountGroupViewModel);
        }

        [HttpPost]
        [Authorize(Policy = Policies.AccountingAdministratorPolicy)]
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
        [Authorize(Policy = Policies.AccountingAdministratorPolicy)]
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
        [Authorize(Policy = Policies.AccountingAdministratorPolicy)]
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
        [Authorize(Policy = Policies.AccountingAdministratorPolicy)]
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
        [Authorize(Policy = Policies.AccountingAdministratorPolicy)]
        public IActionResult CreateBudgetAccountGroup()
        {
            BudgetAccountGroupViewModel budgetAccountGroupViewModel = new BudgetAccountGroupViewModel
            {
                EditMode = EditMode.Create
            };

            return View("CreateBudgetAccountGroup", budgetAccountGroupViewModel);
        }

        [HttpPost]
        [Authorize(Policy = Policies.AccountingAdministratorPolicy)]
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
        [Authorize(Policy = Policies.AccountingAdministratorPolicy)]
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
        [Authorize(Policy = Policies.AccountingAdministratorPolicy)]
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
        [Authorize(Policy = Policies.AccountingAdministratorPolicy)]
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
        [Authorize(Policy = Policies.AccountingAdministratorPolicy)]
        public IActionResult CreatePaymentTerm()
        {
            PaymentTermViewModel paymentTermViewModel = new PaymentTermViewModel
            {
                EditMode = EditMode.Create
            };

            return View("CreatePaymentTerm", paymentTermViewModel);
        }

        [HttpPost]
        [Authorize(Policy = Policies.AccountingAdministratorPolicy)]
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
        [Authorize(Policy = Policies.AccountingAdministratorPolicy)]
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
        [Authorize(Policy = Policies.AccountingAdministratorPolicy)]
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
        [Authorize(Policy = Policies.AccountingAdministratorPolicy)]
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
        [Authorize(Policy = Policies.AccountingViewerPolicy)]
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
        [Authorize(Policy = Policies.AccountingViewerPolicy)]
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
        [Authorize(Policy = Policies.AccountingViewerPolicy)]
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
        [Authorize(Policy = Policies.AccountingModifierPolicy)]
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

            bool canModifyAccounting = _claimResolver.CanModifyAccounting(accountingNumber);
            if (canModifyAccounting == false)
            {
                return Forbid();
            }

            ApplyPostingJournalViewModel applyPostingJournalViewModel = await GetPostingJournal(accountingNumber, postingJournalKey, true);
            applyPostingJournalViewModel.ApplyPostingLines.Add(applyPostingLineViewModel);

            return GetPartialViewForPostingJournal(await SavePostingJournal(postingJournalKey, applyPostingJournalViewModel), postingJournalKey, postingJournalHeader);
        }

        [HttpPost("api/accountings/{accountingNumber}/postingjournals/{postingLineIdentifier}")]
        [Authorize(Policy = Policies.AccountingModifierPolicy)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePostingLineFromPostingJournal(int accountingNumber, string postingJournalKey, Guid postingLineIdentifier, string postingJournalHeader = null)
        {
            if (string.IsNullOrWhiteSpace(postingJournalKey))
            {
                return BadRequest();
            }

            bool canModifyAccounting = _claimResolver.CanModifyAccounting(accountingNumber);
            if (canModifyAccounting == false)
            {
                return Forbid();
            }

            ApplyPostingJournalViewModel applyPostingJournalViewModel = await GetPostingJournal(accountingNumber, postingJournalKey, true);
            applyPostingJournalViewModel.ApplyPostingLines.RemoveAll(m => m.Identifier == postingLineIdentifier);

            if (applyPostingJournalViewModel.ApplyPostingLines.Any())
            {
                applyPostingJournalViewModel = await SavePostingJournal(postingJournalKey, applyPostingJournalViewModel);
            }
            else
            {
                await DeleteFromKeyValueEntry(postingJournalKey);
            }

            return GetPartialViewForPostingJournal(applyPostingJournalViewModel, postingJournalKey, postingJournalHeader);
        }

        [HttpPost("api/accountings/{accountingNumber}/postingwarnings/{postingWarningIdentifier}")]
        [Authorize(Policy = Policies.AccountingModifierPolicy)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePostingWarningFromPostingJournalResult(int accountingNumber, string postingJournalResultKey, Guid postingWarningIdentifier)
        {
            if (string.IsNullOrWhiteSpace(postingJournalResultKey))
            {
                return BadRequest();
            }

            bool canModifyAccounting = _claimResolver.CanModifyAccounting(accountingNumber);
            if (canModifyAccounting == false)
            {
                return Forbid();
            }

            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModel = await GetPostingJournalResult(postingJournalResultKey, true);
            applyPostingJournalResultViewModel.PostingWarnings.RemoveAll(m => m.Identifier == postingWarningIdentifier);

            if (applyPostingJournalResultViewModel.PostingWarnings.Any())
            {
                applyPostingJournalResultViewModel = await SavePostingJournalResult(postingJournalResultKey, applyPostingJournalResultViewModel);
            }
            else
            {
                await DeleteFromKeyValueEntry(postingJournalResultKey);
            }

            ViewData.Add("AccountingNumber", accountingNumber);
            ViewData.Add("PostingJournalResultKey", postingJournalResultKey);

            return PartialView("_PostingWarningCollectionPartial", applyPostingJournalResultViewModel.PostingWarnings);
        }

        [HttpGet("api/accountings/{accountingNumber}/accounts/action/export/csv")]
        [Authorize(Policy = Policies.AccountingViewerPolicy)]
        public async Task<IActionResult> ExportAccountCollectionToCsv([FromRoute] int accountingNumber, [FromQuery] DateTime? statusDate = null)
        {
            IExportAccountCollectionQuery query = new ExportAccountCollectionQuery
            {
                AccountingNumber = accountingNumber,
                StatusDate = statusDate?.Date ?? DateTime.Today
            };
            byte[] csvContent = await _queryBus.QueryAsync<IExportAccountCollectionQuery, byte[]>(query);

            return File(csvContent ?? Array.Empty<byte>(), "application/csv", $"{query.StatusDate:yyyyMMdd} - Accounts.csv");
        }

        [HttpGet("api/accountings/{accountingNumber}/budgetaccounts/action/export/csv")]
        [Authorize(Policy = Policies.AccountingViewerPolicy)]
        public async Task<IActionResult> ExportBudgetAccountCollectionToCsv([FromRoute] int accountingNumber, [FromQuery] DateTime? statusDate = null)
        {
            IExportBudgetAccountCollectionQuery query = new ExportBudgetAccountCollectionQuery
            {
                AccountingNumber = accountingNumber,
                StatusDate = statusDate?.Date ?? DateTime.Today
            };
            byte[] csvContent = await _queryBus.QueryAsync<IExportBudgetAccountCollectionQuery, byte[]>(query);

            return File(csvContent ?? Array.Empty<byte>(), "application/csv", $"{query.StatusDate:yyyyMMdd} - Budget accounts.csv");
        }

        [HttpGet("api/accountings/{accountingNumber}/contactaccounts/action/export/csv")]
        [Authorize(Policy = Policies.AccountingViewerPolicy)]
        public async Task<IActionResult> ExportContactAccountCollectionToCsv([FromRoute] int accountingNumber, [FromQuery] DateTime? statusDate = null)
        {
            IExportContactAccountCollectionQuery query = new ExportContactAccountCollectionQuery
            {
                AccountingNumber = accountingNumber,
                StatusDate = statusDate?.Date ?? DateTime.Today
            };
            byte[] csvContent = await _queryBus.QueryAsync<IExportContactAccountCollectionQuery, byte[]>(query);

            return File(csvContent ?? Array.Empty<byte>(), "application/csv", $"{query.StatusDate:yyyyMMdd} - Contact accounts.csv");
        }

        [HttpGet("api/accountings/{accountingNumber}/result/annual/action/export/csv")]
        [Authorize(Policy = Policies.AccountingViewerPolicy)]
        public async Task<IActionResult> ExportAnnualResultToCsv([FromRoute] int accountingNumber, [FromQuery] DateTime? statusDate = null)
        {
            IExportBudgetAccountGroupStatusCollectionQuery query = new ExportBudgetAccountGroupStatusCollectionQuery
            {
                AccountingNumber = accountingNumber,
                StatusDate = statusDate?.Date ?? DateTime.Today
            };
            byte[] csvContent = await _queryBus.QueryAsync<IExportBudgetAccountGroupStatusCollectionQuery, byte[]>(query);

            return File(csvContent ?? Array.Empty<byte>(), "application/csv", $"{query.StatusDate:yyyyMMdd} - Annual result.csv");
        }

        [HttpGet("api/accountings/{accountingNumber}/balance/action/export/csv")]
        [Authorize(Policy = Policies.AccountingViewerPolicy)]
        public async Task<IActionResult> ExportBalanceToCsv([FromRoute] int accountingNumber, [FromQuery] DateTime? statusDate = null)
        {
            IExportAccountGroupStatusCollectionQuery query = new ExportAccountGroupStatusCollectionQuery
            {
                AccountingNumber = accountingNumber,
                StatusDate = statusDate?.Date ?? DateTime.Today
            };
            byte[] csvContent = await _queryBus.QueryAsync<IExportAccountGroupStatusCollectionQuery, byte[]>(query);

            return File(csvContent ?? Array.Empty<byte>(), "application/csv", $"{query.StatusDate:yyyyMMdd} - Balance.csv");
        }

        [HttpGet("api/accountings/{accountingNumber}/result/monthly/action/export/markdown")]
        [Authorize(Policy = Policies.AccountingViewerPolicy)]
        public async Task<IActionResult> MakeMonthlyAccountingStatementMarkdown([FromRoute] int accountingNumber, [FromQuery] DateTime? statusDate = null)
        {
            IMakeMonthlyAccountingStatementQuery query = new MakeMonthlyAccountingStatementQuery
            {
                AccountingNumber = accountingNumber,
                StatusDate = statusDate?.Date ?? DateTime.Today
            };
            byte[] markdownContent = await _queryBus.QueryAsync<IMakeMonthlyAccountingStatementQuery, byte[]>(query);

            return File(markdownContent ?? Array.Empty<byte>(), "text/markdown", $"{query.StatusDate:yyyyMMdd} - Monthly result.md");
        }

        [HttpGet("api/accountings/{accountingNumber}/result/annual/action/export/markdown")]
        [Authorize(Policy = Policies.AccountingViewerPolicy)]
        public async Task<IActionResult> MakeAnnualAccountingStatementMarkdown([FromRoute] int accountingNumber, [FromQuery] DateTime? statusDate = null)
        {
            IMakeAnnualAccountingStatementQuery query = new MakeAnnualAccountingStatementQuery
            {
                AccountingNumber = accountingNumber,
                StatusDate = statusDate?.Date ?? DateTime.Today
            };
            byte[] markdownContent = await _queryBus.QueryAsync<IMakeAnnualAccountingStatementQuery, byte[]>(query);

            return File(markdownContent ?? Array.Empty<byte>(), "text/markdown", $"{query.StatusDate:yyyyMMdd} - Annual result.md");
        }

        [HttpGet("api/accountings/{accountingNumber}/balance/action/export/markdown")]
        [Authorize(Policy = Policies.AccountingViewerPolicy)]
        public async Task<IActionResult> MakeBalanceSheetMarkdown([FromRoute] int accountingNumber, [FromQuery] DateTime? statusDate = null)
        {
            IMakeBalanceSheetQuery query = new MakeBalanceSheetQuery
            {
                AccountingNumber = accountingNumber,
                StatusDate = statusDate?.Date ?? DateTime.Today
            };
            byte[] markdownContent = await _queryBus.QueryAsync<IMakeBalanceSheetQuery, byte[]>(query);

            return File(markdownContent ?? Array.Empty<byte>(), "text/markdown", $"{query.StatusDate:yyyyMMdd} - Balance sheet.md");
        }

        [HttpGet("api/accountings/{accountingNumber}/contactaccounts/{accountNumber}/action/export/markdown")]
        [Authorize(Policy = Policies.AccountingViewerPolicy)]
        public async Task<IActionResult> MakeContactAccountStatementMarkdown([FromRoute] int accountingNumber, string accountNumber, [FromQuery] DateTime? statusDate = null)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                return BadRequest();
            }

            IMakeContactAccountStatementQuery query = new MakeContactAccountStatementQuery
            {
                AccountingNumber = accountingNumber,
                AccountNumber = accountNumber,
                StatusDate = statusDate?.Date ?? DateTime.Today
            };
            byte[] markdownContent = await _queryBus.QueryAsync<IMakeContactAccountStatementQuery, byte[]>(query);

            return File(markdownContent ?? Array.Empty<byte>(), "text/markdown", $"{query.StatusDate:yyyyMMdd} - Contact account statement ({accountNumber}).md");
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
                .Select(_accountingViewModelConverter.Convert<IAccountGroup, AccountGroupViewModel>)
                .OrderBy(accountGroupViewModel => accountGroupViewModel.Number)
                .ToList();
        }

        private async Task<IEnumerable<BudgetAccountGroupViewModel>> GetBudgetAccountGroupViewModels()
        {
            IEnumerable<IBudgetAccountGroup> budgetAccountGroups = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IBudgetAccountGroup>>(new EmptyQuery());

            return budgetAccountGroups.AsParallel()
                .Select(_accountingViewModelConverter.Convert<IBudgetAccountGroup, BudgetAccountGroupViewModel>)
                .OrderBy(budgetAccountGroupViewModel => budgetAccountGroupViewModel.Number)
                .ToList();
        }

        private async Task<IEnumerable<PaymentTermViewModel>> GetPaymentTermViewModels()
        {
            IEnumerable<IPaymentTerm> paymentTerms = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IPaymentTerm>>(new EmptyQuery());

            return paymentTerms.AsParallel()
                .Select(_accountingViewModelConverter.Convert<IPaymentTerm, PaymentTermViewModel>)
                .OrderBy(paymentTermViewModel => paymentTermViewModel.Number)
                .ToList();
        }

        private async Task<IEnumerable<LetterHeadViewModel>> GetLetterHeadViewModels()
        {
            IEnumerable<ILetterHead> letterHeads = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<ILetterHead>>(new EmptyQuery());

            return letterHeads.AsParallel()
                .Select(_commonViewModelConverter.Convert<ILetterHead, LetterHeadViewModel>)
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

        private async Task<ApplyPostingJournalViewModel> GetPostingJournal(int accountingNumber, string key, bool canModifyAccounting)
        {
            NullGuard.NotNullOrWhiteSpace(key, nameof(key));

            ApplyPostingJournalViewModel postingJournal =
                await GetObjectFromKeyValueEntry<ApplyPostingJournalViewModel>(key) ??
                new ApplyPostingJournalViewModel
                {
                    AccountingNumber = accountingNumber,
                    ApplyPostingLines = new ApplyPostingLineCollectionViewModel()
                };

            if (canModifyAccounting == false)
            {
                postingJournal.ApplyProtection();
            }

            return postingJournal;
        }

        private Task<ApplyPostingJournalViewModel> SavePostingJournal(string key, ApplyPostingJournalViewModel applyPostingJournalViewModel)
        {
            NullGuard.NotNullOrWhiteSpace(key, nameof(key))
                .NotNull(applyPostingJournalViewModel, nameof(applyPostingJournalViewModel));

            ApplyPostingLineViewModel[] orderedPostingLines = applyPostingJournalViewModel.ApplyPostingLines
                .OrderByDescending(applyPostingLine => applyPostingLine.PostingDate.UtcDateTime.Date)
                .ThenByDescending(applyPostingLine => applyPostingLine.SortOrder ?? 0)
                .ToArray();

            applyPostingJournalViewModel.ApplyPostingLines = new ApplyPostingLineCollectionViewModel();
            applyPostingJournalViewModel.ApplyPostingLines.AddRange(orderedPostingLines);

            return SaveKeyValueEntry(key, applyPostingJournalViewModel);
        }

        private Task<string> GetPostingJournalResultKey(int accountingNumber)
        {
            IGetUserSpecificKeyQuery query = new GetUserSpecificKeyQuery
            {
                KeyElementCollection = new[] { nameof(ApplyPostingJournalResultViewModel), Convert.ToString(accountingNumber) }
            };
            return _queryBus.QueryAsync<IGetUserSpecificKeyQuery, string>(query);
        }

        private async Task<ApplyPostingJournalResultViewModel> GetPostingJournalResult(string key, bool canModifyAccounting)
        {
            NullGuard.NotNullOrWhiteSpace(key, nameof(key));

            ApplyPostingJournalResultViewModel postingJournalResult =
                await GetObjectFromKeyValueEntry<ApplyPostingJournalResultViewModel>(key) ??
                new ApplyPostingJournalResultViewModel
                {
                    PostingLines = new PostingLineCollectionViewModel(),
                    PostingWarnings = new PostingWarningCollectionViewModel()
                };

            if (canModifyAccounting == false)
            {
                postingJournalResult.ApplyProtection();
            }

            return postingJournalResult;
        }

        private Task<ApplyPostingJournalResultViewModel> SavePostingJournalResult(string key, ApplyPostingJournalResultViewModel applyPostingJournalResultViewModel)
        {
            NullGuard.NotNullOrWhiteSpace(key, nameof(key))
                .NotNull(applyPostingJournalResultViewModel, nameof(applyPostingJournalResultViewModel));

            PostingWarningViewModel[] orderedPostingWarningCollection = applyPostingJournalResultViewModel.PostingWarnings
                .OrderByDescending(postingWarning => postingWarning.PostingLine.PostingDate.Date)
                .ThenByDescending(postingWarning => postingWarning.PostingLine.SortOrder)
                .ThenBy(postingWarning => (int)postingWarning.Reason)
                .ToArray();

            applyPostingJournalResultViewModel.PostingLines = new PostingLineCollectionViewModel();
            applyPostingJournalResultViewModel.PostingWarnings = new PostingWarningCollectionViewModel();
            applyPostingJournalResultViewModel.PostingWarnings.AddRange(orderedPostingWarningCollection);

            return SaveKeyValueEntry(key, applyPostingJournalResultViewModel);
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

        private async Task<T> SaveKeyValueEntry<T>(string key, T value) where T : class
        {
            NullGuard.NotNullOrWhiteSpace(key, nameof(key))
                .NotNull(value, nameof(value));

            IPushKeyValueEntryCommand command = new PushKeyValueEntryCommand
            {
                Key = key,
                Value = value
            };
            await _commandBus.PublishAsync(command);

            return value;
        }

        private async Task DeleteFromKeyValueEntry(string key)
        {
            NullGuard.NotNullOrWhiteSpace(key, nameof(key));

            IPullKeyValueEntryQuery query = new PullKeyValueEntryQuery
            {
                Key = key
            };
            IKeyValueEntry keyValueEntry = await _queryBus.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(query);
            if (keyValueEntry == null)
            {
                return;
            }

            IDeleteKeyValueEntryCommand deleteKeyValueEntryCommand = new DeleteKeyValueEntryCommand
            {
                Key = key
            };
            await _commandBus.PublishAsync(deleteKeyValueEntryCommand);
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

        private async Task HandlePostingJournalResult(ApplyPostingJournalResultViewModel applyPostingJournalResultViewModel, string postingJournalKey, ApplyPostingJournalViewModel applyPostingJournalViewModelFromKeyValueEntry)
        {
            NullGuard.NotNull(applyPostingJournalResultViewModel, nameof(applyPostingJournalResultViewModel))
                .NotNullOrWhiteSpace(postingJournalKey, nameof(postingJournalKey))
                .NotNull(applyPostingJournalViewModelFromKeyValueEntry, nameof(applyPostingJournalViewModelFromKeyValueEntry));

            Guid[] appliedPostingLineIdentifierCollection = applyPostingJournalResultViewModel.PostingLines
                .Select(appliedPostingLineViewModel => appliedPostingLineViewModel.Identifier)
                .ToArray();

            applyPostingJournalViewModelFromKeyValueEntry.ApplyPostingLines.RemoveAll(applyPostingLineViewModel => appliedPostingLineIdentifierCollection.Any(postingLineIdentifier => applyPostingLineViewModel.Identifier == postingLineIdentifier));

            if (applyPostingJournalViewModelFromKeyValueEntry.ApplyPostingLines.Any())
            {
                await SavePostingJournal(postingJournalKey, applyPostingJournalViewModelFromKeyValueEntry);
                return;
            }

            await DeleteFromKeyValueEntry(postingJournalKey);
        }

        private async Task HandlePostingJournalResult(ApplyPostingJournalResultViewModel applyPostingJournalResultViewModel, string postingJournalResultKey, ApplyPostingJournalResultViewModel applyPostingJournalResultViewModelFromKeyValueEntry)
        {
            NullGuard.NotNull(applyPostingJournalResultViewModel, nameof(applyPostingJournalResultViewModel))
                .NotNullOrWhiteSpace(postingJournalResultKey, nameof(postingJournalResultKey))
                .NotNull(applyPostingJournalResultViewModelFromKeyValueEntry, nameof(applyPostingJournalResultViewModelFromKeyValueEntry));

            PostingWarningViewModel[] postingWarningCollection = applyPostingJournalResultViewModelFromKeyValueEntry.PostingWarnings
                .Concat(applyPostingJournalResultViewModel.PostingWarnings)
                .ToArray();

            applyPostingJournalResultViewModelFromKeyValueEntry.PostingWarnings = new PostingWarningCollectionViewModel();
            applyPostingJournalResultViewModelFromKeyValueEntry.PostingWarnings.AddRange(postingWarningCollection);

            if (applyPostingJournalResultViewModelFromKeyValueEntry.PostingWarnings.Any())
            {
                await SavePostingJournalResult(postingJournalResultKey, applyPostingJournalResultViewModelFromKeyValueEntry);
                return;
            }

            await DeleteFromKeyValueEntry(postingJournalResultKey);
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

        private static string BuildValidationErrorMessage(ModelStateDictionary modelStateDictionary)
        {
            NullGuard.NotNull(modelStateDictionary, nameof(modelStateDictionary));

            StringBuilder validationErrorBuilder = new StringBuilder();

            ValidationProblemDetails validationProblemDetails = new ValidationProblemDetails(modelStateDictionary);
            foreach (KeyValuePair<string, string[]> validationError in validationProblemDetails.Errors.Where(error => string.IsNullOrWhiteSpace(error.Key) == false && error.Value.Length > 0))
            {
                foreach (string validationErrorMessage in validationError.Value.Where(value => string.IsNullOrWhiteSpace(value) == false))
                {
                    validationErrorBuilder.AppendLine($"{validationError.Key}: {validationErrorMessage}");
                }
            }

            return validationErrorBuilder.ToString();
        }

        #endregion
    }
}