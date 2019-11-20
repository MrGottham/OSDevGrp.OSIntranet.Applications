using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    public class GetAccountingQueryHandler : IQueryHandler<IGetAccountingQuery, IAccounting>
    {
        #region Private variables

        private readonly IValidator _validator;
        private readonly IAccountingRepository _accountingRepository;

        #endregion

        #region Constructor

        public GetAccountingQueryHandler(IValidator validator, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository));

            _validator = validator;
            _accountingRepository = accountingRepository;
        }

        #endregion

        #region Methods

        public async Task<IAccounting> QueryAsync(IGetAccountingQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            query.Validate(_validator, _accountingRepository);

            return await _accountingRepository.GetAccountingAsync(query.AccountingNumber, query.StatusDate);
        }

        #endregion
    }
}