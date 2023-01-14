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
    internal class ExportBudgetAccountCollectionQueryHandler : ExportDomainCollectionFromAccountingToCsvQueryHandlerBase<IExportBudgetAccountCollectionQuery, IBudgetAccount, IBudgetAccountToCsvConverter>
    {
        #region Constructor

        public ExportBudgetAccountCollectionQueryHandler(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository, IStatusDateSetter statusDateSetter, IBudgetAccountToCsvConverter domainObjectToCsvConverter) 
            : base(validator, claimResolver, accountingRepository, statusDateSetter, domainObjectToCsvConverter, DefaultUtf8EncodingSettingsProvider.ShouldEmitUtf8Identifier)
        {
        }

        #endregion

        #region Methods

        protected override async Task<IEnumerable<IBudgetAccount>> GetExportDataAsync(IExportBudgetAccountCollectionQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            DateTime statusDate = query.StatusDate.Date;

            IBudgetAccountCollection budgetAccountCollection = await AccountingRepository.GetBudgetAccountsAsync(query.AccountingNumber, statusDate);
            if (budgetAccountCollection == null)
            {
                return Array.Empty<IBudgetAccount>();
            }

            return await budgetAccountCollection.CalculateAsync(statusDate);
        }

        #endregion
    }
}