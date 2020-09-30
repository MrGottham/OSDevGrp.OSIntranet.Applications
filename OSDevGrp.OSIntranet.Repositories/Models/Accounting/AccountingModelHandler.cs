using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
        private readonly BudgetAccountModelHandler _budgetAccountModelHandler;
        private readonly ContactAccountModelHandler _contactAccountModelHandler;

        #endregion

        #region Constructor

        public AccountingModelHandler(RepositoryContext dbContext, IConverter modelConverter, DateTime statusDate, bool includeAccounts, bool includePostingLines) 
            : base(dbContext, modelConverter)
        {
            _includeAccounts = includeAccounts;
            _includePostingLines = includePostingLines;
            _accountModelHandler = new AccountModelHandler(dbContext, modelConverter, statusDate, true, includePostingLines);
            _budgetAccountModelHandler = new BudgetAccountModelHandler(dbContext, modelConverter, statusDate, true, includePostingLines);
            _contactAccountModelHandler = new ContactAccountModelHandler(dbContext, modelConverter, statusDate, includePostingLines);
        }

        #endregion

        #region Properties

        protected override DbSet<AccountingModel> Entities => DbContext.Accountings;

        protected override Func<IAccounting, int> PrimaryKey => accounting => accounting.Number;

        protected override IQueryable<AccountingModel> Reader => Entities
            .Include(accountingModel => accountingModel.LetterHead);

        protected override IQueryable<AccountingModel> DeleteReader => Reader
            .Include(accountingModel => accountingModel.Accounts)
            .Include(accountingModel => accountingModel.BudgetAccounts)
            .Include(accountingModel => accountingModel.ContactAccounts); // TODO: Include posting lines.

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
            _budgetAccountModelHandler.Dispose();
            _contactAccountModelHandler.Dispose();
        }

        protected override async Task<AccountingModel> OnCreateAsync(IAccounting accounting, AccountingModel accountingModel)
        {
            NullGuard.NotNull(accounting, nameof(accounting))
                .NotNull(accountingModel, nameof(accountingModel));

            accountingModel.LetterHead = await DbContext.LetterHeads.SingleAsync(letterHeadModel => letterHeadModel.LetterHeadIdentifier == accountingModel.LetterHeadIdentifier);

            return accountingModel;
        }

        protected override async Task<AccountingModel> OnReadAsync(AccountingModel accountingModel)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel));

            accountingModel.Deletable = await CanDeleteAsync(accountingModel);

            if (_includeAccounts)
            {
                accountingModel.Accounts = (await _accountModelHandler.ReadAsync(accountingModel)).ToList();
                accountingModel.BudgetAccounts = (await _budgetAccountModelHandler.ReadAsync(accountingModel)).ToList();
                accountingModel.ContactAccounts = (await _contactAccountModelHandler.ReadAsync(accountingModel)).ToList();
            }

            if (_includePostingLines)
            {
                // TODO: Include all posting lines for the given status date.
            }

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

            bool usedOnAccount = await DbContext.Accounts.FirstOrDefaultAsync(accountModel => accountModel.AccountingIdentifier == accountingModel.AccountingIdentifier) != null;
            bool usedOnBudgetAccount = await DbContext.BudgetAccounts.FirstOrDefaultAsync(budgetAccountModel => budgetAccountModel.AccountingIdentifier == accountingModel.AccountingIdentifier) != null;
            bool usedOnContactAccount = await DbContext.ContactAccounts.FirstOrDefaultAsync(contactAccountModel => contactAccountModel.AccountingIdentifier == accountingModel.AccountingIdentifier) != null;

            return usedOnAccount == false && usedOnBudgetAccount == false && usedOnContactAccount == false;
        }

        protected override async Task<AccountingModel> OnDeleteAsync(AccountingModel accountingModel)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel));

            await _accountModelHandler.DeleteAsync(accountingModel.Accounts);
            await _budgetAccountModelHandler.DeleteAsync(accountingModel.BudgetAccounts);
            await _contactAccountModelHandler.DeleteAsync(accountingModel.ContactAccounts);

            // TODO: Delete all posting lines.

            return accountingModel;
        }

        #endregion
    }
}