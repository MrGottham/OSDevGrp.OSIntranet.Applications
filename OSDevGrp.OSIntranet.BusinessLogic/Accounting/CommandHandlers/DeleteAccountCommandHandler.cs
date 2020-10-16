using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    public class DeleteAccountCommandHandler : AccountIdentificationCommandHandlerBase<IDeleteAccountCommand>
    {
        #region Constructor

        public DeleteAccountCommandHandler(IValidator validator, IAccountingRepository accountingRepository, ICommonRepository commonRepository) 
            : base(validator, accountingRepository, commonRepository)
        {
        }

        #endregion

        #region Methods

        protected override Task ManageRepositoryAsync(IDeleteAccountCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            return AccountingRepository.DeleteAccountAsync(command.AccountingNumber, command.AccountNumber);
        }

        #endregion
    }
}