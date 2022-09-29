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
    internal class ExportAccountCollectionQueryHandler : ExportDomainCollectionFromAccountingToCsvQueryHandlerBase<IExportAccountCollectionQuery, IAccount, IAccountToCsvConverter>
    {
        #region Constructor

        public ExportAccountCollectionQueryHandler(IValidator validator, IAccountingRepository accountingRepository, IStatusDateSetter statusDateSetter, IAccountToCsvConverter domainObjectToCsvConverter) 
            : base(validator, accountingRepository, statusDateSetter, domainObjectToCsvConverter, DefaultUtf8EncodingSettingsProvider.ShouldEmitUtf8Identifier)
        {
        }

        #endregion

        #region Methods

        protected override async Task<IEnumerable<IAccount>> GetExportDataAsync(IExportAccountCollectionQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            DateTime statusDate = query.StatusDate.Date;

            IAccountCollection accountCollection = await AccountingRepository.GetAccountsAsync(query.AccountingNumber, statusDate);
            if (accountCollection == null)
            {
                return Array.Empty<IAccount>();
            }

            return await accountCollection.CalculateAsync(statusDate);
        }

        #endregion
    }
}