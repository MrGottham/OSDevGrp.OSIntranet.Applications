using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Providers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    internal class MakeAnnualAccountingStatementQueryHandler : MakeMarkdownForAccountingQueryHandlerBase<IMakeAnnualAccountingStatementQuery, IAnnualAccountingStatementToMarkdownConverter>
    {
        #region Constructor

        public MakeAnnualAccountingStatementQueryHandler(IValidator validator, IAccountingRepository accountingRepository, IStatusDateSetter statusDateSetter, IAnnualAccountingStatementToMarkdownConverter annualAccountingStatementToMarkdownConverter) 
            : base(validator, accountingRepository, statusDateSetter, annualAccountingStatementToMarkdownConverter, DefaultUtf8EncodingSettingsProvider.ShouldEmitUtf8Identifier)
        {
        }

        #endregion
    }
}