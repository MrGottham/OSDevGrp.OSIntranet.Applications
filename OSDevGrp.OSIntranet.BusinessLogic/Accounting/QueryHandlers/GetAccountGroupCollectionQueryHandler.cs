using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    public class GetAccountGroupCollectionQueryHandler : IQueryHandler<EmptyQuery, IEnumerable<IAccountGroup>>
    {
        #region Private variables

        private readonly IAccountingRepository _accountingRepository;

        #endregion

        #region Constructor

        public GetAccountGroupCollectionQueryHandler(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            _accountingRepository = accountingRepository;
        }

        #endregion

        #region Methods

        public Task<IEnumerable<IAccountGroup>> QueryAsync(EmptyQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return _accountingRepository.GetAccountGroupsAsync();
        }

        #endregion
    }
}