using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    internal class DeleteAccountGroupCommandHandler : AccountGroupIdentificationCommandHandlerBase<IDeleteAccountGroupCommand>
    {
        #region Constructor

        public DeleteAccountGroupCommandHandler(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository)
            : base(validator, claimResolver, accountingRepository)
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