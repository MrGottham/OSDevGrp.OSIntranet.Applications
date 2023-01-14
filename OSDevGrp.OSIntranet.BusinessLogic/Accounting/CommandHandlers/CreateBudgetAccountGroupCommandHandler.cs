using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    internal class CreateBudgetAccountGroupCommandHandler : AccountGroupIdentificationCommandHandlerBase<ICreateBudgetAccountGroupCommand>
    {
        #region Constructor

        public CreateBudgetAccountGroupCommandHandler(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository)
            : base(validator, claimResolver, accountingRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(ICreateBudgetAccountGroupCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            IBudgetAccountGroup budgetAccountGroup = command.ToDomain();

            await AccountingRepository.CreateBudgetAccountGroupAsync(budgetAccountGroup);
        }

        #endregion
    }
}