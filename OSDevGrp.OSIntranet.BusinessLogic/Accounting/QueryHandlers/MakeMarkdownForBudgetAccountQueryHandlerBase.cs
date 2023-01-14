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
    internal abstract class MakeMarkdownForBudgetAccountQueryHandlerBase<TExportFromAccountQuery, TBudgetAccountToMarkdownConverter> : MakeMarkdownForAccountBaseQueryHandlerBase<TExportFromAccountQuery, IBudgetAccount, TBudgetAccountToMarkdownConverter> where TExportFromAccountQuery : IExportFromAccountQuery where TBudgetAccountToMarkdownConverter : IAccountToMarkdownConverter<IBudgetAccount>
    {
        #region Constructor

        protected MakeMarkdownForBudgetAccountQueryHandlerBase(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository, IStatusDateSetter statusDateSetter, TBudgetAccountToMarkdownConverter budgetAccountToMarkdownConverter, bool encoderShouldEmitUtf8Identifier = true) :
            base(validator, claimResolver, accountingRepository, statusDateSetter, budgetAccountToMarkdownConverter, encoderShouldEmitUtf8Identifier)
        {
        }

        #endregion

        #region Methods

        protected override async Task<IBudgetAccount> GetExportDataAsync(TExportFromAccountQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            DateTime statusDate = query.StatusDate.Date;

            IBudgetAccount budgetAccount = await AccountingRepository.GetBudgetAccountAsync(query.AccountingNumber, query.AccountNumber, statusDate);
            if (budgetAccount == null)
            {
                return null;
            }

            return await budgetAccount.CalculateAsync(statusDate);
        }

        #endregion
    }
}