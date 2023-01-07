using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.CommandHandlers;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
	internal abstract class AccountingIdentificationCommandHandlerBase<T> : CommandHandlerTransactionalBase, ICommandHandler<T> where T : IAccountingIdentificationCommand
    {
        #region Constructor

        protected AccountingIdentificationCommandHandlerBase(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository, ICommonRepository commonRepository)
        {
	        NullGuard.NotNull(validator, nameof(validator))
		        .NotNull(claimResolver, nameof(claimResolver))
		        .NotNull(accountingRepository, nameof(accountingRepository))
		        .NotNull(commonRepository, nameof(commonRepository));

            Validator = validator;
            ClaimResolver = claimResolver;
            AccountingRepository = accountingRepository;
            CommonRepository = commonRepository;
        }

        #endregion

        #region Properties

        protected IValidator Validator { get; }

        protected IClaimResolver ClaimResolver { get; }

        protected IAccountingRepository AccountingRepository { get; }

        protected ICommonRepository CommonRepository { get; }

        #endregion

        #region Methods

        public async Task ExecuteAsync(T command)
        {
            NullGuard.NotNull(command, nameof(command));

            command.Validate(Validator, ClaimResolver, AccountingRepository, CommonRepository);

            await ManageRepositoryAsync(command);
        }

        protected abstract Task ManageRepositoryAsync(T command);

        #endregion
    }
}