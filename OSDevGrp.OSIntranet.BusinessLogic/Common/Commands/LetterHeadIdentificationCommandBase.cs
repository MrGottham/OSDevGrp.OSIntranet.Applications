using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Commands
{
    public abstract class LetterHeadIdentificationCommandBase : ILetterHeadIdentificationCommand
    {
        #region Properties

        public int Number { get; set; }

        #endregion

        #region Methods

        public virtual IValidator Validate(IValidator validator, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(commonRepository, nameof(commonRepository));

            return validator.Integer.ShouldBeBetween(Number, 1, 99, GetType(), nameof(Number));
        }

        #endregion
    }
}