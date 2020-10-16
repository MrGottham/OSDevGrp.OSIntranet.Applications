using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    public class GetAccountQueryHandler : AccountingIdentificationQueryHandlerBase<IGetAccountQuery, IAccount>
    {
        #region Constructor

        public GetAccountQueryHandler(IValidator validator, IAccountingRepository accountingRepository) 
            : base(validator, accountingRepository)
        {
        }

        #endregion

        #region Methods

        protected override Task<IAccount> GetDataAsync(IGetAccountQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return AccountingRepository.GetAccountAsync(query.AccountingNumber, query.AccountNumber, query.StatusDate);
        }

        #endregion
    }
}