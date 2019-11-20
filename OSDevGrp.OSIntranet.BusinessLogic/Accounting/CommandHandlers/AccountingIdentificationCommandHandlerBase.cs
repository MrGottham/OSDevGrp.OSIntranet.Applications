using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.CommandHandlers;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    public abstract class AccountingIdentificationCommandHandlerBase<T> : CommandHandlerTransactionalBase, ICommandHandler<T> where T : IAccountingIdentificationCommand
    {
        #region Constructor

        protected AccountingIdentificationCommandHandlerBase(IValidator validator, IAccountingRepository accountingRepository, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository))
                .NotNull(commonRepository, nameof(commonRepository));

            Validator = validator;
            AccountingRepository = accountingRepository;
            CommonRepository = commonRepository;
        }

        #endregion

        #region Properties

        protected IValidator Validator { get; }

        protected IAccountingRepository AccountingRepository { get; }

        protected ICommonRepository CommonRepository { get; }

        #endregion

        #region Methods

        public async Task ExecuteAsync(T command)
        {
            NullGuard.NotNull(command, nameof(command));

            command.Validate(Validator, AccountingRepository, CommonRepository);

            await ManageRepositoryAsync(command);
        }

        protected abstract Task ManageRepositoryAsync(T command);

        #endregion
    }
}