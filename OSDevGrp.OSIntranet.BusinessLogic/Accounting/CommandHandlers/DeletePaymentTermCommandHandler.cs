using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    internal class DeletePaymentTermCommandHandler : PaymentTermIdentificationCommandHandlerBase<IDeletePaymentTermCommand>
    {
        #region Constructor

        public DeletePaymentTermCommandHandler(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository) 
            : base(validator, claimResolver, accountingRepository)
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