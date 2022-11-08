using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Markdown;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Providers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic
{
    internal class BalanceSheetToMarkdownConverter : AccountingToMarkdownConverterBase, IBalanceSheetToMarkdownConverter
    {
        #region Constructor

        public BalanceSheetToMarkdownConverter(IStatusDateProvider statusDateProvider, IClaimResolver claimResolver) 
            : base(statusDateProvider, claimResolver, DefaultFormatProvider.Create())
        {
        }

        #endregion

        #region Methods

        protected override string GetHeader(DateTime statusDate) => $"Balance pr. {statusDate.ToString("D", FormatProvider)}";

        protected override async Task WriteContentAsync(IAccounting accounting, DateTime statusDate, IMarkdownDocument markdownDocument)
        {
            NullGuard.NotNull(accounting, nameof(accounting))
                .NotNull(markdownDocument, nameof(markdownDocument));

            IAccountGroupStatus[] accountGroupStatusCollection = await GetAccountGroupStatusCollectionAsync(accounting);
            if (accountGroupStatusCollection == null || accountGroupStatusCollection.Length == 0)
            {
                return;
            }

            AddBlockElementMarkdownCollection(markdownDocument, GetBlockElementMarkdownCollection(AccountGroupType.Assets.Translate(), accountGroupStatusCollection.Where(accountGroupStatus => accountGroupStatus.AccountGroupType == AccountGroupType.Assets).OrderBy(accountGroupStatus => accountGroupStatus.Number).ToArray(), accountGroupStatus => accountGroupStatus.ValuesAtStatusDate.Assets, account => account.ValuesAtStatusDate.Balance));
            AddBlockElementMarkdownCollection(markdownDocument, GetBlockElementMarkdownCollection(AccountGroupType.Liabilities.Translate(), accountGroupStatusCollection.Where(accountGroupStatus => accountGroupStatus.AccountGroupType == AccountGroupType.Liabilities).OrderBy(accountGroupStatus => accountGroupStatus.Number).ToArray(), accountGroupStatus => accountGroupStatus.ValuesAtStatusDate.Liabilities, account => account.ValuesAtStatusDate.Balance));
        }

        private async Task<IAccountGroupStatus[]> GetAccountGroupStatusCollectionAsync(IAccounting accounting)
        {
            NullGuard.NotNull(accounting, nameof(accounting));

            IAccountCollection accountCollection = accounting.AccountCollection;
            if (accountCollection == null)
            {
                return Array.Empty<IAccountGroupStatus>();
            }

            IEnumerable<IAccountGroupStatus> accountGroupStatusCollection = await accountCollection.GroupByAccountGroupAsync();
            if (accountGroupStatusCollection == null)
            {
                return Array.Empty<IAccountGroupStatus>();
            }

            return accountGroupStatusCollection.ToArray();
        }

        private void AddBlockElementMarkdownCollection(IMarkdownDocument markdownDocument, IMarkdownBlockElement[] blockElementMarkdownCollection)
        {
            NullGuard.NotNull(markdownDocument, nameof(markdownDocument))
                .NotNull(blockElementMarkdownCollection, nameof(blockElementMarkdownCollection));

            if (blockElementMarkdownCollection is { Length: > 0 })
            {
                foreach (IMarkdownBlockElement blockElementMarkdown in blockElementMarkdownCollection)
                {
                    markdownDocument.Append(blockElementMarkdown);
                }
            }
        }

        private IMarkdownBlockElement[] GetBlockElementMarkdownCollection(string tableHeader, IAccountGroupStatus[] accountGroupStatusCollection, Func<IAccountGroupStatus, decimal> valueForAccountGroupStatusGetter, Func<IAccount, decimal> valueForAccountGetter)
        {
            NullGuard.NotNullOrWhiteSpace(tableHeader, nameof(tableHeader))
                .NotNull(accountGroupStatusCollection, nameof(accountGroupStatusCollection))
                .NotNull(valueForAccountGroupStatusGetter, nameof(valueForAccountGroupStatusGetter))
                .NotNull(valueForAccountGetter, nameof(valueForAccountGetter));

            if (accountGroupStatusCollection.Length == 0)
            {
                return Array.Empty<IMarkdownBlockElement>();
            }

            return new[]
            {
                GetHeaderMarkdown(GetTextMarkdown(tableHeader), 2),
                GetTableMarkdown(tableHeader, accountGroupStatusCollection, valueForAccountGroupStatusGetter, valueForAccountGetter)
            };
        }

        private IMarkdownBlockElement GetTableMarkdown(string valueText, IAccountGroupStatus[] accountGroupStatusCollection, Func<IAccountGroupStatus, decimal> valueForAccountGroupStatusGetter, Func<IAccount, decimal> valueForAccountGetter)
        {
            NullGuard.NotNullOrWhiteSpace(valueText, nameof(valueText))
                .NotNull(accountGroupStatusCollection, nameof(accountGroupStatusCollection))
                .NotNull(valueForAccountGroupStatusGetter, nameof(valueForAccountGroupStatusGetter))
                .NotNull(valueForAccountGetter, nameof(valueForAccountGetter));

            MarkdownTableHeader markdownTableHeader = GetTableHeaderMarkdown(
                GetTableHeaderCellMarkdown(GetTextMarkdown("Kontonr."), MarkdownTableTextAlignment.Right),
                GetTableHeaderCellMarkdown(GetTextMarkdown("Kontonavn"), MarkdownTableTextAlignment.Left),
                GetTableHeaderCellMarkdown(GetTextMarkdown(valueText), MarkdownTableTextAlignment.Right));

            IList<MarkdownTableRow> markdownTableRowCollection = accountGroupStatusCollection.SelectMany(accountGroupStatus => GetTableRowMarkdownCollection(accountGroupStatus, valueForAccountGroupStatusGetter, valueForAccountGetter)).ToList();

            markdownTableRowCollection.Add(GetTableRowMarkdown(
                GetEmptyMarkdown(),
                GetTextMarkdown($"{valueText} i alt"),
                GetCurrencyMarkdown(accountGroupStatusCollection.Sum(valueForAccountGroupStatusGetter))));

            return GetTableMarkdown(markdownTableHeader, markdownTableRowCollection);
        }

        private MarkdownTableRow[] GetTableRowMarkdownCollection(IAccountGroupStatus accountGroupStatus, Func<IAccountGroupStatus, decimal> valueForAccountGroupStatusGetter, Func<IAccount, decimal> valueForAccountGetter)
        {
            NullGuard.NotNull(accountGroupStatus, nameof(accountGroupStatus))
                .NotNull(valueForAccountGroupStatusGetter, nameof(valueForAccountGroupStatusGetter))
                .NotNull(valueForAccountGetter, nameof(valueForAccountGetter));

            string name = accountGroupStatus.Name;

            MarkdownTableRow headerRowMarkdown = GetTableRowMarkdown(
                GetEmptyMarkdown(),
                GetBoldTextMarkdown(name),
                GetEmptyMarkdown());

            MarkdownTableRow totalRowMarkdown = GetTableRowMarkdown(
                GetEmptyMarkdown(),
                GetTextMarkdown($"{name} i alt"),
                GetCurrencyMarkdown(valueForAccountGroupStatusGetter(accountGroupStatus)));

            MarkdownTableRow emptyRowMarkdown = GetTableRowMarkdown(
                GetEmptyMarkdown(),
                GetEmptyMarkdown(),
                GetEmptyMarkdown());

            List<MarkdownTableRow> tableRowMarkdownCollection = new List<MarkdownTableRow>
            {
                headerRowMarkdown
            };

            tableRowMarkdownCollection.AddRange(GetTableRowMarkdownCollection(accountGroupStatus.AccountCollection, valueForAccountGetter));

            tableRowMarkdownCollection.Add(totalRowMarkdown);
            tableRowMarkdownCollection.Add(emptyRowMarkdown);

            return tableRowMarkdownCollection.ToArray();
        }

        private MarkdownTableRow[] GetTableRowMarkdownCollection(IAccountCollection accountCollection, Func<IAccount, decimal> valueForAccountGetter)
        {
            NullGuard.NotNull(accountCollection, nameof(accountCollection))
                .NotNull(valueForAccountGetter, nameof(valueForAccountGetter));

            return accountCollection.Select(account => GetTableRowMarkdown(account, valueForAccountGetter)).ToArray();
        }

        private MarkdownTableRow GetTableRowMarkdown(IAccount account, Func<IAccount, decimal> valueForAccountGetter)
        {
            NullGuard.NotNull(account, nameof(account))
                .NotNull(valueForAccountGetter, nameof(valueForAccountGetter));

            return GetTableRowMarkdown(
                GetTextMarkdown(account.AccountNumber),
                GetTextMarkdown(account.AccountName),
                GetCurrencyMarkdown(valueForAccountGetter(account)));
        }

        #endregion
    }
}