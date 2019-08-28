using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.CommandHandlers
{
    public class DeleteLetterHeadCommandHandler : LetterHeadIdentificationCommandHandlerBase<IDeleteLetterHeadCommand>
    {
        #region Constructor

        public DeleteLetterHeadCommandHandler(IValidator validator, ICommonRepository commonRepository)
            : base(validator, commonRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(IDeleteLetterHeadCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            await CommonRepository.DeleteLetterHeadAsync(command.Number);
        }

        #endregion
    }
}