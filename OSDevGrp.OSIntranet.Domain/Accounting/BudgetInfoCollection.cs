using System;
using System.Linq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class BudgetInfoCollection : InfoCollectionBase<IBudgetInfo, IBudgetInfoCollection>, IBudgetInfoCollection
    {
        #region Properties

        public IBudgetInfoValues ValuesForMonthOfStatusDate { get; private set; } = new BudgetInfoValues(0M, 0M);

        public IBudgetInfoValues ValuesForLastMonthOfStatusDate { get; private set; } = new BudgetInfoValues(0M, 0M);

        public IBudgetInfoValues ValuesForYearToDateOfStatusDate { get; private set; } = new BudgetInfoValues(0M, 0M);

        public IBudgetInfoValues ValuesForLastYearOfStatusDate { get; private set; } = new BudgetInfoValues(0M, 0M);

        #endregion

        #region Methods

        protected override IBudgetInfoCollection Calculate(DateTime statusDate, IBudgetInfo[] calculatedBudgetInfoCollection)
        {
            NullGuard.NotNull(calculatedBudgetInfoCollection, nameof(calculatedBudgetInfoCollection));

            IBudgetInfo budgetInfoForMonthOfStatusDate = calculatedBudgetInfoCollection
                .AsParallel()
                .SingleOrDefault(budgetInfo => budgetInfo.IsMonthOfStatusDate);
            IBudgetInfo budgetInfoForLastMonthOfStatusDate = calculatedBudgetInfoCollection
                .AsParallel()
                .SingleOrDefault(budgetInfo => budgetInfo.IsLastMonthOfStatusDate);
            IBudgetInfo[] budgetInfoCollectionForYearToDateOfStatusDate = calculatedBudgetInfoCollection
                .AsParallel()
                .Where(budgetInfo => budgetInfo.IsYearOfStatusDate)
                .ToArray();
            IBudgetInfo[] budgetInfoCollectionForLastYearOfStatusDate = calculatedBudgetInfoCollection
                .AsParallel()
                .Where(budgetInfo => budgetInfo.IsLastYearOfStatusDate)
                .ToArray();

            ValuesForMonthOfStatusDate = ToBudgetInfoValues(budgetInfoForMonthOfStatusDate);
            ValuesForLastMonthOfStatusDate = ToBudgetInfoValues(budgetInfoForLastMonthOfStatusDate);
            ValuesForYearToDateOfStatusDate = ToBudgetInfoValues(budgetInfoCollectionForYearToDateOfStatusDate);
            ValuesForLastYearOfStatusDate = ToBudgetInfoValues(budgetInfoCollectionForLastYearOfStatusDate);

            return this;
        }

        private IBudgetInfoValues ToBudgetInfoValues(IBudgetInfo budgetInfo)
        {
            return budgetInfo == null ? new BudgetInfoValues(0M, 0M) : new BudgetInfoValues(budgetInfo.Budget, budgetInfo.Posted);
        }

        private IBudgetInfoValues ToBudgetInfoValues(IBudgetInfo[] budgetInfoCollection)
        {
            NullGuard.NotNull(budgetInfoCollection, nameof(budgetInfoCollection));

            return new BudgetInfoValues(budgetInfoCollection.Sum(budgetInfo => budgetInfo.Budget), budgetInfoCollection.Sum(budgetInfo => budgetInfo.Posted));
        }

        #endregion
    }
}