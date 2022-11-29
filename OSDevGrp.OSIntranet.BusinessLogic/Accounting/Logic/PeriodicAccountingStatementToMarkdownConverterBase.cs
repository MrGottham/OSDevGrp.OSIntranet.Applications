using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using Markdown;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic
{
    internal abstract class PeriodicAccountingStatementToMarkdownConverterBase : AccountingToMarkdownConverterBase, IPeriodicAccountingStatementToMarkdownConverter
    {
        #region Constructor

        protected PeriodicAccountingStatementToMarkdownConverterBase(IStatusDateProvider statusDateProvider, IClaimResolver claimResolver, IFormatProvider formatProvider) 
            : base(statusDateProvider, claimResolver, formatProvider)
        {
        }

        #endregion

        #region Methods

        protected override async Task WriteContentAsync(IAccounting accounting, DateTime statusDate, IMarkdownDocument markdownDocument)
        {
            NullGuard.NotNull(accounting, nameof(accounting))
                .NotNull(markdownDocument, nameof(markdownDocument));

            IBudgetAccountGroupStatus[] budgetAccountGroupStatusCollection = await GetBudgetAccountGroupStatusCollection(accounting);
            if (budgetAccountGroupStatusCollection == null || budgetAccountGroupStatusCollection.Length == 0)
            {
                return;
            }

            IMarkdownBlockElement tableExplanationMarkdown = GetTableExplanationMarkdown(statusDate);
            if (tableExplanationMarkdown != null)
            {
                markdownDocument.Append(tableExplanationMarkdown);
            }

            markdownDocument.Append(GetTableMarkdown(budgetAccountGroupStatusCollection));
        }

        protected virtual IMarkdownBlockElement GetTableExplanationMarkdown(DateTime statusDate)
        {
            return null;
        }

        protected abstract decimal GetBudgetForColumnSet1(IBudgetAccountGroupStatus budgetAccountGroupStatus);

        protected abstract decimal GetBudgetForColumnSet1(IBudgetAccount budgetAccount);

        protected abstract decimal GetPostedForColumnSet1(IBudgetAccountGroupStatus budgetAccountGroupStatus);

        protected abstract decimal GetPostedForColumnSet1(IBudgetAccount budgetAccount);

        protected abstract decimal GetBudgetForColumnSet2(IBudgetAccountGroupStatus budgetAccountGroupStatus);

        protected abstract decimal GetBudgetForColumnSet2(IBudgetAccount budgetAccount);

        protected abstract decimal GetPostedForColumnSet2(IBudgetAccountGroupStatus budgetAccountGroupStatus);

        protected abstract decimal GetPostedForColumnSet2(IBudgetAccount budgetAccount);

        private async Task<IBudgetAccountGroupStatus[]> GetBudgetAccountGroupStatusCollection(IAccounting accounting)
        {
            NullGuard.NotNull(accounting, nameof(accounting));

            IBudgetAccountCollection budgetAccountCollection = accounting.BudgetAccountCollection;
            if (budgetAccountCollection == null)
            {
                return Array.Empty<IBudgetAccountGroupStatus>();
            }

            IEnumerable<IBudgetAccountGroupStatus> budgetAccountGroupStatusCollection = await budgetAccountCollection.GroupByBudgetAccountGroupAsync();
            if (budgetAccountGroupStatusCollection == null)
            {
                return Array.Empty<IBudgetAccountGroupStatus>();
            }

            return budgetAccountGroupStatusCollection.ToArray();
        }

        private IMarkdownBlockElement GetTableMarkdown(IBudgetAccountGroupStatus[] budgetAccountGroupStatusCollection)
        {
            NullGuard.NotNull(budgetAccountGroupStatusCollection, nameof(budgetAccountGroupStatusCollection));

            MarkdownTableHeader markdownTableHeader = GetTableHeaderMarkdown(
                GetTableHeaderCellMarkdown(GetTextMarkdown("Kontonr."), MarkdownTableTextAlignment.Right),
                GetTableHeaderCellMarkdown(GetTextMarkdown("Kontonavn"), MarkdownTableTextAlignment.Left),
                GetTableHeaderCellMarkdown(GetTextMarkdown("Budget *"), MarkdownTableTextAlignment.Right),
                GetTableHeaderCellMarkdown(GetTextMarkdown("Bogført *"), MarkdownTableTextAlignment.Right),
                GetTableHeaderCellMarkdown(GetTextMarkdown("Budget **"), MarkdownTableTextAlignment.Right),
                GetTableHeaderCellMarkdown(GetTextMarkdown("Bogført **"), MarkdownTableTextAlignment.Right));

            IList<MarkdownTableRow> markdownTableRowCollection = budgetAccountGroupStatusCollection.SelectMany(GetTableRowMarkdownCollection).ToList();

            markdownTableRowCollection.Add(GetTableRowMarkdown(
                GetEmptyMarkdown(),
                GetTextMarkdown("Resultat i alt"),
                GetCurrencyMarkdown(budgetAccountGroupStatusCollection.Sum(GetBudgetForColumnSet1)),
                GetCurrencyMarkdown(budgetAccountGroupStatusCollection.Sum(GetPostedForColumnSet1)),
                GetCurrencyMarkdown(budgetAccountGroupStatusCollection.Sum(GetBudgetForColumnSet2)),
                GetCurrencyMarkdown(budgetAccountGroupStatusCollection.Sum(GetPostedForColumnSet2))));

            return GetTableMarkdown(markdownTableHeader, markdownTableRowCollection);
        }

        private MarkdownTableRow[] GetTableRowMarkdownCollection(IBudgetAccountGroupStatus budgetAccountGroupStatus)
        {
            NullGuard.NotNull(budgetAccountGroupStatus, nameof(budgetAccountGroupStatus));

            string name = budgetAccountGroupStatus.Name;

            MarkdownTableRow headerRowMarkdown = GetTableRowMarkdown(
                GetEmptyMarkdown(),
                GetBoldTextMarkdown(name),
                GetEmptyMarkdown(),
                GetEmptyMarkdown(),
                GetEmptyMarkdown(),
                GetEmptyMarkdown());

            MarkdownTableRow totalRowMarkdown = GetTableRowMarkdown(
                GetEmptyMarkdown(),
                GetTextMarkdown($"{name} i alt"),
                GetCurrencyMarkdown(GetBudgetForColumnSet1(budgetAccountGroupStatus)),
                GetCurrencyMarkdown(GetPostedForColumnSet1(budgetAccountGroupStatus)),
                GetCurrencyMarkdown(GetBudgetForColumnSet2(budgetAccountGroupStatus)),
                GetCurrencyMarkdown(GetPostedForColumnSet2(budgetAccountGroupStatus)));

            MarkdownTableRow emptyRowMarkdown = GetTableRowMarkdown(
                GetEmptyMarkdown(),
                GetEmptyMarkdown(),
                GetEmptyMarkdown(),
                GetEmptyMarkdown(),
                GetEmptyMarkdown(),
                GetEmptyMarkdown());

            List<MarkdownTableRow> markdownTableRowCollection = new List<MarkdownTableRow>
            {
                headerRowMarkdown
            };

            markdownTableRowCollection.AddRange(GetTableRowMarkdownCollection(budgetAccountGroupStatus.BudgetAccountCollection));

            markdownTableRowCollection.Add(totalRowMarkdown);
            markdownTableRowCollection.Add(emptyRowMarkdown);

            return markdownTableRowCollection.ToArray();
        }

        private MarkdownTableRow[] GetTableRowMarkdownCollection(IBudgetAccountCollection budgetAccountCollection)
        {
            NullGuard.NotNull(budgetAccountCollection, nameof(budgetAccountCollection));

            return budgetAccountCollection.Select(GetTableRowMarkdown).ToArray();
        }

        private MarkdownTableRow GetTableRowMarkdown(IBudgetAccount budgetAccount)
        {
            NullGuard.NotNull(budgetAccount, nameof(budgetAccount));

            return GetTableRowMarkdown(
                GetTextMarkdown(budgetAccount.AccountNumber),
                GetTextMarkdown(budgetAccount.AccountName),
                GetCurrencyMarkdown(GetBudgetForColumnSet1(budgetAccount)),
                GetCurrencyMarkdown(GetPostedForColumnSet1(budgetAccount)),
                GetCurrencyMarkdown(GetBudgetForColumnSet2(budgetAccount)),
                GetCurrencyMarkdown(GetPostedForColumnSet2(budgetAccount)));
        }

        #endregion
    }
}