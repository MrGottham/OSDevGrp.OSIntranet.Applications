using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    internal class GetAccountQueryHandler : AccountingIdentificationQueryHandlerBase<IGetAccountQuery, IAccount>
    {
        #region Constructor

        public GetAccountQueryHandler(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository) 
            : base(validator, claimResolver, accountingRepository)
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