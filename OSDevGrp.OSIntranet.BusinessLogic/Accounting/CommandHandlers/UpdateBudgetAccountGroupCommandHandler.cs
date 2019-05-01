using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    public class UpdateBudgetAccountGroupCommandHandler : AccountGroupIdentificationCommandHandlerBase<IUpdateBudgetAccountGroupCommand>
    {
        #region Constructor

        public UpdateBudgetAccountGroupCommandHandler(IValidator validator, IAccountingRepository accountingRepository)
            : base(validator, accountingRepository)
        {
        }

        #endregion

        #region Methods

        protected async override Task ManageRepositoryAsync(IUpdateBudgetAccountGroupCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            IBudgetAccountGroup budgetAccountGroup = command.ToDomain();

            await AccountingRepository.UpdateBudgetAccountGroupAsync(budgetAccountGroup);
        }

        #endregion
    }
}