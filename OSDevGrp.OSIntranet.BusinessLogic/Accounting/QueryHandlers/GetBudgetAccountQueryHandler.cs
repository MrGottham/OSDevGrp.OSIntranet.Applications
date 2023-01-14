using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    internal class GetBudgetAccountQueryHandler : AccountingIdentificationQueryHandlerBase<IGetBudgetAccountQuery, IBudgetAccount>
    {
        #region Constructor

        public GetBudgetAccountQueryHandler(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository) 
            : base(validator, claimResolver, accountingRepository)
        {
        }

        #endregion

        #region Methods

        protected override Task<IBudgetAccount> GetDataAsync(IGetBudgetAccountQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return AccountingRepository.GetBudgetAccountAsync(query.AccountingNumber, query.AccountNumber, query.StatusDate);
        }

        #endregion
    }
}