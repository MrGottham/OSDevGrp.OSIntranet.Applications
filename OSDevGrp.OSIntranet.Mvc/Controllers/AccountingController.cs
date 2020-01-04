using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;

namespace OSDevGrp.OSIntranet.Mvc.Controllers
{
    [Authorize(Policy = "Accounting")]
    public class AccountingController : Controller
    {
        #region Private variables

        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly IConverter _accountingViewModelConverter = new AccountingViewModelConverter();

        #endregion

        #region Constructor

        public AccountingController(ICommandBus commandBus, IQueryBus queryBus)
        {
            NullGuard.NotNull(commandBus, nameof(commandBus))
                .NotNull(queryBus, nameof(queryBus));

            _commandBus = commandBus;
            _queryBus = queryBus;
        }

        #endregion

        #region Methods

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

        #endregion
    }
}