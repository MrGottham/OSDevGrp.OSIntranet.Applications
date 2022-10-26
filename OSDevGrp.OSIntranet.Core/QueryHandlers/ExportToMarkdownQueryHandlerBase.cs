using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core.Builders;
using OSDevGrp.OSIntranet.Core.Interfaces.Builders;
using OSDevGrp.OSIntranet.Core.Interfaces.Converters;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;

namespace OSDevGrp.OSIntranet.Core.QueryHandlers
{
    public abstract class ExportToMarkdownQueryHandlerBase<TExportQuery, TDomainObject, TDomainObjectToMarkdownConverter> : ExportQueryHandlerBase<TExportQuery, TDomainObject> where TExportQuery : IExportQuery where TDomainObject : class where TDomainObjectToMarkdownConverter : IDomainObjectToMarkdownConverter<TDomainObject>
    {
        #region Private variables

        private readonly TDomainObjectToMarkdownConverter _domainObjectToMarkdownConverter;
        private readonly bool _encoderShouldEmitUtf8Identifier;

        #endregion

        #region Constructor

        protected ExportToMarkdownQueryHandlerBase(TDomainObjectToMarkdownConverter domainObjectToMarkdownConverter, bool encoderShouldEmitUtf8Identifier = true)
        {
            NullGuard.NotNull(domainObjectToMarkdownConverter, nameof(domainObjectToMarkdownConverter));

            _domainObjectToMarkdownConverter = domainObjectToMarkdownConverter;
            _encoderShouldEmitUtf8Identifier = encoderShouldEmitUtf8Identifier;
        }

        #endregion

        #region Methods

        protected sealed override Task<IExportDataContentBuilder> CreateExportDataContentBuilderAsync()
        {
            IExportDataContentBuilder exportDataContentBuilder = new MarkdownContentBuilder<TDomainObject, TDomainObjectToMarkdownConverter>(_domainObjectToMarkdownConverter, _encoderShouldEmitUtf8Identifier);

            return Task.FromResult(exportDataContentBuilder);
        }

        #endregion
    }
}