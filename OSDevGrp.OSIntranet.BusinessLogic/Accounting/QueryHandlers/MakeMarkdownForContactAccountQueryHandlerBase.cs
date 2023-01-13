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
    internal abstract class MakeMarkdownForContactAccountQueryHandlerBase<TExportFromAccountQuery, TContactAccountToMarkdownConverter> : MakeMarkdownForAccountBaseQueryHandlerBase<TExportFromAccountQuery, IContactAccount, TContactAccountToMarkdownConverter> where TExportFromAccountQuery : IExportFromAccountQuery where TContactAccountToMarkdownConverter : IAccountToMarkdownConverter<IContactAccount>
    {
        #region Constructor

        protected MakeMarkdownForContactAccountQueryHandlerBase(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository, IStatusDateSetter statusDateSetter, TContactAccountToMarkdownConverter contactAccountToMarkdownConverter, bool encoderShouldEmitUtf8Identifier = true)
            : base(validator, claimResolver, accountingRepository, statusDateSetter, contactAccountToMarkdownConverter, encoderShouldEmitUtf8Identifier)
        {
        }

        #endregion

        #region Methods

        protected override async Task<IContactAccount> GetExportDataAsync(TExportFromAccountQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            DateTime statusDate = query.StatusDate.Date;

            IContactAccount contactAccount = await AccountingRepository.GetContactAccountAsync(query.AccountingNumber, query.AccountNumber, statusDate);
            if (contactAccount == null)
            {
                return null;
            }

            return await contactAccount.CalculateAsync(statusDate);
        }

        #endregion
    }
}