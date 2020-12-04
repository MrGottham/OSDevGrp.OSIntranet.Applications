using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    public class GetCreditorAccountCollectionQueryHandler : AccountingIdentificationQueryHandlerBase<IGetCreditorAccountCollectionQuery, IContactAccountCollection>
    {
        #region Constructor

        public GetCreditorAccountCollectionQueryHandler(IValidator validator, IAccountingRepository accountingRepository) 
            : base(validator, accountingRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task<IContactAccountCollection> GetDataAsync(IGetCreditorAccountCollectionQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            IContactAccountCollection contactAccountCollection = await AccountingRepository.GetContactAccountsAsync(query.AccountingNumber, query.StatusDate);
            if (contactAccountCollection == null)
            {
                return null;
            }

            IContactAccountCollection calculatedAccountCollection = await contactAccountCollection.CalculateAsync(query.StatusDate);
            if (calculatedAccountCollection == null)
            {
                return null;
            }

            return await calculatedAccountCollection.FindCreditorsAsync();
        }

        protected override Task<IContactAccountCollection> GetResultForNoDataAsync(IGetCreditorAccountCollectionQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return new ContactAccountCollection().CalculateAsync(query.StatusDate);
        }

        #endregion
    }
}