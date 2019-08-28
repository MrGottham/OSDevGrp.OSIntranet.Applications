using OSDevGrp.OSIntranet.BusinessLogic.Common.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Queries
{
    public abstract class LetterHeadIdentificationQueryBase : ILetterHeadIdentificationQuery
    {
        #region Properties

        public int Number { get; set; }

        #endregion

        #region Methods

        public IValidator Validate(IValidator validator, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(commonRepository, nameof(commonRepository));

            return validator.ValidateLetterHeadIdentifier(Number, GetType(), nameof(Number));
        }

        #endregion
    }
}