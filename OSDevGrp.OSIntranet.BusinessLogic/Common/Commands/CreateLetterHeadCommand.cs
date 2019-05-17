using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Commands
{
    public class CreateLetterHeadCommand : LetterHeadCommandBase, ICreateLetterHeadCommand
    {
        #region Methods

        public override IValidator Validate(IValidator validator, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(commonRepository, nameof(commonRepository));

            return base.Validate(validator, commonRepository)
                .Object.ShouldBeUnknownValue(Number, number => Task.Run(async () => await commonRepository.GetLetterHeadAsync(number) != null), GetType(), nameof(Number));
        }

        #endregion
    }
}