using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<IBudgetAccountGroupStatus>> GroupByBudgetAccountGroupAsync()
        {
            Task<IBudgetAccountGroupStatus>[] groupCalculationTasks = this.AsParallel()
                .GroupBy(budgetAccount => budgetAccount.BudgetAccountGroup.Number, budgetAccount => budgetAccount)
                .Select(group =>
                {
                    IBudgetAccountGroup budgetAccountGroup = group.First().BudgetAccountGroup;
                    IBudgetAccountCollection budgetAccountCollection = new BudgetAccountCollection
                    {
                        group.AsEnumerable().AsParallel().OrderBy(account => account.AccountNumber).ToArray()
                    };

                    return budgetAccountGroup.CalculateAsync(StatusDate, budgetAccountCollection);
                })
                .ToArray();

            IBudgetAccountGroupStatus[] budgetAccountGroupStatusCollections = await Task.WhenAll(groupCalculationTasks);

            return budgetAccountGroupStatusCollections.OrderBy(budgetAccountGroupStatus => budgetAccountGroupStatus.Number).ToArray();
        }

        protected override IBudgetAccountCollection Calculate(DateTime statusDate, IReadOnlyCollection<IBudgetAccount> calculatedBudgetAccountCollection)
        {
            NullGuard.NotNull(calculatedBudgetAccountCollection, nameof(calculatedBudgetAccountCollection));

            ValuesForMonthOfStatusDate = ToBudgetInfoValues(calculatedBudgetAccountCollection, budgetAccount => budgetAccount.ValuesForMonthOfStatusDate);
            ValuesForLastMonthOfStatusDate = ToBudgetInfoValues(calculatedBudgetAccountCollection, budgetAccount => budgetAccount.ValuesForLastMonthOfStatusDate);
            ValuesForYearToDateOfStatusDate = ToBudgetInfoValues(calculatedBudgetAccountCollection, budgetAccount => budgetAccount.ValuesForYearToDateOfStatusDate);
            ValuesForLastYearOfStatusDate = ToBudgetInfoValues(calculatedBudgetAccountCollection, budgetAccount => budgetAccount.ValuesForLastYearOfStatusDate);

            return this;
        }

        protected override IBudgetAccountCollection AlreadyCalculated() => this;

        private static IBudgetInfoValues ToBudgetInfoValues(IReadOnlyCollection<IBudgetAccount> budgetAccountCollection, Func<IBudgetAccount, IBudgetInfoValues> selector)
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