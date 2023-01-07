using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands
{
    public class UpdateBudgetAccountGroupCommand : BudgetAccountGroupCommandBase, IUpdateBudgetAccountGroupCommand
    {
        #region Methods

        public override IValidator Validate(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository) 
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(claimResolver, nameof(claimResolver))
                .NotNull(accountingRepository, nameof(accountingRepository));

            return base.Validate(validator, claimResolver, accountingRepository)
                .Object.ShouldBeKnownValue(Number, number => Task.Run(async () => await GetBudgetAccountGroupAsync(accountingRepository) != null), GetType(), nameof(Number));
        }

        #endregion
    }
}