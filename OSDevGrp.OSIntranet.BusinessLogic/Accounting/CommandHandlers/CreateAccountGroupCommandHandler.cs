using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    public class CreateAccountGroupCommandHandler : AccountGroupIdentificationCommandHandlerBase<ICreateAccountGroupCommand>
    {
        #region Constructor

        public CreateAccountGroupCommandHandler(IValidator validator, IAccountingRepository accountingRepository)
            : base(validator, accountingRepository)
        {
        }

        #endregion

        #region Methods

        protected async override Task ManageRepositoryAsync(ICreateAccountGroupCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            IAccountGroup accountGroup = command.ToDomain();

            await AccountingRepository.CreateAccountGroupAsync(accountGroup);
        }

        #endregion
    }
}