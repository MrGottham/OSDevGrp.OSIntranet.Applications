using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    public class GetAccountGroupQueryHandler : IQueryHandler<IGetAccountGroupQuery, IAccountGroup>
    {
        #region Private variables

        private readonly IAccountingRepository _accountingRepository;

        #endregion

        #region Constructor

        public GetAccountGroupQueryHandler(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            _accountingRepository = accountingRepository;
        }

        #endregion

        #region Methods

        public Task<IAccountGroup> QueryAsync(IGetAccountGroupQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return _accountingRepository.GetAccountGroupAsync(query.Number);
        }

        #endregion
    }
}