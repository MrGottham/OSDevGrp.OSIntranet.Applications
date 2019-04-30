using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands
{
    public class CreateBudgetAccountGroupCommand : BudgetAccountGroupCommandBase, ICreateBudgetAccountGroupCommand
    {
        #region Methods

        public override IValidator Validate(IValidator validator, IAccountingRepository accountingRepository) 
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository));

            return base.Validate(validator, accountingRepository)
                .Object.ShouldBeUnknownValue(Number, number => Task.Run(async () => await accountingRepository.GetBudgetAccountGroupAsync(number) == null), GetType(), nameof(Number));
        }

        #endregion
    }
}