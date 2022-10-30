using System;
using Markdown;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Providers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic
{
    internal class AnnualAccountingStatementToMarkdownConverter : PeriodicAccountingStatementToMarkdownConverterBase, IAnnualAccountingStatementToMarkdownConverter
    {
        #region Constructor

        public AnnualAccountingStatementToMarkdownConverter(IStatusDateProvider statusDateProvider, IClaimResolver claimResolver)
            : base(statusDateProvider, claimResolver, DefaultFormatProvider.Create())
        {
        }

        #endregion

        #region Methods

        protected override string GetHeader(DateTime statusDate) => $"Årsopgørelse pr. {statusDate.ToString("D", FormatProvider)}";

        protected override IMarkdownBlockElement GetTableExplanationMarkdown(DateTime statusDate)
        {
            MarkdownInlineElement explanation1Markdown = GetTextMarkdown($"*) Beløb er opgjort for perioden {statusDate.GetFirstDateOfYear().ToString("d. MMMM", FormatProvider)} - {statusDate.ToString("d. MMMM yyyy", FormatProvider)}");
            MarkdownInlineElement explanation2Markdown = GetTextMarkdown($"**) Beløb er opgjort for perioden {statusDate.GetEndDateOfLastYear().GetFirstDateOfYear().ToString("d. MMMM", FormatProvider)} - {statusDate.GetEndDateOfLastYear().ToString("d. MMMM yyyy", FormatProvider)}");

            return GetParagraphMarkdown(GetTextMarkdown($"{explanation1Markdown}{GetNewLineMarkdown()}{explanation2Markdown}"));
        }

        protected override decimal GetBudgetForColumnSet1(IBudgetAccountGroupStatus budgetAccountGroupStatus)
        {
            NullGuard.NotNull(budgetAccountGroupStatus, nameof(budgetAccountGroupStatus));

            return budgetAccountGroupStatus.ValuesForYearToDateOfStatusDate.Budget;
        }

        protected override decimal GetBudgetForColumnSet1(IBudgetAccount budgetAccount)
        {
            NullGuard.NotNull(budgetAccount, nameof(budgetAccount));

            return budgetAccount.ValuesForYearToDateOfStatusDate.Budget;
        }

        protected override decimal GetPostedForColumnSet1(IBudgetAccountGroupStatus budgetAccountGroupStatus)
        {
            NullGuard.NotNull(budgetAccountGroupStatus, nameof(budgetAccountGroupStatus));

            return budgetAccountGroupStatus.ValuesForYearToDateOfStatusDate.Posted;
        }

        protected override decimal GetPostedForColumnSet1(IBudgetAccount budgetAccount)
        {
            NullGuard.NotNull(budgetAccount, nameof(budgetAccount));

            return budgetAccount.ValuesForYearToDateOfStatusDate.Posted;
        }

        protected override decimal GetBudgetForColumnSet2(IBudgetAccountGroupStatus budgetAccountGroupStatus)
        {
            NullGuard.NotNull(budgetAccountGroupStatus, nameof(budgetAccountGroupStatus));

            return budgetAccountGroupStatus.ValuesForLastYearOfStatusDate.Budget;
        }

        protected override decimal GetBudgetForColumnSet2(IBudgetAccount budgetAccount)
        {
            NullGuard.NotNull(budgetAccount, nameof(budgetAccount));

            return budgetAccount.ValuesForLastYearOfStatusDate.Budget;
        }

        protected override decimal GetPostedForColumnSet2(IBudgetAccountGroupStatus budgetAccountGroupStatus)
        {
            NullGuard.NotNull(budgetAccountGroupStatus, nameof(budgetAccountGroupStatus));

            return budgetAccountGroupStatus.ValuesForLastYearOfStatusDate.Posted;
        }

        protected override decimal GetPostedForColumnSet2(IBudgetAccount budgetAccount)
        {
            NullGuard.NotNull(budgetAccount, nameof(budgetAccount));

            return budgetAccount.ValuesForLastYearOfStatusDate.Posted;
        }

        #endregion
    }
}