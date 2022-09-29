using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Providers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    internal class ExportContactAccountCollectionQueryHandler : ExportDomainCollectionFromAccountingToCsvQueryHandlerBase<IExportContactAccountCollectionQuery, IContactAccount, IContactAccountToCsvConverter>
    {
        #region Constructor

        public ExportContactAccountCollectionQueryHandler(IValidator validator, IAccountingRepository accountingRepository, IStatusDateSetter statusDateSetter, IContactAccountToCsvConverter domainObjectToCsvConverter) 
            : base(validator, accountingRepository, statusDateSetter, domainObjectToCsvConverter, DefaultUtf8EncodingSettingsProvider.ShouldEmitUtf8Identifier)
        {
        }

        #endregion

        #region Methods

        protected override async Task<IEnumerable<IContactAccount>> GetExportDataAsync(IExportContactAccountCollectionQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            DateTime statusDate = query.StatusDate.Date;

            IContactAccountCollection contactAccountCollection = await AccountingRepository.GetContactAccountsAsync(query.AccountingNumber, statusDate);
            if (contactAccountCollection == null)
            {
                return Array.Empty<IContactAccount>();
            }

            return await contactAccountCollection.CalculateAsync(statusDate);
        }

        #endregion
    }
}