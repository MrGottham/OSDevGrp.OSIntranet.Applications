using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Providers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    internal class MakeContactAccountStatementQueryHandler : MakeMarkdownForContactAccountQueryHandlerBase<IMakeContactAccountStatementQuery, IContactAccountStatementToMarkdownConverter>
    {
        #region Constructor

        public MakeContactAccountStatementQueryHandler(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository, IStatusDateSetter statusDateSetter, IContactAccountStatementToMarkdownConverter contactAccountStatementToMarkdownConverter) 
            : base(validator, claimResolver, accountingRepository, statusDateSetter, contactAccountStatementToMarkdownConverter, DefaultUtf8EncodingSettingsProvider.ShouldEmitUtf8Identifier)
        {
        }

        #endregion
    }
}