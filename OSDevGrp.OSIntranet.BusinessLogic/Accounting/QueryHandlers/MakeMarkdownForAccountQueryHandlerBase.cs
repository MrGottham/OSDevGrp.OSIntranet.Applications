using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    internal abstract class MakeMarkdownForAccountQueryHandlerBase<TExportFromAccountQuery, TAccountToMarkdownConverter> : MakeMarkdownForAccountBaseQueryHandlerBase<TExportFromAccountQuery, IAccount, TAccountToMarkdownConverter> where TExportFromAccountQuery : IExportFromAccountQuery where TAccountToMarkdownConverter : IAccountToMarkdownConverter<IAccount>
    {
        #region Constructor

        protected MakeMarkdownForAccountQueryHandlerBase(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository, IStatusDateSetter statusDateSetter, TAccountToMarkdownConverter accountToMarkdownConverter, bool encoderShouldEmitUtf8Identifier = true) 
            : base(validator, claimResolver, accountingRepository, statusDateSetter, accountToMarkdownConverter, encoderShouldEmitUtf8Identifier)
        {
        }

        #endregion

        #region Methods

        protected override async Task<IAccount> GetExportDataAsync(TExportFromAccountQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            DateTime statusDate = query.StatusDate.Date;

            IAccount account = await AccountingRepository.GetAccountAsync(query.AccountingNumber, query.AccountNumber, statusDate);
            if (account == null)
            {
                return null;
            }

            return await account.CalculateAsync(statusDate);
        }

        #endregion
    }
}