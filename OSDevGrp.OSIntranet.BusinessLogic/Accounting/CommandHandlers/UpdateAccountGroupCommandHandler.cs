using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    public class UpdateAccountGroupCommandHandler : AccountGroupIdentificationCommandHandlerBase<IUpdateAccountGroupCommand>
    {
        #region Constructor

        public UpdateAccountGroupCommandHandler(IValidator validator, IAccountingRepository accountingRepository)
            : base(validator, accountingRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(IUpdateAccountGroupCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            IAccountGroup accountGroup = command.ToDomain();

            await AccountingRepository.UpdateAccountGroupAsync(accountGroup);
        }

        #endregion
    }
}