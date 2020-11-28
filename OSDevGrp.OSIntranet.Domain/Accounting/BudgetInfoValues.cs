using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    internal class BudgetInfoValues : IBudgetInfoValues
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

        #endregion
    }
}