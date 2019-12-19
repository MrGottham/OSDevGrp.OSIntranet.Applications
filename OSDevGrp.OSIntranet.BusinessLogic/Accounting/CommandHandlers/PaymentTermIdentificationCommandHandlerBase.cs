using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.CommandHandlers;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    public abstract class PaymentTermIdentificationCommandHandlerBase<T> : CommandHandlerTransactionalBase, ICommandHandler<T> where T : IPaymentTermIdentificationCommand
    {
        #region Constructors

        protected PaymentTermIdentificationCommandHandlerBase(IValidator validator, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository));

            Validator = validator;
            AccountingRepository = accountingRepository;
        }

        #endregion

        #region Properties

        protected IValidator Validator { get; }

        protected IAccountingRepository AccountingRepository { get; }

        #endregion

        #region Methods

        public async Task ExecuteAsync(T command)
        {
            NullGuard.NotNull(command, nameof(command));

            command.Validate(Validator, AccountingRepository);

            await ManageRepositoryAsync(command);
        }

        protected abstract Task ManageRepositoryAsync(T command);

        #endregion
    }
}
