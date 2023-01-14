using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    internal class UpdatePaymentTermCommandHandler : PaymentTermIdentificationCommandHandlerBase<IUpdatePaymentTermCommand>
    {
        #region Constructor

        public UpdatePaymentTermCommandHandler(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository) 
            : base(validator, claimResolver, accountingRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(IUpdatePaymentTermCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            IPaymentTerm paymentTerm = command.ToDomain();

            await AccountingRepository.UpdatePaymentTermAsync(paymentTerm);
        }

        #endregion
    }
}