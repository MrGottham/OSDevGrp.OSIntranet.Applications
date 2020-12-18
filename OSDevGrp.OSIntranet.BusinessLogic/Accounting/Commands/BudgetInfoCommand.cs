using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands
{
    public class BudgetInfoCommand : InfoCommandBase, IBudgetInfoCommand
    {
        #region Properties

        public decimal Income { get; set; }

        public decimal Expenses { get; set; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator)
        {
            NullGuard.NotNull(validator, nameof(validator));

            return base.Validate(validator)
                .Decimal.ShouldBeGreaterThanOrEqualToZero(Income, GetType(), nameof(Income))
                .Decimal.ShouldBeGreaterThanOrEqualToZero(Expenses, GetType(), nameof(Expenses));
        }

        public IBudgetInfo ToDomain(IBudgetAccount budgetAccount)
        {
            NullGuard.NotNull(budgetAccount, nameof(budgetAccount));

            return new BudgetInfo(budgetAccount, Year, Month, Income, Expenses);
        }

        #endregion
    }
}