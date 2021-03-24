using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
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
            Task<IEnumerable<LetterHeadViewModel>> getLetterHeadViewModelCollectionTask = GetLetterHeadViewModels();
            Task<IAccounting> getAccountingTask = GetAccounting(accountingNumber);
            await Task.WhenAll(
                getLetterHeadViewModelCollectionTask,
                getAccountingTask);

            IAccounting accounting = getAccountingTask.Result;
            if (accounting == null)
            {
                return BadRequest();
            }

            AccountingViewModel accountingViewModel = _accountingViewModelConverter.Convert<IAccounting, AccountingViewModel>(accounting);
            accountingViewModel.LetterHeads = getLetterHeadViewModelCollectionTask.Result.ToList();

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
        public Task<IActionResult> CreateAccount(AccountViewModel accountViewModel)
        {
            throw new NotImplementedException();
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
            Task<IAccount> getAccountTask = GetAccount<GetAccountQuery, IAccount>(accountingNumber, accountNumber);
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
        public Task<IActionResult> UpdateAccount(AccountViewModel accountViewModel)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteAccount(int accountingNumber, string accountNumber)
        {
            throw new NotImplementedException();
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
        public Task<IActionResult> CreateBudgetAccount(BudgetAccountViewModel budgetAccountViewModel)
        {
            throw new NotImplementedException();
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
            Task<IBudgetAccount> getBudgetAccountTask = GetAccount<GetBudgetAccountQuery, IBudgetAccount>(accountingNumber, accountNumber);
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
        public Task<IActionResult> UpdateBudgetAccount(BudgetAccountViewModel budgetAccountViewModel)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteBudgetAccount(int accountingNumber, string accountNumber)
        {
            throw new NotImplementedException();
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
        public Task<IActionResult> CreateContactAccount(ContactAccountViewModel contactAccountViewModel)
        {
            throw new NotImplementedException();
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
            Task<IContactAccount> getContactAccountTask = GetAccount<GetContactAccountQuery, IContactAccount>(accountingNumber, accountNumber);
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
        public Task<IActionResult> UpdateContactAccount(ContactAccountViewModel contactAccountViewModel)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteContactAccount(int accountingNumber, string accountNumber)
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

        private Task<IAccounting> GetAccounting(int accountingNumber)
        {
            IGetAccountingQuery query = new GetAccountingQuery
            {
                AccountingNumber = accountingNumber,
                StatusDate = DateTime.Today
            };

            return _queryBus.QueryAsync<IGetAccountingQuery, IAccounting>(query);
        }

        private Task<TAccount> GetAccount<TGetAccountQuery, TAccount>(int accountingNumber, string accountNumber) where TGetAccountQuery : class, IAccountIdentificationQuery, new() where TAccount : IAccountBase
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            TGetAccountQuery query = new TGetAccountQuery
            {
                AccountingNumber = accountingNumber,
                AccountNumber = accountNumber
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
                {(short) creationDate.Year, CreateInfoCollectionViewModelForCreation<TInfoCollectionViewModel, TInfoViewModel>(creationDate)},
                {(short) (creationDate.Year + 1), CreateInfoCollectionViewModelForCreation<TInfoCollectionViewModel, TInfoViewModel>(new DateTime(creationDate.Year + 1, 1, 1))}
            };

            return new TInfoDictionaryViewModelBase
            {
                Items = new ConcurrentDictionary<short, TInfoCollectionViewModel>(dictionary)
            };
        }

        private static TInfoCollectionViewModel CreateInfoCollectionViewModelForCreation<TInfoCollectionViewModel, TInfoViewModel>(DateTime creationDate) where TInfoCollectionViewModel : InfoCollectionViewModelBase<TInfoViewModel>, new() where TInfoViewModel : InfoViewModelBase, new()
        {
            IList<TInfoViewModel> collection = new List<TInfoViewModel>();
            for (short month = (short) creationDate.Month; month <= 12; month++)
            {
                collection.Add(new TInfoViewModel {Year = (short) creationDate.Year, Month = month, EditMode = EditMode.Create});
            }

            return new TInfoCollectionViewModel
            {
                Items = new ReadOnlyCollection<TInfoViewModel>(collection)
            };
        }

        #endregion
    }
}