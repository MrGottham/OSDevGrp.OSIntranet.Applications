using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    internal abstract class MakeMarkdownForAccountingQueryHandlerBase<TExportFromAccountingQuery, TAccountingToMarkdownConverter> : MakeMarkdownForDomainObjectFromAccountingQueryHandlerBase<TExportFromAccountingQuery, IAccounting, TAccountingToMarkdownConverter> where TExportFromAccountingQuery : IExportFromAccountingQuery where TAccountingToMarkdownConverter : IAccountingToMarkdownConverter
    {
        #region Constructor

        protected MakeMarkdownForAccountingQueryHandlerBase(IValidator validator, IAccountingRepository accountingRepository, IStatusDateSetter statusDateSetter, TAccountingToMarkdownConverter accountingToMarkdownConverter, bool encoderShouldEmitUtf8Identifier = true) 
            : base(validator, accountingRepository, statusDateSetter, accountingToMarkdownConverter, encoderShouldEmitUtf8Identifier)
        {
        }

        #endregion

        #region Methods

        protected override async Task<IAccounting> GetExportDataAsync(TExportFromAccountingQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            DateTime statusDate = query.StatusDate.Date;

            IAccounting accounting = await AccountingRepository.GetAccountingAsync(query.AccountingNumber, statusDate);
            if (accounting == null)
            {
                return null;
            }

            return await accounting.CalculateAsync(statusDate);
        }

        #endregion
    }
}