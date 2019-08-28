using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    public class DeleteBudgetAccountGroupCommandHandler : AccountGroupIdentificationCommandHandlerBase<IDeleteBudgetAccountGroupCommand>
    {
        #region Constructor

        public DeleteBudgetAccountGroupCommandHandler(IValidator validator, IAccountingRepository accountingRepository)
            : base(validator, accountingRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(IDeleteBudgetAccountGroupCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            await AccountingRepository.DeleteBudgetAccountGroupAsync(command.Number);
        }

        #endregion
    }
}