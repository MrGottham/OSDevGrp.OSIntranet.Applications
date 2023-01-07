using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    internal class DeleteBudgetAccountGroupCommandHandler : AccountGroupIdentificationCommandHandlerBase<IDeleteBudgetAccountGroupCommand>
    {
        #region Constructor

        public DeleteBudgetAccountGroupCommandHandler(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository)
            : base(validator, claimResolver, accountingRepository)
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