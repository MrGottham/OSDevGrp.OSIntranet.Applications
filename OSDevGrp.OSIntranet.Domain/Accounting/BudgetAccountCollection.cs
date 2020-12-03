using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public async Task<IReadOnlyDictionary<IBudgetAccountGroup, IBudgetAccountCollection>> GroupByBudgetAccountGroupAsync()
        {
            Task<IBudgetAccountCollection>[] groupCalculationTasks = this.GroupBy(budgetAccount => budgetAccount.BudgetAccountGroup.Number, budgetAccount => budgetAccount)
                .Select(group =>
                {
                    IBudgetAccountCollection budgetAccountCollection = new BudgetAccountCollection
                    {
                        group.AsEnumerable().OrderBy(account => account.AccountNumber).ToArray()
                    };

                    return budgetAccountCollection.CalculateAsync(StatusDate);
                })
                .ToArray();

            IBudgetAccountCollection[] calculatedBudgetAccountCollections = await Task.WhenAll(groupCalculationTasks);

            return new ReadOnlyDictionary<IBudgetAccountGroup, IBudgetAccountCollection>(calculatedBudgetAccountCollections
                .OrderBy(calculatedBudgetAccountCollection => calculatedBudgetAccountCollection.First().BudgetAccountGroup.Number)
                .ToDictionary(calculatedBudgetAccountCollection => calculatedBudgetAccountCollection.First().BudgetAccountGroup, calculatedAccountCollection => calculatedAccountCollection));
        }

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

        protected override IBudgetAccountCollection AlreadyCalculated() => this;

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