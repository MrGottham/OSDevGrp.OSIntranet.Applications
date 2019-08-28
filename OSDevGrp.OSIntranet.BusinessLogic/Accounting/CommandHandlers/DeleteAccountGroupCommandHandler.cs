using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    public class DeleteAccountGroupCommandHandler : AccountGroupIdentificationCommandHandlerBase<IDeleteAccountGroupCommand>
    {
        #region Constructor

        public DeleteAccountGroupCommandHandler(IValidator validator, IAccountingRepository accountingRepository)
            : base(validator, accountingRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(IDeleteAccountGroupCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            await AccountingRepository.DeleteAccountGroupAsync(command.Number);
        }

        #endregion
    }
}