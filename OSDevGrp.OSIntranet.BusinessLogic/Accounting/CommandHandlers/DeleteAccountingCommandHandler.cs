using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    internal class DeleteAccountingCommandHandler : AccountingIdentificationCommandHandlerBase<IDeleteAccountingCommand>
    {
        #region Constructor

        public DeleteAccountingCommandHandler(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository, ICommonRepository commonRepository)
            : base(validator, claimResolver, accountingRepository, commonRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(IDeleteAccountingCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            await AccountingRepository.DeleteAccountingAsync(command.AccountingNumber);
        }

        #endregion
    }
}