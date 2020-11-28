using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class BudgetAccountCollection : AccountCollectionBase<IBudgetAccount, IBudgetAccountCollection>, IBudgetAccountCollection
    {
        #region Properties

        public IBudgetInfoValues ValuesForMonthOfStatusDate { get; private set; } = new BudgetInfoValues(0M, 0M);

        public IBudgetInfoValues ValuesForLastMonthOfStatusDate { get; private set; } = new BudgetInfoValues(0M, 0M);

        public IBudgetInfoValues ValuesForYearToDateOfStatusDate { get; private set; } = new BudgetInfoValues(0M, 0M);

        public IBudgetInfoValues ValuesForLastYearOfStatusDate { get; private set; } = new BudgetInfoValues(0M, 0M);

        #endregion

        #region Methods

        protected override IBudgetAccountCollection Calculate(DateTime statusDate, IEnumerable<IBudgetAccount> calculatedBudgetAccountCollection)
        {
            NullGuard.NotNull(calculatedBudgetAccountCollection, nameof(calculatedBudgetAccountCollection));

            IBudgetAccount[] calculatedBudgetAccountArray = calculatedBudgetAccountCollection.ToArray();

            ValuesForMonthOfStatusDate = ToBudgetInfoValues(calculatedBudgetAccountArray, budgetAccount => budgetAccount.ValuesForMonthOfStatusDate);
            ValuesForLastMonthOfStatusDate = ToBudgetInfoValues(calculatedBudgetAccountArray, budgetAccount => budgetAccount.ValuesForLastMonthOfStatusDate);
            ValuesForYearToDateOfStatusDate = ToBudgetInfoValues(calculatedBudgetAccountArray, budgetAccount => budgetAccount.ValuesForYearToDateOfStatusDate);
            ValuesForLastYearOfStatusDate = ToBudgetInfoValues(calculatedBudgetAccountArray, budgetAccount => budgetAccount.ValuesForLastYearOfStatusDate);

            return this;
        }

        private static IBudgetInfoValues ToBudgetInfoValues(IBudgetAccount[] budgetAccountCollection, Func<IBudgetAccount, IBudgetInfoValues> selector)
        {
            NullGuard.NotNull(budgetAccountCollection, nameof(budgetAccountCollection))
                .NotNull(selector, nameof(selector));

            return new BudgetInfoValues(
                budgetAccountCollection.AsParallel().Sum(budgetAccount => selector(budgetAccount).Budget),
                budgetAccountCollection.AsParallel().Sum(budgetAccount => selector(budgetAccount).Posted));
        }

        #endregion
    } 
}