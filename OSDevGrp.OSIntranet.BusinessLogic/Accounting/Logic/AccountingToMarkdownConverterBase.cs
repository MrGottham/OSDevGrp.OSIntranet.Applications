using System.Threading.Tasks;
using System;
using Markdown;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Converters;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic
{
    internal abstract class AccountingToMarkdownConverterBase : DomainObjectToMarkdownConverterBase<IAccounting>, IAccountingToMarkdownConverter
    {
        #region Private variables

        private readonly IStatusDateProvider _statusDateProvider;
        private readonly IClaimResolver _claimResolver;

        #endregion

        #region Constructor

        protected AccountingToMarkdownConverterBase(IStatusDateProvider statusDateProvider, IClaimResolver claimResolver, IFormatProvider formatProvider)
            : base(formatProvider)
        {
            NullGuard.NotNull(statusDateProvider, nameof(statusDateProvider))
                .NotNull(claimResolver, nameof(claimResolver));

            _statusDateProvider = statusDateProvider;
            _claimResolver = claimResolver;
        }

        #endregion

        #region Methods

        protected sealed override async Task WriteDocumentAsync(IAccounting accounting, IMarkdownDocument markdownDocument)
        {
            NullGuard.NotNull(accounting, nameof(accounting))
                .NotNull(markdownDocument, nameof(markdownDocument));

            DateTime statusDate = _statusDateProvider.GetStatusDate();
            string markdownCreatorName = _claimResolver.GetName();
            string markdownCreatorMailAddress = _claimResolver.GetMailAddress();

            markdownDocument.Append(GetHeaderMarkdown(GetTextMarkdown(GetHeader(statusDate)), 1));
            markdownDocument.Append(GetBasicAccountingInformationMarkdown(accounting));

            await WriteContentAsync(accounting, statusDate, markdownDocument);

            markdownDocument.Append(GetCreatorInformationMarkdown(DateTime.Now, markdownCreatorName, markdownCreatorMailAddress));
        }

        protected abstract string GetHeader(DateTime statusDate);

        protected abstract Task WriteContentAsync(IAccounting accounting, DateTime statusDate, IMarkdownDocument markdownDocument);

        private IMarkdownBlockElement GetBasicAccountingInformationMarkdown(IAccounting accounting)
        {
            NullGuard.NotNull(accounting, nameof(accounting));

            return GetParagraphMarkdown(GetTextMarkdown($"Regnskab: {accounting.Name}"));
        }

        #endregion
    }
}