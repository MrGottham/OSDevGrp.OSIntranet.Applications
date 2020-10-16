using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    public class DeleteBudgetAccountCommandHandler : AccountIdentificationCommandHandlerBase<IDeleteBudgetAccountCommand>
    {
        #region Constructor

        public DeleteBudgetAccountCommandHandler(IValidator validator, IAccountingRepository accountingRepository, ICommonRepository commonRepository) 
            : base(validator, accountingRepository, commonRepository)
        {
        }

        #endregion

        #region Methods

        protected override Task ManageRepositoryAsync(IDeleteBudgetAccountCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            return AccountingRepository.DeleteBudgetAccountAsync(command.AccountingNumber, command.AccountNumber);
        }

        #endregion
    }
}