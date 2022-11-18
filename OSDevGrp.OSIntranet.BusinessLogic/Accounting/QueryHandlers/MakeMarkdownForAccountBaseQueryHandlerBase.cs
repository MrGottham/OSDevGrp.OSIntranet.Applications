using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    internal abstract class MakeMarkdownForAccountBaseQueryHandlerBase<TExportFromAccountQuery, TAccount, TAccountToMarkdownConverter> : MakeMarkdownForDomainObjectFromAccountingQueryHandlerBase<TExportFromAccountQuery, TAccount, TAccountToMarkdownConverter> where TExportFromAccountQuery : IExportFromAccountQuery where TAccount : class, IAccountBase where TAccountToMarkdownConverter : IAccountToMarkdownConverter<TAccount>
    {
        #region Constructor

        protected MakeMarkdownForAccountBaseQueryHandlerBase(IValidator validator, IAccountingRepository accountingRepository, IStatusDateSetter statusDateSetter, TAccountToMarkdownConverter accountToMarkdownConverter, bool encoderShouldEmitUtf8Identifier = true) :
            base(validator, accountingRepository, statusDateSetter, accountToMarkdownConverter, encoderShouldEmitUtf8Identifier)
        {
        }

        #endregion
    }
}