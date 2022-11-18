using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Converters;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic
{
    internal abstract class AccountToMarkdownConverterBase<TAccount> : DomainObjectToMarkdownConverterBase<TAccount>, IAccountToMarkdownConverter<TAccount> where TAccount : class, IAccountBase
    {
        #region Private variables

        private readonly IStatusDateProvider _statusDateProvider;
        private readonly IClaimResolver _claimResolver;

        #endregion

        #region Constructor

        protected AccountToMarkdownConverterBase(IStatusDateProvider statusDateProvider, IClaimResolver claimResolver, IFormatProvider formatProvider) 
            : base(formatProvider)
        {
            NullGuard.NotNull(statusDateProvider, nameof(statusDateProvider))
                .NotNull(claimResolver, nameof(claimResolver));

            _statusDateProvider = statusDateProvider;
            _claimResolver = claimResolver;
        }

        #endregion

        #region Methods

        protected sealed override async Task WriteDocumentAsync(TAccount account, IMarkdownDocument markdownDocument)
        {
            NullGuard.NotNull(account, nameof(account))
                .NotNull(markdownDocument, nameof(markdownDocument));

            IAccounting accounting = account.Accounting;

            DateTime statusDate = _statusDateProvider.GetStatusDate();
            string markdownCreatorName = _claimResolver.GetName();
            string markdownCreatorMailAddress = _claimResolver.GetMailAddress();

            await WriteHeaderAsync(accounting, account, statusDate, markdownDocument);
            await WriteContentAsync(accounting, account, statusDate, markdownDocument);
            await WriteFooterAsync(accounting, markdownCreatorName, markdownCreatorMailAddress, markdownDocument);
        }

        protected virtual IEnumerable<IMarkdownBlockElement> GetHeaderMarkdownCollection(IAccounting accounting, TAccount account, DateTime statusDate) => Array.Empty<IMarkdownBlockElement>();

        protected virtual IMarkdownBlockElement GetContentHeaderMarkdown(IAccounting accounting, TAccount account, DateTime statusDate) => null;

        protected virtual IEnumerable<IMarkdownBlockElement> GetContentExplanationMarkdownCollection(IAccounting accounting, TAccount account, DateTime statusDate) => Array.Empty<IMarkdownBlockElement>();

        protected virtual IEnumerable<IPostingLine> GetPostingLineCollectionForContent(IPostingLineCollection postingLineCollection, DateTime statusDate) => postingLineCollection;

        protected virtual decimal GetPostingValue(IPostingLine postingLine) => postingLine.PostingValue;

        protected abstract decimal GetBalance(IPostingLine postingLine);

        private async Task WriteHeaderAsync(IAccounting accounting, TAccount account, DateTime statusDate, IMarkdownDocument markdownDocument)
        {
            NullGuard.NotNull(accounting, nameof(accounting))
                .NotNull(account, nameof(account))
                .NotNull(markdownDocument, nameof(markdownDocument));

            await WriteAsync(GetHeaderMarkdownCollection(accounting, account, statusDate), markdownDocument);
        }

        private async Task WriteContentAsync(IAccounting accounting, TAccount account, DateTime statusDate, IMarkdownDocument markdownDocument)
        {
            NullGuard.NotNull(accounting, nameof(accounting))
                .NotNull(account, nameof(account))
                .NotNull(markdownDocument, nameof(markdownDocument));

            IMarkdownBlockElement contentHeaderMarkdown = GetContentHeaderMarkdown(accounting, account, statusDate);
            if (contentHeaderMarkdown != null)
            {
                await WriteAsync(contentHeaderMarkdown, markdownDocument);
            }

            await WriteAsync(GetContentExplanationMarkdownCollection(accounting, account, statusDate), markdownDocument);
            await WriteAsync(GetTableMarkdown(GetPostingLineCollectionForContent(account.PostingLineCollection.Ordered(), statusDate)), markdownDocument);
        }

        private async Task WriteFooterAsync(IAccounting accounting, string markdownCreatorName, string markdownCreatorMailAddress, IMarkdownDocument markdownDocument)
        {
            NullGuard.NotNull(accounting, nameof(accounting))
                .NotNull(markdownDocument, nameof(markdownDocument));

            await WriteAsync(GetCreatedForInformationMarkdown(accounting), markdownDocument);
            await WriteAsync(GetCreatorInformationMarkdown(DateTime.Now, markdownCreatorName, markdownCreatorMailAddress), markdownDocument);
        }

        private IMarkdownBlockElement GetTableMarkdown(IEnumerable<IPostingLine> postingLineCollection)
        {
            NullGuard.NotNull(postingLineCollection, nameof(postingLineCollection));

            MarkdownTableHeader markdownTableHeader = GetTableHeaderMarkdown(
                GetTableHeaderCellMarkdown(GetTextMarkdown("Dato"), MarkdownTableTextAlignment.Right),
                GetTableHeaderCellMarkdown(GetTextMarkdown("Tekst"), MarkdownTableTextAlignment.Left),
                GetTableHeaderCellMarkdown(GetTextMarkdown("Beløb"), MarkdownTableTextAlignment.Right),
                GetTableHeaderCellMarkdown(GetTextMarkdown("Saldo"), MarkdownTableTextAlignment.Right));

            return GetTableMarkdown(markdownTableHeader, postingLineCollection.Select(GetTableRowMarkdown).ToArray());
        }

        private MarkdownTableRow GetTableRowMarkdown(IPostingLine postingLine)
        {
            NullGuard.NotNull(postingLine, nameof(postingLine));

            return GetTableRowMarkdown(
                GetTextMarkdown(postingLine.PostingDate.ToString("d", FormatProvider)),
                GetTextMarkdown(postingLine.Details),
                GetCurrencyMarkdown(GetPostingValue(postingLine), false),
                GetCurrencyMarkdown(GetBalance(postingLine), false));
        }

        private static async Task WriteAsync(IEnumerable<IMarkdownBlockElement> markdownBlockElementCollection, IMarkdownDocument markdownDocument)
        {
            NullGuard.NotNull(markdownBlockElementCollection, nameof(markdownBlockElementCollection))
                .NotNull(markdownDocument, nameof(markdownDocument));

            foreach (IMarkdownBlockElement markdownBlockElement in markdownBlockElementCollection)
            {
                await WriteAsync(markdownBlockElement, markdownDocument);
            }
        }

        private static Task WriteAsync(IMarkdownBlockElement markdownBlockElement, IMarkdownDocument markdownDocument)
        {
            NullGuard.NotNull(markdownBlockElement, nameof(markdownBlockElement))
                .NotNull(markdownDocument, nameof(markdownDocument));

            markdownDocument.Append(markdownBlockElement);

            return Task.CompletedTask;
        }

        private static IMarkdownBlockElement GetCreatedForInformationMarkdown(IAccounting accounting)
        {
            NullGuard.NotNull(accounting, nameof(accounting));

            MarkdownInlineElement createdForMarkdown = GetTextMarkdown("Udskrevet for");

            ILetterHead letterHead = accounting.LetterHead;
            return letterHead == null
                ? GetParagraphMarkdown(GetItalicTextMarkdown($"{createdForMarkdown} {GetBoldTextMarkdown(accounting.Name)}"))
                : GetParagraphMarkdown(GetItalicTextMarkdown($"{createdForMarkdown} {GetLetterHeadMarkdown(letterHead)}"));
        }

        private static MarkdownInlineElement GetLetterHeadMarkdown(ILetterHead letterHead)
        {
            NullGuard.NotNull(letterHead, nameof(letterHead));

            string[] letterHeadLineCollection =
            {
                letterHead.Line1,
                letterHead.Line2,
                letterHead.Line3,
                letterHead.Line4,
                letterHead.Line5,
                letterHead.Line6,
                letterHead.Line7
            };

            StringBuilder letterHeadBuilder = new StringBuilder();
            foreach (string letterHeadLine in letterHeadLineCollection.Where(value => string.IsNullOrWhiteSpace(value) == false))
            {
                if (letterHeadBuilder.Length > 0)
                {
                    letterHeadBuilder.Append(GetTextMarkdown($", {letterHeadLine}"));
                    continue;
                }

                letterHeadBuilder.Append(GetBoldTextMarkdown(letterHeadLine));
            }

            return letterHeadBuilder.Length > 0
                ? GetTextMarkdown(letterHeadBuilder.ToString())
                : GetBoldTextMarkdown(letterHead.Name);
        }

        #endregion
    }
}