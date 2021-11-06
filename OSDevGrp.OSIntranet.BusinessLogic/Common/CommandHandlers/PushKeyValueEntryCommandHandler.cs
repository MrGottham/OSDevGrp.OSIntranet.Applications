using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.CommandHandlers
{
    public class PushKeyValueEntryCommandHandler : KeyValueEntryIdentificationCommandHandlerBase<IPushKeyValueEntryCommand>
    {
        #region Constructor

        public PushKeyValueEntryCommandHandler(IValidator validator, ICommonRepository commonRepository) 
            : base(validator, commonRepository)
        {
        }

        #endregion

        #region Methods

        protected override Task ManageRepositoryAsync(IPushKeyValueEntryCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            IKeyValueEntry keyValueEntry = command.ToDomain();

            return CommonRepository.PushKeyValueEntryAsync(keyValueEntry);
        }

        #endregion
    }
}