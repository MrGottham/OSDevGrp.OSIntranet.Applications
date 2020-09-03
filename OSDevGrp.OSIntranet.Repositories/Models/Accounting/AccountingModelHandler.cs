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
    internal class AccountingModelHandler : ModelHandlerBase<IAccounting, AccountingContext, AccountingModel, int>
    {
        #region Private variables

        private readonly DateTime _statusDate;
        private readonly bool _includeAccounts;
        private readonly bool _includePostingLines;

        #endregion

        #region Constructor

        public AccountingModelHandler(AccountingContext dbContext, IConverter modelConverter, DateTime statusDate, bool includeAccounts, bool includePostingLines) 
            : base(dbContext, modelConverter)
        {
            _statusDate = statusDate;
            _includeAccounts = includeAccounts;
            _includePostingLines = includePostingLines;
        }

        #endregion

        #region Properties

        protected override DbSet<AccountingModel> Entities => DbContext.Accountings;

        protected override Func<IAccounting, int> PrimaryKey => accounting => accounting.Number;

        protected override IQueryable<AccountingModel> Reader => Entities
            .Include(accountingModel => accountingModel.LetterHead);

        protected override IQueryable<AccountingModel> UpdateReader => Reader; // TODO: Include all account types and posting lines.

        protected override IQueryable<AccountingModel> DeleteReader => Reader; // TODO: Include all account types and posting lines.

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
                // TODO: Include all account types for the given status date.
            }

            if (_includePostingLines)
            {
                // TODO: Include all accounting lines for the given status date.
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

            // TODO: Update all account types.
            // TODO: Update all accounting lines.
        }

        protected override Task<bool> CanDeleteAsync(AccountingModel accountingModel)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel));

            return Task.FromResult(false);
        }

        protected override Task<AccountingModel> OnDeleteAsync(AccountingModel accountingModel)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel));

            // TODO: Delete all account types.
            // TODO: Delete all accounting lines.

            return Task.FromResult(accountingModel);
        }

        #endregion
    }
}