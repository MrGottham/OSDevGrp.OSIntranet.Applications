using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    internal class GetBudgetAccountGroupCollectionQueryHandler : IQueryHandler<EmptyQuery, IEnumerable<IBudgetAccountGroup>>
    {
        #region Private variables

        private readonly IAccountingRepository _accountingRepository;
        private readonly IClaimResolver _claimResolver;

        #endregion

        #region Constructor

        public GetBudgetAccountGroupCollectionQueryHandler(IAccountingRepository accountingRepository, IClaimResolver claimResolver)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository))
                .NotNull(claimResolver, nameof(claimResolver));

            _accountingRepository = accountingRepository;
            _claimResolver = claimResolver;
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<IBudgetAccountGroup>> QueryAsync(EmptyQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            IBudgetAccountGroup[] budgetAccountGroupCollection = (await _accountingRepository.GetBudgetAccountGroupsAsync() ?? Array.Empty<IBudgetAccountGroup>()).ToArray();
            if (budgetAccountGroupCollection.Length == 0)
            {
                return budgetAccountGroupCollection;
            }

            if (_claimResolver.IsAccountingAdministrator())
            {
                return budgetAccountGroupCollection;
            }

            foreach (IBudgetAccountGroup budgetAccountGroup in budgetAccountGroupCollection)
            {
                budgetAccountGroup.ApplyProtection();
            }

            return budgetAccountGroupCollection;
        }

        #endregion
    }
}