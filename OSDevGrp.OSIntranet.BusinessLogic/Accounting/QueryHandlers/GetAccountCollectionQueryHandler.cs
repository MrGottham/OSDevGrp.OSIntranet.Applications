using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    internal class GetAccountCollectionQueryHandler : AccountingIdentificationQueryHandlerBase<IGetAccountCollectionQuery, IAccountCollection>
    {
        #region Constructor

        public GetAccountCollectionQueryHandler(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository) 
            : base(validator, claimResolver, accountingRepository)
        {
        }

        #endregion

        #region Methods

        protected override Task<IAccountCollection> GetDataAsync(IGetAccountCollectionQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return AccountingRepository.GetAccountsAsync(query.AccountingNumber, query.StatusDate);
        }

        protected override Task<IAccountCollection> GetResultForNoDataAsync(IGetAccountCollectionQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return new AccountCollection().CalculateAsync(query.StatusDate);
        }

        #endregion
    }
}