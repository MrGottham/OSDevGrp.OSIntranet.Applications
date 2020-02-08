using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    public class GetAccountingCollectionQueryHandler : IQueryHandler<EmptyQuery, IEnumerable<IAccounting>>
    {
        #region Private variables

        private readonly IAccountingRepository _accountingRepository;
        private readonly IAccountingHelper _accountingHelper;

        #endregion

        #region Constructor

        public GetAccountingCollectionQueryHandler(IAccountingRepository accountingRepository, IAccountingHelper accountingHelper)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository))
                .NotNull(accountingHelper, nameof(accountingHelper));

            _accountingRepository = accountingRepository;
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
                return new List<IAccounting>(0);
            }

            IEnumerable<IAccounting> calculatedAccountings = await Task.WhenAll(accountings.Select(accounting => accounting.CalculateAsync(DateTime.Today)).ToArray());

            return _accountingHelper.ApplyLogicForPrincipal(calculatedAccountings);
        }

        #endregion
    }
}