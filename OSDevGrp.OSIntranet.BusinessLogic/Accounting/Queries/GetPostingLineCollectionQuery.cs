using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Queries
{
    public class GetPostingLineCollectionQuery : AccountingIdentificationQueryBase, IGetPostingLineCollectionQuery
    {
        #region Properties

        public int NumberOfPostingLines { get; set; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository));

            return base.Validate(validator, accountingRepository)
                .Integer.ShouldBeBetween(NumberOfPostingLines, 1, 512, GetType(), nameof(NumberOfPostingLines));
        }

        #endregion
    }
}