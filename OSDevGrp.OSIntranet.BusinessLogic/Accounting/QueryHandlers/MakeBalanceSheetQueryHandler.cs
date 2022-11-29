﻿using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Providers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    internal class MakeBalanceSheetQueryHandler : MakeMarkdownForAccountingQueryHandlerBase<IMakeBalanceSheetQuery, IBalanceSheetToMarkdownConverter>
    {
        #region Constructor

        public MakeBalanceSheetQueryHandler(IValidator validator, IAccountingRepository accountingRepository, IStatusDateSetter statusDateSetter, IBalanceSheetToMarkdownConverter accountingToMarkdownConverter) 
            : base(validator, accountingRepository, statusDateSetter, accountingToMarkdownConverter, DefaultUtf8EncodingSettingsProvider.ShouldEmitUtf8Identifier)
        {
        }

        #endregion
    }
}