﻿using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Providers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    internal class MakeMonthlyAccountingStatementQueryHandler : MakeMarkdownForAccountingQueryHandlerBase<IMakeMonthlyAccountingStatementQuery, IMonthlyAccountingStatementToMarkdownConverter>
    {
        #region Constructor

        public MakeMonthlyAccountingStatementQueryHandler(IValidator validator, IAccountingRepository accountingRepository, IStatusDateSetter statusDateSetter, IMonthlyAccountingStatementToMarkdownConverter monthlyAccountingStatementToMarkdownConverter) 
            : base(validator, accountingRepository, statusDateSetter, monthlyAccountingStatementToMarkdownConverter, DefaultUtf8EncodingSettingsProvider.ShouldEmitUtf8Identifier)
        {
        }

        #endregion
    }
}