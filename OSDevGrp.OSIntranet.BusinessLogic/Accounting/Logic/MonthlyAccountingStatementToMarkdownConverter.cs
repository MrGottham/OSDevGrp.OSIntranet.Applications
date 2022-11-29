using System;
using Markdown;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Providers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic
{
    internal class MonthlyAccountingStatementToMarkdownConverter : PeriodicAccountingStatementToMarkdownConverterBase, IMonthlyAccountingStatementToMarkdownConverter
    {
        #region Constructor

        public MonthlyAccountingStatementToMarkdownConverter(IStatusDateProvider statusDateProvider, IClaimResolver claimResolver) 
            : base(statusDateProvider, claimResolver, DefaultFormatProvider.Create())
        {
        }

        #endregion

        #region Methods

        protected override string GetHeader(DateTime statusDate) => $"Månedsopgørelse pr. {statusDate.ToString("D", FormatProvider)}";

        protected override IMarkdownBlockElement GetTableExplanationMarkdown(DateTime statusDate)
        {
            MarkdownInlineElement explanation1Markdown = GetTextMarkdown($"*) Beløb er opgjort for perioden {statusDate.GetFirstDateOfMonth().ToString("d.", FormatProvider)} - {statusDate.ToString("d. MMMM yyyy", FormatProvider)}");
            MarkdownInlineElement explanation2Markdown = GetTextMarkdown($"**) Beløb er opgjort for perioden {statusDate.GetEndDateOfLastMonth().GetFirstDateOfMonth().ToString("d.", FormatProvider)} - {statusDate.GetEndDateOfLastMonth().ToString("d. MMMM yyyy", FormatProvider)}");

            return GetParagraphMarkdown(GetTextMarkdown($"{explanation1Markdown}{GetNewLineMarkdown()}{explanation2Markdown}"));
        }

        protected override decimal GetBudgetForColumnSet1(IBudgetAccountGroupStatus budgetAccountGroupStatus)
        {
            NullGuard.NotNull(budgetAccountGroupStatus, nameof(budgetAccountGroupStatus));

            return budgetAccountGroupStatus.ValuesForMonthOfStatusDate.Budget;
        }

        protected override decimal GetBudgetForColumnSet1(IBudgetAccount budgetAccount)
        {
            NullGuard.NotNull(budgetAccount, nameof(budgetAccount));

            return budgetAccount.ValuesForMonthOfStatusDate.Budget;
        }

        protected override decimal GetPostedForColumnSet1(IBudgetAccountGroupStatus budgetAccountGroupStatus)
        {
            NullGuard.NotNull(budgetAccountGroupStatus, nameof(budgetAccountGroupStatus));

            return budgetAccountGroupStatus.ValuesForMonthOfStatusDate.Posted;
        }

        protected override decimal GetPostedForColumnSet1(IBudgetAccount budgetAccount)
        {
            NullGuard.NotNull(budgetAccount, nameof(budgetAccount));

            return budgetAccount.ValuesForMonthOfStatusDate.Posted;
        }

        protected override decimal GetBudgetForColumnSet2(IBudgetAccountGroupStatus budgetAccountGroupStatus)
        {
            NullGuard.NotNull(budgetAccountGroupStatus, nameof(budgetAccountGroupStatus));

            return budgetAccountGroupStatus.ValuesForLastMonthOfStatusDate.Budget;
        }

        protected override decimal GetBudgetForColumnSet2(IBudgetAccount budgetAccount)
        {
            NullGuard.NotNull(budgetAccount, nameof(budgetAccount));

            return budgetAccount.ValuesForLastMonthOfStatusDate.Budget;
        }

        protected override decimal GetPostedForColumnSet2(IBudgetAccountGroupStatus budgetAccountGroupStatus)
        {
            NullGuard.NotNull(budgetAccountGroupStatus, nameof(budgetAccountGroupStatus));

            return budgetAccountGroupStatus.ValuesForLastMonthOfStatusDate.Posted;
        }

        protected override decimal GetPostedForColumnSet2(IBudgetAccount budgetAccount)
        {
            NullGuard.NotNull(budgetAccount, nameof(budgetAccount));

            return budgetAccount.ValuesForLastMonthOfStatusDate.Posted;
        }

        #endregion
    }
}