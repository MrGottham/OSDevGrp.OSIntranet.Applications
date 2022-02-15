using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class BudgetInfoValues : IBudgetInfoValues
    {
        #region Constructor

        public BudgetInfoValues(decimal budget, decimal posted)
        {
            Budget = budget;
            Posted = posted;
        }

        #endregion

        #region Properties

        public decimal Budget { get; }

        public decimal Posted { get; }

        public decimal Available => BudgetInfo.CalculateAvailable(Budget, Posted);

        #endregion
    }
}