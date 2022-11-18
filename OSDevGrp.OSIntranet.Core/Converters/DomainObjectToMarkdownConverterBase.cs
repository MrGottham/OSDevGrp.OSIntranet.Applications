using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Markdown;
using OSDevGrp.OSIntranet.Core.Interfaces.Converters;

namespace OSDevGrp.OSIntranet.Core.Converters
{
    public abstract class DomainObjectToMarkdownConverterBase<TDomainObject> : IDomainObjectToMarkdownConverter<TDomainObject> where TDomainObject : class
    {
        #region Constructor

        protected DomainObjectToMarkdownConverterBase(IFormatProvider formatProvider)
        {
            NullGuard.NotNull(formatProvider, nameof(formatProvider));

            FormatProvider = formatProvider;
        }

        #endregion

        #region Properties

        protected IFormatProvider FormatProvider { get; }

        #endregion

        #region Method

        public async Task<string> ConvertAsync(TDomainObject domainObject)
        {
            NullGuard.NotNull(domainObject, nameof(domainObject));

            IMarkdownDocument markdownDocument = new MarkdownDocument();

            await WriteDocumentAsync(domainObject, markdownDocument);

            string markdownContent = markdownDocument.ToString();

            return string.IsNullOrWhiteSpace(markdownContent) == false ? markdownContent : null;
        }

        protected abstract Task WriteDocumentAsync(TDomainObject domainObject, IMarkdownDocument markdownDocument);

        protected IMarkdownBlockElement GetCreatorInformationMarkdown(DateTime creationTime, string creatorName, string creatorMailAddress)
        {
            MarkdownInlineElement createdMarkdown = GetTextMarkdown($"Udskrevet den {creationTime.ToString("d", FormatProvider)} kl. {creationTime.ToString("t", FormatProvider)}");

            if (string.IsNullOrWhiteSpace(creatorName) == false && string.IsNullOrWhiteSpace(creatorMailAddress) == false)
            {
                MarkdownInlineElement creatorMailAddressMarkdown = GetLinkMarkdown(creatorName, new Uri($"mailto:{creatorMailAddress}"));

                return GetParagraphMarkdown(GetItalicTextMarkdown($"{createdMarkdown} af {creatorMailAddressMarkdown}"));
            }

            if (string.IsNullOrWhiteSpace(creatorName) == false)
            {
                MarkdownInlineElement creatorNameMarkdown = GetTextMarkdown(creatorName);

                return GetParagraphMarkdown(GetItalicTextMarkdown($"{createdMarkdown} af {creatorNameMarkdown}"));
            }

            if (string.IsNullOrWhiteSpace(creatorMailAddress) == false)
            {
                MarkdownInlineElement creatorMailAddressMarkdown = GetLinkMarkdown(creatorMailAddress, new Uri($"mailto:{creatorMailAddress}"));

                return GetParagraphMarkdown(GetItalicTextMarkdown($"{createdMarkdown} af {creatorMailAddressMarkdown}"));
            }

            return GetParagraphMarkdown(GetItalicTextMarkdown($"{createdMarkdown}"));
        }

        protected MarkdownInlineElement GetCurrencyMarkdown(decimal value, bool hideZero = true)
        {
            return value == 0M && hideZero ? GetEmptyMarkdown() : GetTextMarkdown(value.ToString("c", FormatProvider));
        }

        protected static IMarkdownBlockElement GetHeaderMarkdown(MarkdownInlineElement markdownInlineElement, int level)
        {
            NullGuard.NotNull(markdownInlineElement, nameof(markdownInlineElement));

            return new MarkdownHeader(markdownInlineElement, level);
        }

        protected static IMarkdownBlockElement GetParagraphMarkdown(MarkdownInlineElement markdownInlineElement)
        {
            NullGuard.NotNull(markdownInlineElement, nameof(markdownInlineElement));

            return new MarkdownParagraph(markdownInlineElement);
        }

        public static IMarkdownBlockElement GetTableMarkdown(MarkdownTableHeader markdownTableHeader, IEnumerable<MarkdownTableRow> markdownTableRowCollection)
        {
            NullGuard.NotNull(markdownTableHeader, nameof(markdownTableHeader))
                .NotNull(markdownTableRowCollection, nameof(markdownTableRowCollection));

            return new MarkdownTable(markdownTableHeader, markdownTableRowCollection);
        }

        protected static MarkdownTableHeader GetTableHeaderMarkdown(params MarkdownTableHeaderCell[] markdownTableHeaderCellCollection)
        {
            NullGuard.NotNull(markdownTableHeaderCellCollection, nameof(markdownTableHeaderCellCollection));

            return new MarkdownTableHeader(markdownTableHeaderCellCollection);
        }

        protected static MarkdownTableHeaderCell GetTableHeaderCellMarkdown(MarkdownInlineElement markdownInlineElement, MarkdownTableTextAlignment markdownTableTextAlignment)
        {
            NullGuard.NotNull(markdownInlineElement, nameof(markdownInlineElement));

            return new MarkdownTableHeaderCell(markdownInlineElement, markdownTableTextAlignment);
        }

        protected static MarkdownTableRow GetTableRowMarkdown(params MarkdownInlineElement[] markdownInlineElementCollection)
        {
            NullGuard.NotNull(markdownInlineElementCollection, nameof(markdownInlineElementCollection));

            return new MarkdownTableRow(markdownInlineElementCollection);
        }

        protected static MarkdownInlineElement GetTextMarkdown(string text)
        {
            NullGuard.NotNullOrWhiteSpace(text, nameof(text));

            return new MarkdownText(text);
        }

        protected static MarkdownInlineElement GetItalicTextMarkdown(string text)
        {
            NullGuard.NotNullOrWhiteSpace(text, nameof(text));

            return GetEmphasisMarkdown(text, '*');
        }

        protected static MarkdownInlineElement GetBoldTextMarkdown(string text)
        {
            NullGuard.NotNullOrWhiteSpace(text, nameof(text));

            return GetStrongEmphasisMarkdown(text);
        }

        protected static MarkdownInlineElement GetEmphasisMarkdown(string text, char emphasisChar)
        {
            NullGuard.NotNullOrWhiteSpace(text, nameof(text))
                .NotNull(emphasisChar, nameof(emphasisChar));

            return new MarkdownEmphasis(text, emphasisChar);
        }

        protected static MarkdownInlineElement GetStrongEmphasisMarkdown(string text)
        {
            NullGuard.NotNullOrWhiteSpace(text, nameof(text));

            return new MarkdownStrongEmphasis(text);
        }

        protected static MarkdownInlineElement GetLinkMarkdown(string text, Uri link)
        {
            NullGuard.NotNullOrWhiteSpace(text, nameof(text))
                .NotNull(link, nameof(link));

            return new MarkdownLink(text, link.AbsoluteUri);
        }

        protected static MarkdownInlineElement GetEmptyMarkdown()
        {
            return new MarkdownText(string.Empty);
        }

        protected static MarkdownInlineElement GetNewLineMarkdown()
        {
            return new MarkdownText($"   {Environment.NewLine}");
        }

        #endregion
    }
}