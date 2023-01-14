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
    internal class GetPostingLineCollectionQueryHandler : AccountingIdentificationQueryHandlerBase<IGetPostingLineCollectionQuery, IPostingLineCollection>
    {
        #region Constructor

        public GetPostingLineCollectionQueryHandler(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository) 
            : base(validator, claimResolver, accountingRepository)
        {
        }

        #endregion

        #region Methods

        protected override Task<IPostingLineCollection> GetDataAsync(IGetPostingLineCollectionQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return AccountingRepository.GetPostingLinesAsync(query.AccountingNumber, query.StatusDate, query.NumberOfPostingLines);
        }

        protected override Task<IPostingLineCollection> GetResultForNoDataAsync(IGetPostingLineCollectionQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return new PostingLineCollection().CalculateAsync(query.StatusDate);
        }

        #endregion
    }
}