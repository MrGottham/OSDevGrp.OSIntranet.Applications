using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands
{
    public class DeleteBudgetAccountGroupCommand : AccountGroupIdentificationCommandBase, IDeleteBudgetAccountGroupCommand
    {
        #region Methods

        public override IValidator Validate(IValidator validator, IAccountingRepository accountingRepository) 
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository));

            IBudgetAccountGroup budgetAccountGroup = null;
            Task<IBudgetAccountGroup> budgetAccountGroupGetter = Task.Run(async () => budgetAccountGroup ?? (budgetAccountGroup = await accountingRepository.GetBudgetAccountGroupAsync(Number)));

            return base.Validate(validator, accountingRepository)
                .Object.ShouldBeKnownValue(Number, number => Task.Run(async () => await budgetAccountGroupGetter != null), GetType(), nameof(Number))
                .Object.ShouldBeDeletable(Number, number => budgetAccountGroupGetter, GetType(), nameof(Number));
        }

        #endregion
    }
}