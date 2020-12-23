using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class AccountingModelHandler : ModelHandlerBase<IAccounting, RepositoryContext, AccountingModel, int>
    {
        #region Private variables

        private readonly bool _includeAccounts;
        private readonly bool _includePostingLines;
        private readonly AccountModelHandler _accountModelHandler;
        private readonly CreditInfoModelHandler _creditInfoModelHandler;
        private readonly BudgetAccountModelHandler _budgetAccountModelHandler;
        private readonly BudgetInfoModelHandler _budgetInfoModelHandler;
        private readonly ContactAccountModelHandler _contactAccountModelHandler;
        private IReadOnlyCollection<CreditInfoModel> _creditInfoModelCollection;
        private IReadOnlyCollection<BudgetInfoModel> _budgetInfoModelCollection;

        #endregion

        #region Constructor

        public AccountingModelHandler(RepositoryContext dbContext, IConverter modelConverter, DateTime statusDate, bool includeAccounts, bool includePostingLines) 
            : base(dbContext, modelConverter)
        {
            _includeAccounts = includeAccounts;
            _includePostingLines = includePostingLines;
            _accountModelHandler = new AccountModelHandler(dbContext, modelConverter, statusDate, true, includePostingLines);
            _creditInfoModelHandler = new CreditInfoModelHandler(dbContext, modelConverter);
            _budgetAccountModelHandler = new BudgetAccountModelHandler(dbContext, modelConverter, statusDate, true, includePostingLines);
            _budgetInfoModelHandler = new BudgetInfoModelHandler(dbContext, modelConverter);
            _contactAccountModelHandler = new ContactAccountModelHandler(dbContext, modelConverter, statusDate, includePostingLines);
        }

        #endregion

        #region Properties

        protected override DbSet<AccountingModel> Entities => DbContext.Accountings;

        protected override Func<IAccounting, int> PrimaryKey => accounting => accounting.Number;

        protected override IQueryable<AccountingModel> Reader => CreateReader(_includeAccounts, _includePostingLines);

        protected override IQueryable<AccountingModel> UpdateReader => CreateReader(false, false);

        protected override IQueryable<AccountingModel> DeleteReader => CreateReader(true, true);

        #endregion

        #region Methods

        internal async Task<bool> ExistsAsync(int number)
        {
            return await ReadAsync(number) != null;
        }

        protected override Expression<Func<AccountingModel, bool>> EntitySelector(int primaryKey) => accountingModel => accountingModel.AccountingIdentifier == primaryKey;

        protected override Task<IEnumerable<IAccounting>> SortAsync(IEnumerable<IAccounting> accountingCollection)
        {
            NullGuard.NotNull(accountingCollection, nameof(accountingCollection));

            return Task.FromResult(accountingCollection.OrderBy(accounting => accounting.Number).AsEnumerable());
        }

        protected override void OnDispose()
        {
            _accountModelHandler.Dispose();
            _creditInfoModelHandler.Dispose();
            _budgetAccountModelHandler.Dispose();
            _budgetInfoModelHandler.Dispose();
            _contactAccountModelHandler.Dispose();
        }

        protected override async Task<AccountingModel> OnCreateAsync(IAccounting accounting, AccountingModel accountingModel)
        {
            NullGuard.NotNull(accounting, nameof(accounting))
                .NotNull(accountingModel, nameof(accountingModel));

            accountingModel.LetterHead = await DbContext.LetterHeads.SingleAsync(letterHeadModel => letterHeadModel.LetterHeadIdentifier == accountingModel.LetterHeadIdentifier);

            return accountingModel;
        }

        protected override async Task PrepareReadAsync(int primaryKey, object prepareReadStatus = null)
        {
            if (_includeAccounts == false)
            {
                return;
            }

            _creditInfoModelCollection = await _creditInfoModelHandler.ForAsync(primaryKey);
            _budgetInfoModelCollection = await _budgetInfoModelHandler.ForAsync(primaryKey);
        }

        protected override async Task<AccountingModel> OnReadAsync(AccountingModel accountingModel)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel));

            if (accountingModel.Accounts != null)
            {
                accountingModel.Accounts.ForAll(accountModel => accountModel.ExtractCreditInfos(_creditInfoModelCollection));

                accountingModel.Accounts = (await _accountModelHandler.ReadAsync(accountingModel.Accounts)).ToList();
            }

            if (accountingModel.BudgetAccounts != null)
            {
                accountingModel.BudgetAccounts.ForAll(budgetAccountModel => budgetAccountModel.ExtractBudgetInfos(_budgetInfoModelCollection));

                accountingModel.BudgetAccounts = (await _budgetAccountModelHandler.ReadAsync(accountingModel.BudgetAccounts)).ToList();
            }

            if (accountingModel.ContactAccounts != null)
            {
                accountingModel.ContactAccounts = (await _contactAccountModelHandler.ReadAsync(accountingModel.ContactAccounts)).ToList();
            }

            if (_includePostingLines)
            {
                // TODO: Include all posting lines for the given status date.
            }

            accountingModel.Deletable = await CanDeleteAsync(accountingModel);

            return accountingModel;
        }

        protected override async Task OnUpdateAsync(IAccounting accounting, AccountingModel accountingModel)
        {
            NullGuard.NotNull(accounting, nameof(accounting))
                .NotNull(accountingModel, nameof(accountingModel));

            accountingModel.Name = accounting.Name;
            accountingModel.LetterHeadIdentifier = accounting.LetterHead.Number;
            accountingModel.LetterHead = await DbContext.LetterHeads.SingleAsync(letterHeadModel => letterHeadModel.LetterHeadIdentifier == accountingModel.LetterHeadIdentifier);
            accountingModel.BalanceBelowZero = accounting.BalanceBelowZero;
            accountingModel.BackDating = accounting.BackDating;
        }

        protected override async Task<bool> CanDeleteAsync(AccountingModel accountingModel)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel));

            bool usedOnAccount = await CanDeleteAsync(accountingModel.Accounts, DbContext.Accounts.FirstOrDefaultAsync(accountModel => accountModel.AccountingIdentifier == accountingModel.AccountingIdentifier));
            bool usedOnBudgetAccount = await CanDeleteAsync(accountingModel.BudgetAccounts, DbContext.BudgetAccounts.FirstOrDefaultAsync(budgetAccountModel => budgetAccountModel.AccountingIdentifier == accountingModel.AccountingIdentifier));
            bool usedOnContactAccount = await CanDeleteAsync(accountingModel.ContactAccounts, DbContext.ContactAccounts.FirstOrDefaultAsync(contactAccountModel => contactAccountModel.AccountingIdentifier == accountingModel.AccountingIdentifier));

            return usedOnAccount == false && usedOnBudgetAccount == false && usedOnContactAccount == false;
        }

        protected override async Task<AccountingModel> OnDeleteAsync(AccountingModel accountingModel)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel));

            await PrepareReadAsync(accountingModel.AccountingIdentifier);
            accountingModel.Accounts.ForAll(accountModel => accountModel.ExtractCreditInfos(_creditInfoModelCollection));
            accountingModel.BudgetAccounts.ForAll(budgetAccountModel => budgetAccountModel.ExtractBudgetInfos(_budgetInfoModelCollection));

            // TODO: Delete all posting lines.

            await _accountModelHandler.DeleteAsync(accountingModel.Accounts);
            await _budgetAccountModelHandler.DeleteAsync(accountingModel.BudgetAccounts);
            await _contactAccountModelHandler.DeleteAsync(accountingModel.ContactAccounts);

            return accountingModel;
        }

        private IQueryable<AccountingModel> CreateReader(bool includeAccounts, bool includePostingLines)
        {
            IQueryable<AccountingModel> reader = Entities.Include(accountingModel => accountingModel.LetterHead);

            if (includeAccounts)
            {
                reader = reader.Include(accountingModel => accountingModel.Accounts)
                    .Include(accountingModel => accountingModel.Accounts).ThenInclude(accountModel => accountModel.BasicAccount)
                    .Include(accountingModel => accountingModel.Accounts).ThenInclude(accountModel => accountModel.AccountGroup);

                reader = reader.Include(accountingModel => accountingModel.BudgetAccounts)
                    .Include(accountingModel => accountingModel.BudgetAccounts).ThenInclude(budgetAccountModel => budgetAccountModel.BasicAccount)
                    .Include(accountingModel => accountingModel.BudgetAccounts).ThenInclude(budgetAccountModel => budgetAccountModel.BudgetAccountGroup);

                reader = reader.Include(accountingModel => accountingModel.ContactAccounts)
                    .Include(accountingModel => accountingModel.ContactAccounts).ThenInclude(contactAccounts => contactAccounts.BasicAccount)
                    .Include(accountingModel => accountingModel.ContactAccounts).ThenInclude(contactAccounts => contactAccounts.PaymentTerm);
            }

            if (includePostingLines)
            {
                // TODO: Include posting lines.
            }

            return reader;
        }

        private static async Task<bool> CanDeleteAsync<TAccountModel>(IList<TAccountModel> accountModelCollection, Task<TAccountModel> accountModelGetter) where TAccountModel : AccountModelBase
        {
            NullGuard.NotNull(accountModelGetter, nameof(accountModelGetter));

            if (accountModelCollection != null)
            {
                return accountModelCollection.Count > 0;
            }

            return await accountModelGetter != null;
        }

        #endregion
    }
}