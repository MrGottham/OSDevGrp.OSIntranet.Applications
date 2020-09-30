using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class AccountModelHandler : AccountModelHandlerBase<IAccount, AccountModel>
    {
        #region Private variables

        private readonly bool _includeCreditInformation;

        #endregion

        #region Constructor

        public AccountModelHandler(RepositoryContext dbContext, IConverter modelConverter, DateTime statusDate, bool includeCreditInformation, bool includePostingLines) 
            : base(dbContext, modelConverter, statusDate, includePostingLines)
        {
            _includeCreditInformation = includeCreditInformation;
        }

        #endregion

        #region Properties

        protected override DbSet<AccountModel> Entities => DbContext.Accounts;

        protected override IQueryable<AccountModel> Reader => Entities
            .Include(accountModel => accountModel.Accounting).ThenInclude(accountingModel => accountingModel.LetterHead)
            .Include(accountModel => accountModel.BasicAccount)
            .Include(accountModel => accountModel.AccountGroup);

        protected override IQueryable<AccountModel> UpdateReader => Reader; // TODO: Include credit information.

        protected override IQueryable<AccountModel> DeleteReader => Reader; // TODO: Include credit information and posting lines.

        #endregion

        #region Methods

        protected override async Task<AccountModel> OnCreateAsync(IAccount account, AccountModel accountModel)
        {
            NullGuard.NotNull(account, nameof(account))
                .NotNull(accountModel, nameof(accountModel));

            accountModel = await base.OnCreateAsync(account, accountModel);
            accountModel.AccountGroup = await DbContext.AccountGroups.SingleAsync(accountGroupModel => accountGroupModel.AccountGroupIdentifier == account.AccountGroup.Number);

            // TODO: Create credit information.

            return accountModel;
        }

        protected override async Task<AccountModel> OnReadAsync(AccountModel accountModel)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel));

            accountModel = await base.OnReadAsync(accountModel);

            if (_includeCreditInformation)
            {
                // TODO: Read all credit information for the given status date.
            }

            if (IncludePostingLines)
            {
                // TODO: Include all posting lines for the given status date.
            }

            return accountModel;
        }

        protected override async Task OnUpdateAsync(IAccount account, AccountModel accountModel)
        {
            NullGuard.NotNull(account, nameof(account))
                .NotNull(accountModel, nameof(accountModel));

            await base.OnUpdateAsync(account, accountModel);

            accountModel.AccountGroupIdentifier = account.AccountGroup.Number;
            accountModel.AccountGroup = await DbContext.AccountGroups.SingleAsync(accountGroupModel => accountGroupModel.AccountGroupIdentifier == account.AccountGroup.Number);

            // TODO: Create credit information.
        }

        protected override async Task<AccountModel> OnDeleteAsync(AccountModel accountModel)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel));

            accountModel = await base.OnDeleteAsync(accountModel);

            // TODO: Delete all credit information.
            // TODO: Delete all posting lines.

            return accountModel;
        }

        protected override Task<bool> CanDeleteAsync(AccountModel accountModel)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel));

            // TODO: Validate the existence of credit information.
            // TODO: Validate the existence of posting lines.

            return Task.FromResult(false);
        }

        #endregion
    }
}