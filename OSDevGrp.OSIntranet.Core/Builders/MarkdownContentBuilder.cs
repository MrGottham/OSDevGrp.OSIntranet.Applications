using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core.Interfaces.Converters;

namespace OSDevGrp.OSIntranet.Core.Builders
{
    public sealed class MarkdownContentBuilder<TDomainObject, TDomainObjectToMarkdownConverter> : ExportDataContentBuilderBase where TDomainObject : class where TDomainObjectToMarkdownConverter : IDomainObjectToMarkdownConverter<TDomainObject>
    {
        #region Private variables

        private readonly TDomainObjectToMarkdownConverter _domainObjectToMarkdownConverter;

        #endregion

        #region Constructor

        public MarkdownContentBuilder(TDomainObjectToMarkdownConverter domainObjectToMarkdownConverter, bool encoderShouldEmitUtf8Identifier = true)
            : base(encoderShouldEmitUtf8Identifier)
        {
            NullGuard.NotNull(domainObjectToMarkdownConverter, nameof(domainObjectToMarkdownConverter));

            _domainObjectToMarkdownConverter = domainObjectToMarkdownConverter;
        }

        #endregion

        #region Methods

        public override async Task WithContentAsync<TExportQuery, TExportData>(TExportQuery query, TExportData data)
        {
            NullGuard.NotNull(query, nameof(query))
                .NotNull(data, nameof(data));

            if (data is TDomainObject domainObject == false)
            {
                throw new ArgumentException($"Value cannot be cast to {typeof(TDomainObject).Name}.", nameof(data));
            }

            string markdownContent = await _domainObjectToMarkdownConverter.ConvertAsync(domainObject);
            if (string.IsNullOrWhiteSpace(markdownContent))
            {
                return;
            }

            await Writer.WriteAsync(markdownContent);
        }

        #endregion
    }
}