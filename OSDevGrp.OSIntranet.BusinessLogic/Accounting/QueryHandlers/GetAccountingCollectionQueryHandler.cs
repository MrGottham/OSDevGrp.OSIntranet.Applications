using System.Collections.Generic;
using System.Threading.Tasks;
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

        #endregion

        #region Constructor

        public GetAccountingCollectionQueryHandler(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            _accountingRepository = accountingRepository;
        }

        #endregion

        #region Methods

        public Task<IEnumerable<IAccounting>> QueryAsync(EmptyQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return _accountingRepository.GetAccountingsAsync();
        }

        #endregion
    }
}