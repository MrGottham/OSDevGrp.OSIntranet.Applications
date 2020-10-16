using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    public class GetContactAccountCollectionQueryHandler : AccountingIdentificationQueryHandlerBase<IGetContactAccountCollectionQuery, IContactAccountCollection>
    {
        #region Constructor

        public GetContactAccountCollectionQueryHandler(IValidator validator, IAccountingRepository accountingRepository)
            : base(validator, accountingRepository)
        {
        }

        #endregion

        #region Methods

        protected override Task<IContactAccountCollection> GetDataAsync(IGetContactAccountCollectionQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return AccountingRepository.GetContactAccountsAsync(query.AccountingNumber, query.StatusDate);
        }

        protected override Task<IContactAccountCollection> GetResultForNoDataAsync(IGetContactAccountCollectionQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return new ContactAccountCollection().CalculateAsync(query.StatusDate);
        }

        #endregion
    }
}