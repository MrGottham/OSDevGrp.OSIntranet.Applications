using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.CommandHandlers
{
    public class DeleteKeyValueEntryCommandHandler : KeyValueEntryIdentificationCommandHandlerBase<IDeleteKeyValueEntryCommand>
    {
        #region Constructor

        public DeleteKeyValueEntryCommandHandler(IValidator validator, ICommonRepository commonRepository) 
            : base(validator, commonRepository)
        {
        }

        #endregion

        #region Methods

        protected override Task ManageRepositoryAsync(IDeleteKeyValueEntryCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            return CommonRepository.DeleteKeyValueEntryAsync(command.Key);
        }

        #endregion
    }
}