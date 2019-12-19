using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    public class DeletePaymentTermCommandHandler : PaymentTermIdentificationCommandHandlerBase<IDeletePaymentTermCommand>
    {
        #region Constructor

        public DeletePaymentTermCommandHandler(IValidator validator, IAccountingRepository accountingRepository) 
            : base(validator, accountingRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(IDeletePaymentTermCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            await AccountingRepository.DeletePaymentTermAsync(command.Number);
        }

        #endregion
    }
}
