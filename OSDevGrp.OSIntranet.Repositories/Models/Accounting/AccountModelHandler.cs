using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Converters.Extensions;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class AccountModelHandler : AccountModelHandlerBase<IAccount, AccountModel>
    {
        #region Private variables

        private readonly bool _includeCreditInformation;
        private readonly CreditInfoModelHandler _creditInfoModelHandler;

        #endregion

        #region Constructor

        public AccountModelHandler(RepositoryContext dbContext, IConverter modelConverter, DateTime statusDate, bool includeCreditInformation, bool includePostingLines) 
            : base(dbContext, modelConverter, statusDate, includePostingLines)
        {
            _includeCreditInformation = includeCreditInformation;
            _creditInfoModelHandler = new CreditInfoModelHandler(dbContext, modelConverter);
        }

        #endregion

        #region Properties

        protected override DbSet<AccountModel> Entities => DbContext.Accounts;

        protected override IQueryable<AccountModel> Reader => CreateReader(_includeCreditInformation, IncludePostingLines);

        protected override IQueryable<AccountModel> UpdateReader => CreateReader(true, false);

        protected override IQueryable<AccountModel> DeleteReader => CreateReader(true, true);

        #endregion

        #region Methods

        protected override void OnDispose()
        {
            _creditInfoModelHandler.Dispose();
        }

        protected override async Task<AccountModel> OnCreateAsync(IAccount account, AccountModel accountModel)
        {
            NullGuard.NotNull(account, nameof(account))
                .NotNull(accountModel, nameof(accountModel));

            accountModel = await base.OnCreateAsync(account, accountModel);
            accountModel.AccountGroup = await DbContext.AccountGroups.SingleAsync(accountGroupModel => accountGroupModel.AccountGroupIdentifier == account.AccountGroup.Number);

            account.CreditInfoCollection.EnsurePopulation(account);

            await _creditInfoModelHandler.CreateOrUpdateAsync(account.CreditInfoCollection, accountModel);

            return accountModel;
        }

        protected override async Task<AccountModel> OnReadAsync(AccountModel accountModel)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel));

            if (accountModel.CreditInfos == null)
            {
                return await base.OnReadAsync(accountModel);
            }

            accountModel.CreditInfos = (await _creditInfoModelHandler.ReadAsync(accountModel.CreditInfos)).ToList();

            return await base.OnReadAsync(accountModel);
        }

        protected override async Task OnUpdateAsync(IAccount account, AccountModel accountModel)
        {
            NullGuard.NotNull(account, nameof(account))
                .NotNull(accountModel, nameof(accountModel));

            await base.OnUpdateAsync(account, accountModel);

            accountModel.AccountGroupIdentifier = account.AccountGroup.Number;
            accountModel.AccountGroup = await DbContext.AccountGroups.SingleAsync(accountGroupModel => accountGroupModel.AccountGroupIdentifier == account.AccountGroup.Number);

            account.CreditInfoCollection.EnsurePopulation(account);

            await _creditInfoModelHandler.CreateOrUpdateAsync(account.CreditInfoCollection, accountModel);
        }

        protected override async Task<bool> CanDeleteAsync(AccountModel accountModel)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel));

            // TODO: Validate the existence of posting lines.

            if (accountModel.CreditInfos == null)
            {
                return false;
            }

            return await _creditInfoModelHandler.IsDeletable(accountModel.CreditInfos);
        }

        protected override async Task<AccountModel> OnDeleteAsync(AccountModel accountModel)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel));

            // TODO: Delete all posting lines.
            await _creditInfoModelHandler.DeleteAsync(accountModel.CreditInfos);

            return await base.OnDeleteAsync(accountModel);
        }

        private IQueryable<AccountModel> CreateReader(bool includeCreditInformation, bool includePostingLines)
        {
            IQueryable<AccountModel> reader = Entities
                .Include(accountModel => accountModel.Accounting).ThenInclude(accountingModel => accountingModel.LetterHead)
                .Include(accountModel => accountModel.BasicAccount)
                .Include(accountModel => accountModel.AccountGroup);

            if (includeCreditInformation)
            {
                reader = reader.Include(accountModel => accountModel.CreditInfos).ThenInclude(creditInfoModel => creditInfoModel.Account).ThenInclude(accountModel => accountModel.Accounting).ThenInclude(accountingModel => accountingModel.LetterHead)
                    .Include(accountModel => accountModel.CreditInfos).ThenInclude(creditInfoModel => creditInfoModel.Account).ThenInclude(accountModel => accountModel.BasicAccount)
                    .Include(accountModel => accountModel.CreditInfos).ThenInclude(creditInfoModel => creditInfoModel.Account).ThenInclude(accountModel => accountModel.AccountGroup)
                    .Include(accountModel => accountModel.CreditInfos).ThenInclude(creditInfoModel => creditInfoModel.YearMonth);
            }

            if (includePostingLines)
            {
                // TODO: Include posting lines.
            }

            return reader;
        }

        #endregion
    }
}