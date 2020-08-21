using System;
using System.Collections.Generic;
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
            AccountingOptionsViewModel accountingOptionsViewModel = new AccountingOptionsViewModel
            {
                DefaultAccountingNumber = accountingNumber ?? _claimResolver.GetAccountingNumber()
            };

            return View("Accountings", accountingOptionsViewModel);
        }

        [HttpGet]
        public IActionResult StartLoadingAccountings(int? accountingNumber = null)
        {
            AccountingOptionsViewModel accountingOptionsViewModel = new AccountingOptionsViewModel
            {
                DefaultAccountingNumber = accountingNumber
            };

            return PartialView("_LoadingAccountingsPartial", accountingOptionsViewModel);
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
            AccountingIdentificationViewModel accountingIdentificationViewModel = new AccountingIdentificationViewModel
            {
                AccountingNumber = accountingNumber
            };

            return PartialView("_LoadingAccountingPartial", accountingIdentificationViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> LoadAccounting(int accountingNumber)
        {
            IGetAccountingQuery getAccountingQuery = new GetAccountingQuery
            {
                AccountingNumber = accountingNumber,
                StatusDate = DateTime.Today
            };

            Task<IEnumerable<LetterHeadViewModel>> getLetterHeadViewModelCollectionTask = GetLetterHeadViewModels();
            Task<IAccounting> getAccountingTask = _queryBus.QueryAsync<IGetAccountingQuery, IAccounting>(getAccountingQuery);
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
            AccountingOptionsViewModel accountingOptionsViewModel = new AccountingOptionsViewModel();

            return PartialView("_CreatingAccountingPartial", accountingOptionsViewModel);
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
        public Task<IActionResult> CreateAccounting(AccountingViewModel accountingViewModel)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> UpdateAccounting(AccountingViewModel accountingViewModel)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> AccountGroups()
        {
            IEnumerable<IAccountGroup> accountGroups = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IAccountGroup>>(new EmptyQuery());

            IEnumerable<AccountGroupViewModel> accountGroupViewModels = accountGroups.AsParallel()
                .Select(accountGroup => _accountingViewModelConverter.Convert<IAccountGroup, AccountGroupViewModel>(accountGroup))
                .OrderBy(accountGroupViewModel => accountGroupViewModel.Number)
                .ToList();

            return View("AccountGroups", accountGroupViewModels);
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
            IEnumerable<IBudgetAccountGroup> budgetAccountGroups = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IBudgetAccountGroup>>(new EmptyQuery());

            IEnumerable<BudgetAccountGroupViewModel> budgetAccountGroupViewModels = budgetAccountGroups.AsParallel()
                .Select(budgetAccountGroup => _accountingViewModelConverter.Convert<IBudgetAccountGroup, BudgetAccountGroupViewModel>(budgetAccountGroup))
                .OrderBy(budgetAccountGroupViewModel => budgetAccountGroupViewModel.Number)
                .ToList();

            return View("BudgetAccountGroups", budgetAccountGroupViewModels);
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
            IEnumerable<IPaymentTerm> paymentTerms =  await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IPaymentTerm>>(new EmptyQuery());

            IEnumerable<PaymentTermViewModel> paymentTermViewModels = paymentTerms.AsParallel()
                .Select(paymentTerm => _accountingViewModelConverter.Convert<IPaymentTerm, PaymentTermViewModel>(paymentTerm))
                .OrderBy(paymentTermViewModel => paymentTermViewModel.Number)
                .ToList();

            return View("PaymentTerms", paymentTermViewModels);
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

        private async Task<IEnumerable<LetterHeadViewModel>> GetLetterHeadViewModels()
        {
            IEnumerable<ILetterHead> letterHeads = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<ILetterHead>>(new EmptyQuery());

            return letterHeads.AsParallel()
                .Select(letterHead => _commonViewModelConverter.Convert<ILetterHead, LetterHeadViewModel>(letterHead))
                .OrderBy(letterHeadViewModel => letterHeadViewModel.Number)
                .ToList();
        }

        #endregion
    }
}