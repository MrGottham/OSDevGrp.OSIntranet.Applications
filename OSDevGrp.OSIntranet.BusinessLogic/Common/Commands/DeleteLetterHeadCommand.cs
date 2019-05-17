using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Commands
{
    public class DeleteLetterHeadCommand : LetterHeadIdentificationCommandBase, IDeleteLetterHeadCommand
    {
        #region Methods

        public override IValidator Validate(IValidator validator, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(commonRepository, nameof(commonRepository));

            ILetterHead letterHead = null;
            Task<ILetterHead> letterHeadGetter = Task.Run(async () => letterHead ?? (letterHead = await commonRepository.GetLetterHeadAsync(Number)));

            return base.Validate(validator, commonRepository)
                .Object.ShouldBeKnownValue(Number, number => Task.Run(async () => await letterHeadGetter != null), GetType(), nameof(Number))
                .Object.ShouldBeDeletable(Number, number => letterHeadGetter, GetType(), nameof(Number));
        }

        #endregion
    }
}