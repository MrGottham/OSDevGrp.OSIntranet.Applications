using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;

namespace OSDevGrp.OSIntranet.Mvc.Controllers
{
    [Authorize(Policy = "Accounting")]
    public class AccountingController : Controller
    {
        #region Private variables

        private readonly IQueryBus _queryBus;
        private readonly IConverter _accountingViewModelConverter = new AccountingViewModelConverter();

        #endregion

        #region Constructor

        public AccountingController(IQueryBus queryBus)
        {
            NullGuard.NotNull(queryBus, nameof(queryBus));

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
        public async Task<IActionResult> BudgetAccountGroups()
        {
            IEnumerable<IBudgetAccountGroup> budgetAccountGroups = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IBudgetAccountGroup>>(new EmptyQuery());

            IEnumerable<BudgetAccountGroupViewModel> budgetAccountGroupViewModels = budgetAccountGroups.AsParallel()
                .Select(budgetAccountGroup => _accountingViewModelConverter.Convert<IBudgetAccountGroup, BudgetAccountGroupViewModel>(budgetAccountGroup))
                .OrderBy(budgetAccountGroupViewModel => budgetAccountGroupViewModel.Number)
                .ToList();

            return View("BudgetAccountGroups", budgetAccountGroupViewModels);
        }

        #endregion
    }
}