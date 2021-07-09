using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    public class GetPostingLineCollectionQueryHandler : AccountingIdentificationQueryHandlerBase<IGetPostingLineCollectionQuery, IPostingLineCollection>
    {
        #region Constructor

        public GetPostingLineCollectionQueryHandler(IValidator validator, IAccountingRepository accountingRepository) 
            : base(validator, accountingRepository)
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