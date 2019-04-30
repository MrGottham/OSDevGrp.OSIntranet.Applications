using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
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

        #endregion
    }
}