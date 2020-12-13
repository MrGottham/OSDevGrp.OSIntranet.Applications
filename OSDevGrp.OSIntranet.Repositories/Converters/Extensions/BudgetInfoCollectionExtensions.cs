using System;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Converters.Extensions
{
    internal static class BudgetInfoCollectionExtensions
    {
        internal static void Populate(this IBudgetInfoCollection budgetInfoCollection, IBudgetAccount budgetAccount, IBudgetInfo[] budgetInfos, DateTime statusDate, DateTime statusDateForBudgetInfos)
        {
            NullGuard.NotNull(budgetInfoCollection, nameof(budgetInfoCollection))
                .NotNull(budgetAccount, nameof(budgetAccount))
                .NotNull(budgetInfos, nameof(budgetInfos));

            DateTime fromDate = new DateTime(statusDate.AddYears(-1).Year, 1, 1);

            if (budgetInfos.Length == 0)
            {
                budgetInfoCollection.Add(new BudgetInfo(budgetAccount, (short) fromDate.Year, (short) fromDate.Month, 0M, 0M));
                budgetInfoCollection.Add(new BudgetInfo(budgetAccount, (short) statusDateForBudgetInfos.Year, (short) statusDateForBudgetInfos.Month, 0M, 0M));

                budgetInfoCollection.EnsurePopulation(budgetAccount);

                return;
            }

            IBudgetInfo budgetInfoForFromDate = budgetInfos.FindInfoForFromDate(fromDate,
                (year, month, budgetInfoBeforeFromDate) => new BudgetInfo(budgetAccount, year, month, budgetInfoBeforeFromDate.Income, budgetInfoBeforeFromDate.Expenses),
                (year, month) => new BudgetInfo(budgetAccount, year, month, 0M, 0M));
            budgetInfoCollection.Add(budgetInfoForFromDate);

            budgetInfoCollection.Add(budgetInfos.Between(budgetInfoForFromDate.ToDate.AddDays(1), statusDateForBudgetInfos));

            IBudgetInfo lastBudgetInfo = budgetInfoCollection.Last();
            if (lastBudgetInfo == null || lastBudgetInfo.Year > (short) statusDateForBudgetInfos.Year || lastBudgetInfo.Year == (short) statusDateForBudgetInfos.Year && lastBudgetInfo.Month >= (short) statusDateForBudgetInfos.Month)
            {
                budgetInfoCollection.EnsurePopulation(budgetAccount);

                return;
            }

            budgetInfoCollection.Add(new BudgetInfo(budgetAccount, (short) statusDateForBudgetInfos.Year, (short) statusDateForBudgetInfos.Month, lastBudgetInfo.Income, lastBudgetInfo.Expenses));

            budgetInfoCollection.EnsurePopulation(budgetAccount);
        }

        internal static void EnsurePopulation(this IBudgetInfoCollection budgetInfoCollection, IBudgetAccount budgetAccount)
        {
            NullGuard.NotNull(budgetInfoCollection, nameof(budgetInfoCollection))
                .NotNull(budgetAccount, nameof(budgetAccount));

            budgetInfoCollection.EnsurePopulation<IBudgetInfo, IBudgetInfoCollection>(budgetInfo =>
            {
                DateTime nextBudgetInfoFromDate = budgetInfo.ToDate.AddDays(1);
                return new BudgetInfo(budgetAccount, (short) nextBudgetInfoFromDate.Year, (short) nextBudgetInfoFromDate.Month, budgetInfo.Income, budgetInfo.Expenses);
            });
        }
    }
}