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
    internal class GetAccountGroupCollectionQueryHandler : IQueryHandler<EmptyQuery, IEnumerable<IAccountGroup>>
    {
        #region Private variables

        private readonly IAccountingRepository _accountingRepository;
        private readonly IClaimResolver _claimResolver;

        #endregion

        #region Constructor

        public GetAccountGroupCollectionQueryHandler(IAccountingRepository accountingRepository, IClaimResolver claimResolver)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository))
                .NotNull(claimResolver, nameof(claimResolver));

            _accountingRepository = accountingRepository;
            _claimResolver = claimResolver;
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<IAccountGroup>> QueryAsync(EmptyQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            IAccountGroup[] accountGroupCollection = (await _accountingRepository.GetAccountGroupsAsync() ?? Array.Empty<IAccountGroup>()).ToArray();
            if (accountGroupCollection.Length == 0)
            {
                return accountGroupCollection;
            }

            if (_claimResolver.IsAccountingAdministrator())
            {
                return accountGroupCollection;
            }

            foreach (IAccountGroup accountGroup in accountGroupCollection)
            {
                accountGroup.ApplyProtection();
            }

            return accountGroupCollection;
        }

        #endregion
    }
}