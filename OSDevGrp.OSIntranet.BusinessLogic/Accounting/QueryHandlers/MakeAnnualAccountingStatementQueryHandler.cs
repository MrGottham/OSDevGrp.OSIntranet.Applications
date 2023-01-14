using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Providers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    internal class MakeAnnualAccountingStatementQueryHandler : MakeMarkdownForAccountingQueryHandlerBase<IMakeAnnualAccountingStatementQuery, IAnnualAccountingStatementToMarkdownConverter>
    {
        #region Constructor

        public MakeAnnualAccountingStatementQueryHandler(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository, IStatusDateSetter statusDateSetter, IAnnualAccountingStatementToMarkdownConverter annualAccountingStatementToMarkdownConverter) 
            : base(validator, claimResolver, accountingRepository, statusDateSetter, annualAccountingStatementToMarkdownConverter, DefaultUtf8EncodingSettingsProvider.ShouldEmitUtf8Identifier)
        {
        }

        #endregion
    }
}