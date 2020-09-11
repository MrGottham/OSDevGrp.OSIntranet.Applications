using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Providers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.QueryHandlers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers
{
    public class ExportContactCollectionQueryHandler : ExportToCsvQueryHandlerBase<IExportContactCollectionQuery, IContact, IContactToCsvConverter>
    {
        #region Private variables

        private readonly IQueryHandler<IGetContactCollectionQuery, IEnumerable<IContact>> _getContactCollectionQueryHandler;

        #endregion

        #region Constructor

        public ExportContactCollectionQueryHandler(IQueryHandler<IGetContactCollectionQuery, IEnumerable<IContact>> getContactCollectionQueryHandler, IContactToCsvConverter domainObjectToCsvConverter)
            : base(domainObjectToCsvConverter, DefaultUtf8EncodingSettingsProvider.ShouldEmitUtf8Identifier)
        {
            NullGuard.NotNull(getContactCollectionQueryHandler, nameof(getContactCollectionQueryHandler));

            _getContactCollectionQueryHandler = getContactCollectionQueryHandler;
        }

        #endregion

        #region Methods

        protected override Task<IEnumerable<IContact>> GetExportDataAsync(IExportContactCollectionQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return _getContactCollectionQueryHandler.QueryAsync(query.ToGetContactCollectionQuery());
        }

        #endregion
    }
}