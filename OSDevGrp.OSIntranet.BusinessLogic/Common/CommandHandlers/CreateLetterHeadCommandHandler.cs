using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.CommandHandlers
{
    public class CreateLetterHeadCommandHandler : LetterHeadIdentificationCommandHandlerBase<ICreateLetterHeadCommand>
    {
        #region Constructor

        public CreateLetterHeadCommandHandler(IValidator validator, ICommonRepository commonRepository)
            : base(validator, commonRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(ICreateLetterHeadCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            ILetterHead letterHead = command.ToDomain();

            await CommonRepository.CreateLetterHeadAsync(letterHead);
        }

        #endregion
    }
}