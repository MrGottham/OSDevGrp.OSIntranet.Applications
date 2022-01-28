using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Events;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class AccountingModelHandler : ModelHandlerBase<IAccounting, RepositoryContext, AccountingModel, int, AccountingIdentificationState>, IEventHandler<AccountModelCollectionLoadedEvent>, IEventHandler<BudgetAccountModelCollectionLoadedEvent>, IEventHandler<ContactAccountModelCollectionLoadedEvent>, IEventHandler<PostingLineModelCollectionLoadedEvent>
    {
        #region Private variables

        private readonly IEventPublisher _eventPublisher;
        private readonly DateTime _statusDate;
        private readonly bool _includeAccounts;
        private readonly bool _includePostingLines;
        private readonly AccountModelHandler _accountModelHandler;
        private readonly BudgetAccountModelHandler _budgetAccountModelHandler;
        private readonly ContactAccountModelHandler _contactAccountModelHandler;
        private readonly PostingLineModelHandler _postingLineModelHandler;
        private readonly object _syncRoot = new object();
        private IReadOnlyCollection<AccountModel> _accountModelCollection;
        private IReadOnlyCollection<BudgetAccountModel> _budgetAccountModelCollection;
        private IReadOnlyCollection<ContactAccountModel> _contactAccountModelCollection;
        private IReadOnlyCollection<PostingLineModel> _postingLineModelCollection;

        #endregion

        #region Constructor

        public AccountingModelHandler(RepositoryContext dbContext, IConverter modelConverter, IEventPublisher eventPublisher, DateTime statusDate, bool includeAccounts, bool includePostingLines) 
            : base(dbContext, modelConverter)
        {
            NullGuard.NotNull(eventPublisher, nameof(eventPublisher));

            _eventPublisher = eventPublisher;
            _statusDate = statusDate.Date;
            _includeAccounts = includeAccounts;
            _includePostingLines = includePostingLines;

            if (_includeAccounts)
            {
                _accountModelHandler = new AccountModelHandler(dbContext, modelConverter, _eventPublisher, _statusDate, true, false);
                _budgetAccountModelHandler = new BudgetAccountModelHandler(dbContext, modelConverter, _eventPublisher, _statusDate, true, false);
                _contactAccountModelHandler = new ContactAccountModelHandler(dbContext, modelConverter, _eventPublisher, _statusDate, false);
            }

            if (_includePostingLines)
            {
                _postingLineModelHandler = new PostingLineModelHandler(dbContext, modelConverter, _eventPublisher, DateTime.MinValue, _statusDate, false, false);
            }

            _eventPublisher.AddSubscriber(this);
        }

        #endregion

        #region Properties

        protected override DbSet<AccountingModel> Entities => DbContext.Accountings;

        protected override Func<IAccounting, int> PrimaryKey => accounting => accounting.Number;

        protected override IQueryable<AccountingModel> Reader => CreateReader(_includeAccounts);

        protected override IQueryable<AccountingModel> UpdateReader => CreateReader(false);

        protected override IQueryable<AccountingModel> DeleteReader => CreateReader(true);

        #endregion

        #region Methods

        public Task HandleAsync(AccountModelCollectionLoadedEvent accountModelCollectionLoadedEvent)
        {
            NullGuard.NotNull(accountModelCollectionLoadedEvent, nameof(accountModelCollectionLoadedEvent));

            if (accountModelCollectionLoadedEvent.FromSameDbContext(DbContext) == false)
            {
                return Task.CompletedTask;
            }

            lock (_syncRoot)
            {
                if (_accountModelCollection != null)
                {
                    return Task.CompletedTask;
                }

                if (accountModelCollectionLoadedEvent.StatusDate != _statusDate || _includeAccounts == false)
                {
                    return Task.CompletedTask;
                }

                _accountModelCollection = accountModelCollectionLoadedEvent.ModelCollection;

                return Task.CompletedTask;
            }
        }

        public Task HandleAsync(BudgetAccountModelCollectionLoadedEvent budgetAccountModelCollectionLoadedEvent)
        {
            NullGuard.NotNull(budgetAccountModelCollectionLoadedEvent, nameof(budgetAccountModelCollectionLoadedEvent));

            if (budgetAccountModelCollectionLoadedEvent.FromSameDbContext(DbContext) == false)
            {
                return Task.CompletedTask;
            }

            lock (_syncRoot)
            {
                if (_budgetAccountModelCollection != null)
                {
                    return Task.CompletedTask;
                }

                if (budgetAccountModelCollectionLoadedEvent.StatusDate != _statusDate || _includeAccounts == false)
                {
                    return Task.CompletedTask;
                }

                _budgetAccountModelCollection = budgetAccountModelCollectionLoadedEvent.ModelCollection;

                return Task.CompletedTask;
            }
        }

        public Task HandleAsync(ContactAccountModelCollectionLoadedEvent contactAccountModelCollectionLoadedEvent)
        {
            NullGuard.NotNull(contactAccountModelCollectionLoadedEvent, nameof(contactAccountModelCollectionLoadedEvent));

            if (contactAccountModelCollectionLoadedEvent.FromSameDbContext(DbContext) == false)
            {
                return Task.CompletedTask;
            }

            lock (_syncRoot)
            {
                if (_contactAccountModelCollection != null)
                {
                    return Task.CompletedTask;
                }

                if (contactAccountModelCollectionLoadedEvent.StatusDate != _statusDate || _includeAccounts == false)
                {
                    return Task.CompletedTask;
                }

                _contactAccountModelCollection = contactAccountModelCollectionLoadedEvent.ModelCollection;

                return Task.CompletedTask;
            }
        }

        public Task HandleAsync(PostingLineModelCollectionLoadedEvent postingLineModelCollectionLoadedEvent)
        {
            NullGuard.NotNull(postingLineModelCollectionLoadedEvent, nameof(postingLineModelCollectionLoadedEvent));

            if (postingLineModelCollectionLoadedEvent.FromSameDbContext(DbContext) == false)
            {
                return Task.CompletedTask;
            }

            lock (_syncRoot)
            {
                if (_postingLineModelCollection != null && postingLineModelCollectionLoadedEvent.ContainsMorePostingLines(_postingLineModelCollection) == false)
                {
                    return Task.CompletedTask;
                }

                if (postingLineModelCollectionLoadedEvent.ToDate != _statusDate || _includePostingLines == false)
                {
                    return Task.CompletedTask;
                }

                _postingLineModelCollection = postingLineModelCollectionLoadedEvent.ModelCollection;

                return Task.CompletedTask;
            }
        }

        internal async Task<bool> ExistsAsync(int number)
        {
            return await ReadAsync(number, new AccountingIdentificationState(number)) != null;
        }

        internal async Task<AccountingModel> ForAsync(int number)
        {
            await PrepareReadAsync(new AccountingIdentificationState(number));

            AccountingModel accountingModel = await CreateReader(false).SingleOrDefaultAsync(m => m.AccountingIdentifier == number);
            if (accountingModel == null)
            {
                return null;
            }

            return await OnReadAsync(accountingModel);
        }

        protected override Expression<Func<AccountingModel, bool>> EntitySelector(int primaryKey) => accountingModel => accountingModel.AccountingIdentifier == primaryKey;

        protected override Task<IEnumerable<IAccounting>> SortAsync(IEnumerable<IAccounting> accountingCollection)
        {
            NullGuard.NotNull(accountingCollection, nameof(accountingCollection));

            return Task.FromResult(accountingCollection.OrderBy(accounting => accounting.Number).AsEnumerable());
        }

        protected override void OnDispose()
        {
            lock (_syncRoot)
            {
                _eventPublisher.RemoveSubscriber(this);
            }

            _accountModelHandler?.Dispose();
            _budgetAccountModelHandler?.Dispose();
            _contactAccountModelHandler?.Dispose();
            _postingLineModelHandler?.Dispose();
        }

        protected override async Task<AccountingModel> OnCreateAsync(IAccounting accounting, AccountingModel accountingModel)
        {
            NullGuard.NotNull(accounting, nameof(accounting))
                .NotNull(accountingModel, nameof(accountingModel));

            accountingModel.LetterHead = await DbContext.LetterHeads.SingleAsync(letterHeadModel => letterHeadModel.LetterHeadIdentifier == accountingModel.LetterHeadIdentifier);

            return accountingModel;
        }

        protected override async Task PrepareReadAsync(AccountingIdentificationState accountingIdentificationState)
        {
            NullGuard.NotNull(accountingIdentificationState, nameof(accountingIdentificationState));

            if (_includeAccounts)
            {
                _accountModelCollection ??= await _accountModelHandler.ForAsync(accountingIdentificationState.AccountingIdentifier);
                _budgetAccountModelCollection ??= await _budgetAccountModelHandler.ForAsync(accountingIdentificationState.AccountingIdentifier);
                _contactAccountModelCollection ??= await _contactAccountModelHandler.ForAsync(accountingIdentificationState.AccountingIdentifier);
            }

            if (_includePostingLines == false)
            {
                return;
            }

            _postingLineModelCollection ??= await _postingLineModelHandler.ForAsync(accountingIdentificationState.AccountingIdentifier);
        }

        protected override Task PrepareReadAsync(int primaryKey, AccountingIdentificationState accountingIdentificationState)
        {
            NullGuard.NotNull(accountingIdentificationState, nameof(accountingIdentificationState));

            return PrepareReadAsync(new AccountingIdentificationState(primaryKey));
        }

        protected override async Task<AccountingModel> OnReadAsync(AccountingModel accountingModel)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel));

            accountingModel.Accounts = await OnReadAsync(accountingModel, _accountModelCollection, _accountModelHandler);
            accountingModel.BudgetAccounts = await OnReadAsync(accountingModel, _budgetAccountModelCollection, _budgetAccountModelHandler);
            accountingModel.ContactAccounts = await OnReadAsync(accountingModel, _contactAccountModelCollection, _contactAccountModelHandler);
            accountingModel.PostingLines = await OnReadAsync(accountingModel, _postingLineModelCollection, _accountModelHandler, _budgetAccountModelHandler, _contactAccountModelHandler, _postingLineModelHandler);
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

            bool usedOnAccounts = await InUseAsync(accountingModel.Accounts, () => DbContext.Accounts.FirstOrDefaultAsync(accountModel => accountModel.AccountingIdentifier == accountingModel.AccountingIdentifier));
            bool usedOnBudgetAccounts = await InUseAsync(accountingModel.BudgetAccounts, () => DbContext.BudgetAccounts.FirstOrDefaultAsync(budgetAccountModel => budgetAccountModel.AccountingIdentifier == accountingModel.AccountingIdentifier));
            bool usedOnContactAccounts = await InUseAsync(accountingModel.ContactAccounts, () => DbContext.ContactAccounts.FirstOrDefaultAsync(contactAccountModel => contactAccountModel.AccountingIdentifier == accountingModel.AccountingIdentifier));
            bool usedOnPostingLines = await InUseAsync(accountingModel.PostingLines, () => DbContext.PostingLines.FirstOrDefaultAsync(postingLineModel => postingLineModel.AccountingIdentifier == accountingModel.AccountingIdentifier));

            return usedOnAccounts == false && usedOnBudgetAccounts == false && usedOnContactAccounts == false && usedOnPostingLines == false;
        }

        protected override async Task<AccountingModel> OnDeleteAsync(AccountingModel accountingModel)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel));

            await _postingLineModelHandler.DeleteAsync(accountingModel.PostingLines);
            await _accountModelHandler.DeleteAsync(accountingModel.Accounts);
            await _budgetAccountModelHandler.DeleteAsync(accountingModel.BudgetAccounts);
            await _contactAccountModelHandler.DeleteAsync(accountingModel.ContactAccounts);

            return accountingModel;
        }

        private IQueryable<AccountingModel> CreateReader(bool includeAccounts)
        {
            IQueryable<AccountingModel> reader = Entities.Include(accountingModel => accountingModel.LetterHead);

            if (includeAccounts == false || _accountModelCollection != null && _budgetAccountModelCollection != null && _contactAccountModelCollection != null)
            {
                return reader;
            }

            return reader.Include(accountingModel => accountingModel.Accounts).ThenInclude(accountModel => accountModel.BasicAccount)
                .Include(accountingModel => accountingModel.Accounts).ThenInclude(accountModel => accountModel.AccountGroup)
                .Include(accountingModel => accountingModel.BudgetAccounts).ThenInclude(budgetAccountModel => budgetAccountModel.BasicAccount)
                .Include(accountingModel => accountingModel.BudgetAccounts).ThenInclude(budgetAccountModel => budgetAccountModel.BudgetAccountGroup)
                .Include(accountingModel => accountingModel.ContactAccounts).ThenInclude(contactAccounts => contactAccounts.BasicAccount)
                .Include(accountingModel => accountingModel.ContactAccounts).ThenInclude(contactAccounts => contactAccounts.PaymentTerm);
        }

        private static async Task<List<AccountModel>> OnReadAsync(AccountingModel accountingModel, IReadOnlyCollection<AccountModel> accountModelCollection, AccountModelHandler accountModelHandler)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel));

            if (accountModelCollection == null || accountModelHandler == null)
            {
                return accountingModel.Accounts;
            }

            if (accountingModel.Accounts == null)
            {
                accountingModel.ExtractAccounts(accountModelCollection);
            }

            return (await accountModelHandler.ReadAsync(accountingModel.Accounts)).ToList();
        }

        private static async Task<List<BudgetAccountModel>> OnReadAsync(AccountingModel accountingModel, IReadOnlyCollection<BudgetAccountModel> budgetAccountModelCollection, BudgetAccountModelHandler budgetAccountModelHandler)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel));

            if (budgetAccountModelCollection == null || budgetAccountModelHandler == null)
            {
                return accountingModel.BudgetAccounts;
            }

            if (accountingModel.BudgetAccounts == null)
            {
                accountingModel.ExtractBudgetAccounts(budgetAccountModelCollection);
            }

            return (await budgetAccountModelHandler.ReadAsync(accountingModel.BudgetAccounts)).ToList();
        }

        private static async Task<List<ContactAccountModel>> OnReadAsync(AccountingModel accountingModel, IReadOnlyCollection<ContactAccountModel> contactAccountModelCollection, ContactAccountModelHandler contactAccountModelHandler)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel));

            if (contactAccountModelCollection == null || contactAccountModelHandler == null)
            {
                return accountingModel.ContactAccounts;
            }

            if (accountingModel.ContactAccounts == null)
            {
                accountingModel.ExtractContactAccounts(contactAccountModelCollection);
            }

            return (await contactAccountModelHandler.ReadAsync(accountingModel.ContactAccounts)).ToList();
        }

        private static async Task<List<PostingLineModel>> OnReadAsync(AccountingModel accountingModel, IReadOnlyCollection<PostingLineModel> postingLineModelCollection, AccountModelHandler accountModelHandler, BudgetAccountModelHandler budgetAccountModelHandler, ContactAccountModelHandler contactAccountModelHandler, PostingLineModelHandler postingLineModelHandler)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel));

            if (postingLineModelCollection == null || postingLineModelHandler == null)
            {
                return accountingModel.PostingLines;
            }

            foreach (AccountModel accountModel in accountingModel.Accounts ?? new List<AccountModel>(0))
            {
                if (accountModel.PostingLines == null)
                {
                    accountModel.ExtractPostingLines(postingLineModelCollection);
                }

                accountModel.PostingLines = (await postingLineModelHandler.ReadAsync(accountModel.PostingLines)).ToList();
                if (accountModel.PostingLines.Any() == false && accountModelHandler != null)
                {
                    accountModel.Deletable = await accountModelHandler.IsDeletableAsync(accountModel);
                }
            }

            foreach (BudgetAccountModel budgetAccountModel in accountingModel.BudgetAccounts ?? new List<BudgetAccountModel>(0))
            {
                if (budgetAccountModel.PostingLines == null)
                {
                    budgetAccountModel.ExtractPostingLines(postingLineModelCollection);
                }

                budgetAccountModel.PostingLines = (await postingLineModelHandler.ReadAsync(budgetAccountModel.PostingLines)).ToList();
                if (budgetAccountModel.PostingLines.Any() == false && budgetAccountModelHandler != null)
                {
                    budgetAccountModel.Deletable = await budgetAccountModelHandler.IsDeletableAsync(budgetAccountModel);
                }
            }

            foreach (ContactAccountModel contactAccountModel in accountingModel.ContactAccounts ?? new List<ContactAccountModel>(0))
            {
                if (contactAccountModel.PostingLines == null)
                {
                    contactAccountModel.ExtractPostingLines(postingLineModelCollection);
                }

                contactAccountModel.PostingLines = (await postingLineModelHandler.ReadAsync(contactAccountModel.PostingLines)).ToList();
                if (contactAccountModel.PostingLines.Any() == false && contactAccountModelHandler != null)
                {
                    contactAccountModel.Deletable = await contactAccountModelHandler.IsDeletableAsync(contactAccountModel);
                }
            }

            if (accountingModel.PostingLines == null)
            {
                accountingModel.ExtractPostingLines(postingLineModelCollection);
            }

            return (await postingLineModelHandler.ReadAsync(accountingModel.PostingLines)).ToList();
        }

        private static async Task<bool> InUseAsync<TAccountModel>(IEnumerable<TAccountModel> accountModelCollection, Func<Task<TAccountModel>> accountModelGetter) where TAccountModel : AccountModelBase
        {
            NullGuard.NotNull(accountModelGetter, nameof(accountModelGetter));

            if (accountModelCollection != null)
            {
                return accountModelCollection.Any();
            }

            return await accountModelGetter() != null;
        }

        private static async Task<bool> InUseAsync(IEnumerable<PostingLineModel> postingLineModelCollection, Func<Task<PostingLineModel>> postingLineModelGetter)
        {
            NullGuard.NotNull(postingLineModelGetter, nameof(postingLineModelGetter));

            if (postingLineModelCollection != null)
            {
                return postingLineModelCollection.Any();
            }

            return await postingLineModelGetter() != null;
        }

        #endregion
    }
}