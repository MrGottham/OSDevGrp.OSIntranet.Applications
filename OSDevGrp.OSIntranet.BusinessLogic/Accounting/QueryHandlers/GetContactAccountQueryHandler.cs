using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    public class GetContactAccountQueryHandler : AccountingIdentificationQueryHandlerBase<IGetContactAccountQuery, IContactAccount>
    {
        #region Constructor

        public GetContactAccountQueryHandler(IValidator validator, IAccountingRepository accountingRepository) 
            : base(validator, accountingRepository)
        {
        }

        #endregion

        #region Methods

        protected override Task<IContactAccount> GetDataAsync(IGetContactAccountQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return AccountingRepository.GetContactAccountAsync(query.AccountingNumber, query.AccountNumber, query.StatusDate);
        }

        #endregion
    }
}