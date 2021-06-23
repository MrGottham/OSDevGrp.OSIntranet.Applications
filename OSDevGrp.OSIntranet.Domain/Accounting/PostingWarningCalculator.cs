using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class PostingWarningCalculator : IPostingWarningCalculator
    {
        #region Methods

        public async Task<IPostingWarningCollection> CalculateAsync(IPostingLine postingLine)
        {
            NullGuard.NotNull(postingLine, nameof(postingLine));

            Task<IPostingWarning>[] calculationTasks =
            {
                CalculateAsync(postingLine.Account, postingLine.AccountValuesAtPostingDate, postingLine),
                CalculateAsync(postingLine.BudgetAccount, postingLine.BudgetAccountValuesAtPostingDate, postingLine),
                CalculateAsync(postingLine.ContactAccount, postingLine.ContactAccountValuesAtPostingDate, postingLine)
            };
            IPostingWarning[] postingWarnings = await Task.WhenAll(calculationTasks);

            return BuildPostingWarningCollection(postingWarnings.Where(postingWarning => postingWarning != null));
        }

        public async Task<IPostingWarningCollection> CalculateAsync(IPostingLineCollection postingLineCollection)
        {
            NullGuard.NotNull(postingLineCollection, nameof(postingLineCollection));

            Task<IPostingWarningCollection>[] calculationTasks = postingLineCollection.Select(CalculateAsync).ToArray();
            IPostingWarningCollection[] postingWarningCollections = await Task.WhenAll(calculationTasks);

            return BuildPostingWarningCollection(postingWarningCollections.SelectMany(postingWarningCollection => postingWarningCollection));
        }

        private static Task<IPostingWarning> CalculateAsync(IAccount account, ICreditInfoValues creditInfoValues, IPostingLine postingLine)
        {
            NullGuard.NotNull(account, nameof(account))
                .NotNull(creditInfoValues, nameof(creditInfoValues))
                .NotNull(postingLine, nameof(postingLine));

            return Task.Run(() =>
            {
                if (account.AccountGroupType != AccountGroupType.Assets)
                {
                    return null;
                }

                decimal credit = creditInfoValues.Credit;
                decimal balance = creditInfoValues.Balance;

                if (balance >= 0M || Math.Abs(balance) <= Math.Abs(credit))
                {
                    return null;
                }

                return (IPostingWarning) new PostingWarning(PostingWarningReason.AccountIsOverdrawn, account, Math.Abs(balance) - Math.Abs(credit), postingLine);
            });
        }

        private static Task<IPostingWarning> CalculateAsync(IBudgetAccount budgetAccount, IBudgetInfoValues budgetInfoValues, IPostingLine postingLine)
        {
            NullGuard.NotNull(postingLine, nameof(postingLine));

            return Task.Run(() =>
            {
                if (budgetAccount == null || budgetInfoValues == null)
                {
                    return null;
                }

                decimal budget = budgetInfoValues.Budget;
                decimal posted = budgetInfoValues.Posted;

                if (budget > 0M && posted < budget)
                {
                    return new PostingWarning(PostingWarningReason.ExpectedIncomeHasNotBeenReachedYet, budgetAccount, budget - posted, postingLine);
                }

                if (budget < 0M && posted < budget)
                {
                    return new PostingWarning(PostingWarningReason.ExpectedExpensesHaveAlreadyBeenReached, budgetAccount, Math.Abs(posted) - Math.Abs(budget), postingLine);
                }

                return (IPostingWarning) null;
            });
        }

        private static Task<IPostingWarning> CalculateAsync(IContactAccount contactAccount, IContactInfoValues contactInfoValues, IPostingLine postingLine)
        {
            NullGuard.NotNull(postingLine, nameof(postingLine));

            return Task.Run(() => (IPostingWarning) null);
        }

        private static IPostingWarningCollection BuildPostingWarningCollection(IEnumerable<IPostingWarning> postingWarnings)
        {
            NullGuard.NotNull(postingWarnings, nameof(postingWarnings));

            return new PostingWarningCollection
            {
                postingWarnings
            };
        }

        #endregion
    }
}