using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.CommandHandlers
{
    public class UpdateLetterHeadCommandHandler : LetterHeadIdentificationCommandHandlerBase<IUpdateLetterHeadCommand>
    {
        #region Constructor

        public UpdateLetterHeadCommandHandler(IValidator validator, ICommonRepository commonRepository)
            : base(validator, commonRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(IUpdateLetterHeadCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            ILetterHead letterHead = command.ToDomain();

            await CommonRepository.UpdateLetterHeadAsync(letterHead);
        }

        #endregion
    }
}