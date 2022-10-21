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
    internal class ExportBudgetAccountGroupStatusCollectionQueryHandler : ExportDomainCollectionFromAccountingToCsvQueryHandlerBase<IExportBudgetAccountGroupStatusCollectionQuery, IBudgetAccountGroupStatus, IBudgetAccountGroupStatusToCsvConverter>
    {
        #region Constructor

        public ExportBudgetAccountGroupStatusCollectionQueryHandler(IValidator validator, IAccountingRepository accountingRepository, IStatusDateSetter statusDateSetter, IBudgetAccountGroupStatusToCsvConverter domainObjectToCsvConverter) 
            : base(validator, accountingRepository, statusDateSetter, domainObjectToCsvConverter, DefaultUtf8EncodingSettingsProvider.ShouldEmitUtf8Identifier)
        {
        }

        #endregion

        #region Methods

        protected override async Task<IEnumerable<IBudgetAccountGroupStatus>> GetExportDataAsync(IExportBudgetAccountGroupStatusCollectionQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            DateTime statusDate = query.StatusDate;

            IBudgetAccountCollection budgetAccountCollection = await AccountingRepository.GetBudgetAccountsAsync(query.AccountingNumber, statusDate);
            if (budgetAccountCollection == null)
            {
                return Array.Empty<IBudgetAccountGroupStatus>();
            }

            IBudgetAccountCollection calculatedBudgetAccountCollection = await budgetAccountCollection.CalculateAsync(statusDate);
            if (calculatedBudgetAccountCollection == null)
            {
                return Array.Empty<IBudgetAccountGroupStatus>();
            }

            return await calculatedBudgetAccountCollection.GroupByBudgetAccountGroupAsync();
        }

        #endregion
    }
}