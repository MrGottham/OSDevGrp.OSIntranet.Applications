using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Providers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    internal class ExportAccountGroupStatusCollectionQueryHandler : ExportDomainCollectionFromAccountingToCsvQueryHandlerBase<IExportAccountGroupStatusCollectionQuery, IAccountGroupStatus, IAccountGroupStatusToCsvConverter>
    {
        #region Constructor

        public ExportAccountGroupStatusCollectionQueryHandler(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository, IStatusDateSetter statusDateSetter, IAccountGroupStatusToCsvConverter domainObjectToCsvConverter) 
            : base(validator, claimResolver, accountingRepository, statusDateSetter, domainObjectToCsvConverter, DefaultUtf8EncodingSettingsProvider.ShouldEmitUtf8Identifier)
        {
        }

        #endregion

        #region Methods

        protected override async Task<IEnumerable<IAccountGroupStatus>> GetExportDataAsync(IExportAccountGroupStatusCollectionQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            DateTime statusDate = query.StatusDate;

            IAccountCollection accountCollection = await AccountingRepository.GetAccountsAsync(query.AccountingNumber, statusDate);
            if (accountCollection == null)
            {
                return Array.Empty<IAccountGroupStatus>();
            }

            IAccountCollection calculatedAccountCollection = await accountCollection.CalculateAsync(statusDate);
            if (calculatedAccountCollection == null)
            {
                return Array.Empty<IAccountGroupStatus>();
            }

            return await calculatedAccountCollection.GroupByAccountGroupAsync();
        }

        #endregion
    }
}