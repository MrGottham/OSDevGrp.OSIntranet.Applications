using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core.Builders;
using OSDevGrp.OSIntranet.Core.Interfaces.Builders;
using OSDevGrp.OSIntranet.Core.Interfaces.Converters;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;

namespace OSDevGrp.OSIntranet.Core.QueryHandlers
{
    public abstract class ExportToCsvQueryHandlerBase<TExportQuery, TDomainObject, TDomainObjectToCsvConverter> : ExportQueryHandlerBase<TExportQuery, IEnumerable<TDomainObject>> where TExportQuery : IExportQuery where TDomainObject : class where TDomainObjectToCsvConverter : IDomainObjectToCsvConverter<TDomainObject>
    {
        #region Private variables

        private readonly TDomainObjectToCsvConverter _domainObjectToCsvConverter;
        private readonly bool _encoderShouldEmitUtf8Identifier;

        #endregion

        #region Constructor

        protected ExportToCsvQueryHandlerBase(TDomainObjectToCsvConverter domainObjectToCsvConverter, bool encoderShouldEmitUtf8Identifier = true)
        {
            NullGuard.NotNull(domainObjectToCsvConverter, nameof(domainObjectToCsvConverter));

            _domainObjectToCsvConverter = domainObjectToCsvConverter;
            _encoderShouldEmitUtf8Identifier = encoderShouldEmitUtf8Identifier;
        }

        #endregion

        #region Methods

        protected sealed override Task<IExportDataContentBuilder> CreateExportDataContentBuilderAsync()
        {
            IExportDataContentBuilder exportDataContentBuilder = new CsvContentBuilder<TDomainObject, IDomainObjectToCsvConverter<TDomainObject>>(_domainObjectToCsvConverter, _encoderShouldEmitUtf8Identifier);

            return Task.FromResult(exportDataContentBuilder);
        }

        #endregion
    }
}