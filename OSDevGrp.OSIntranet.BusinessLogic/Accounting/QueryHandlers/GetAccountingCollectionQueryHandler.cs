using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
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
    public class GetAccountingCollectionQueryHandler : IQueryHandler<EmptyQuery, IEnumerable<IAccounting>>
    {
        #region Private variables

        private readonly IAccountingRepository _accountingRepository;
        private readonly IClaimResolver _claimResolver;
        private readonly IAccountingHelper _accountingHelper;

        #endregion

        #region Constructor

        public GetAccountingCollectionQueryHandler(IAccountingRepository accountingRepository, IClaimResolver claimResolver, IAccountingHelper accountingHelper)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository))
                .NotNull(claimResolver, nameof(claimResolver))
                .NotNull(accountingHelper, nameof(accountingHelper));

            _accountingRepository = accountingRepository;
            _claimResolver = claimResolver;
            _accountingHelper = accountingHelper;
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<IAccounting>> QueryAsync(EmptyQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            IEnumerable<IAccounting> accountings = await _accountingRepository.GetAccountingsAsync();
            if (accountings == null)
            {
                return Array.Empty<IAccounting>();
            }

            IEnumerable<IAccounting> calculatedAccountings = await Task.WhenAll(accountings.Where(accounting => _claimResolver.CanAccessAccounting(accounting.Number)).Select(accounting => accounting.CalculateAsync(DateTime.Today)).ToArray());

            return _accountingHelper.ApplyLogicForPrincipal(calculatedAccountings);
        }

        #endregion
    }
}