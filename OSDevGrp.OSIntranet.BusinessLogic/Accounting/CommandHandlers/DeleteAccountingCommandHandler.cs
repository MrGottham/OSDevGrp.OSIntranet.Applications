using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    public class DeleteAccountingCommandHandler : AccountingIdentificationCommandHandlerBase<IDeleteAccountingCommand>
    {
        #region Constructor

        public DeleteAccountingCommandHandler(IValidator validator, IAccountingRepository accountingRepository, ICommonRepository commonRepository)
            : base(validator, accountingRepository, commonRepository)
        {
        }

        #endregion

        #region Methods

        protected async override Task ManageRepositoryAsync(IDeleteAccountingCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            await AccountingRepository.DeleteAccountingAsync(command.AccountingNumber);
        }

        #endregion
    }
}