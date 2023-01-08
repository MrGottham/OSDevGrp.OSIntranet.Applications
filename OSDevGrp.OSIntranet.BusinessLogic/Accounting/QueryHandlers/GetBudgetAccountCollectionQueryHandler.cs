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
    internal class GetBudgetAccountCollectionQueryHandler : AccountingIdentificationQueryHandlerBase<IGetBudgetAccountCollectionQuery, IBudgetAccountCollection>
    {
        #region Constructor

        public GetBudgetAccountCollectionQueryHandler(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository)
            : base(validator, claimResolver, accountingRepository)
        {
        }

        #endregion

        #region Methods

        protected override Task<IBudgetAccountCollection> GetDataAsync(IGetBudgetAccountCollectionQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return AccountingRepository.GetBudgetAccountsAsync(query.AccountingNumber, query.StatusDate);
        }

        protected override Task<IBudgetAccountCollection> GetResultForNoDataAsync(IGetBudgetAccountCollectionQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return new BudgetAccountCollection().CalculateAsync(query.StatusDate);
        }

        #endregion
    }
}