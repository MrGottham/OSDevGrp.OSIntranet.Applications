using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Events;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class PostingLineModelHandler : ModelHandlerBase<IPostingLine, RepositoryContext, PostingLineModel, string, AccountingIdentificationState>, IEventHandler<AccountModelCollectionLoadedEvent>, IEventHandler<BudgetAccountModelCollectionLoadedEvent>, IEventHandler<ContactAccountModelCollectionLoadedEvent>
    {
        #region Private variables

        private readonly IEventPublisher _eventPublisher;
        private readonly DateTime _fromDate;
        private readonly DateTime _toDate;
        private readonly bool _includeCreditInformation;
        private readonly bool _includeBudgetInformation;
        private readonly int? _numberOfPostingLines;
        private readonly bool _applyingPostingLines;
        private readonly AccountingModelHandler _accountingModelHandler;
        private readonly AccountModelHandler _accountModelHandler;
        private readonly BudgetAccountModelHandler _budgetAccountModelHandler;
        private readonly ContactAccountModelHandler _contactAccountModelHandler;
        private readonly object _syncRoot = new object();
        private AccountingModel _accountingModel;
        private IReadOnlyCollection<AccountModel> _accountModelCollection;
        private IReadOnlyCollection<BudgetAccountModel> _budgetAccountModelCollection;
        private IReadOnlyCollection<ContactAccountModel> _contactAccountModelCollection;

        #endregion

        #region Constructor

        public PostingLineModelHandler(RepositoryContext dbContext, IConverter modelConverter, IEventPublisher eventPublisher, DateTime fromDate, DateTime toDate, bool includeCreditInformation, bool includeBudgetInformation, int? numberOfPostingLines = null, bool applyingPostingLines = false) 
            : base(dbContext, modelConverter)
        {
            NullGuard.NotNull(eventPublisher, nameof(eventPublisher));

            _eventPublisher = eventPublisher;
            _fromDate = fromDate.Date;
            _toDate = toDate.Date;
            _includeCreditInformation = includeCreditInformation;
            _includeBudgetInformation = includeBudgetInformation;
            _numberOfPostingLines = numberOfPostingLines;
            _applyingPostingLines = applyingPostingLines;
            _accountingModelHandler = new AccountingModelHandler(dbContext, ModelConverter, _eventPublisher, _toDate, false, false);
            _accountModelHandler = new AccountModelHandler(dbContext, modelConverter, _eventPublisher, _toDate, _includeCreditInformation, false);
            _budgetAccountModelHandler = new BudgetAccountModelHandler(dbContext, modelConverter, _eventPublisher, _toDate, _includeBudgetInformation, false);
            _contactAccountModelHandler = new ContactAccountModelHandler(dbContext, modelConverter, _eventPublisher, _toDate, false);

            _eventPublisher.AddSubscriber(this);
        }

        #endregion

        #region Properties

        protected override DbSet<PostingLineModel> Entities => DbContext.PostingLines;

        protected override Func<IPostingLine, string> PrimaryKey => postingLine => postingLine.Identifier.ToString("D").ToUpper();

        protected override IQueryable<PostingLineModel> Reader => CreateReader(_numberOfPostingLines);

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

                if (accountModelCollectionLoadedEvent.StatusDate != _toDate || _includeCreditInformation && accountModelCollectionLoadedEvent.CreditInformationIncluded == false)
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

                if (budgetAccountModelCollectionLoadedEvent.StatusDate != _toDate || _includeBudgetInformation && budgetAccountModelCollectionLoadedEvent.BudgetInformationIncluded == false)
                {
                    return Task.CompletedTask;
                }

                _budgetAccountModelCollection = budgetAccountModelCollectionLoadedEvent.ModelCollection;

                return Task.CompletedTask;
            }
        }

        public Task HandleAsync(ContactAccountModelCollectionLoadedEvent contactAccountModelCollection)
        {
            NullGuard.NotNull(contactAccountModelCollection, nameof(contactAccountModelCollection));

            if (contactAccountModelCollection.FromSameDbContext(DbContext) == false)
            {
                return Task.CompletedTask;
            }

            lock (_syncRoot)
            {
                if (_contactAccountModelCollection != null)
                {
                    return Task.CompletedTask;
                }

                if (contactAccountModelCollection.StatusDate != _toDate)
                {
                    return Task.CompletedTask;
                }

                _contactAccountModelCollection = contactAccountModelCollection.ModelCollection;

                return Task.CompletedTask;
            }
        }

        internal Task<IEnumerable<IPostingLine>> ReadAsync(int accountingNumber)
        {
            DateTime startDate = _fromDate.Date;
            DateTime endDate = _toDate.AddDays(1).Date;

            return ReadAsync(postingLineModel => postingLineModel.AccountingIdentifier == accountingNumber && postingLineModel.PostingDate >= startDate && postingLineModel.PostingDate < endDate, new AccountingIdentificationState(accountingNumber));
        }

        internal async Task<IEnumerable<PostingLineModel>> ReadAsync(IEnumerable<PostingLineModel> postingLineModelCollection)
        {
            NullGuard.NotNull(postingLineModelCollection, nameof(postingLineModelCollection));

            return await Task.WhenAll(postingLineModelCollection.Select(OnReadAsync));
        }

        internal async Task<IReadOnlyCollection<PostingLineModel>> ForAsync(int accountingNumber, bool callPrepareReadAsync = true)
        {
            if (callPrepareReadAsync)
            {
                await PrepareReadAsync(new AccountingIdentificationState(accountingNumber));
            }

            DateTime startDate = _fromDate.Date;
            DateTime endDate = _toDate.AddDays(1).Date;

            IQueryable<PostingLineModel> query = CreateReader(null)
                .Where(postingLineModel => postingLineModel.AccountingIdentifier == accountingNumber &&
                                           postingLineModel.PostingDate >= startDate &&
                                           postingLineModel.PostingDate < endDate);

            IReadOnlyCollection<PostingLineModel> postingLineModelCollection = (await ReadAsync(query)).ToArray();

            lock (_syncRoot)
            {
                _eventPublisher.PublishAsync(new PostingLineModelCollectionLoadedEvent(DbContext, postingLineModelCollection, _fromDate, _toDate))
                    .GetAwaiter()
                    .GetResult();
            }

            return postingLineModelCollection;
        }

        internal Task DeleteAsync(IEnumerable<PostingLineModel> postingLineModelCollection)
        {
            NullGuard.NotNull(postingLineModelCollection, nameof(postingLineModelCollection));

            if (postingLineModelCollection.Any() == false)
            {
                return Task.CompletedTask;
            }

            throw new IntranetExceptionBuilder(ErrorCode.UnableToDeleteOneOrMoreObjects, nameof(PostingLineModel))
                .WithMethodBase(MethodBase.GetCurrentMethod())
                .Build();
        }

        protected override Expression<Func<PostingLineModel, bool>> EntitySelector(string primaryKey) => postingLineModel => postingLineModel.PostingLineIdentification == primaryKey;

        protected override Task<IEnumerable<IPostingLine>> SortAsync(IEnumerable<IPostingLine> postingLineCollection)
        {
            NullGuard.NotNull(postingLineCollection, nameof(postingLineCollection));

            return Task.FromResult(postingLineCollection.OrderByDescending(postingLine => postingLine.PostingDate.Date).ThenByDescending(postingLine => postingLine.SortOrder).AsEnumerable());
        }

        protected override void OnDispose()
        {
            lock (_syncRoot)
            {
                _eventPublisher.RemoveSubscriber(this);
            }

            _accountingModelHandler.Dispose();
            _accountModelHandler.Dispose();
            _budgetAccountModelHandler.Dispose();
            _contactAccountModelHandler.Dispose();
        }

        protected override async Task<PostingLineModel> OnCreateAsync(IPostingLine postingLine, PostingLineModel postingLineModel)
        {
            NullGuard.NotNull(postingLine, nameof(postingLine))
                .NotNull(postingLineModel, nameof(postingLineModel));

            postingLineModel.Accounting = await DbContext.Accountings.SingleAsync(accountingModel => accountingModel.AccountingIdentifier == postingLine.Accounting.Number);
            postingLineModel.Account = await DbContext.Accounts.SingleAsync(accountModel => accountModel.AccountingIdentifier == postingLine.Accounting.Number && accountModel.AccountNumber == postingLine.Account.AccountNumber);

            if (postingLine.BudgetAccount != null)
            {
                postingLineModel.BudgetAccount = await DbContext.BudgetAccounts.SingleAsync(budgetAccountModel => budgetAccountModel.AccountingIdentifier == postingLine.Accounting.Number && budgetAccountModel.AccountNumber == postingLine.BudgetAccount.AccountNumber);
            }

            if (postingLine.ContactAccount != null)
            {
                postingLineModel.ContactAccount = await DbContext.ContactAccounts.SingleAsync(contactAccountModel => contactAccountModel.AccountingIdentifier == postingLine.Accounting.Number && contactAccountModel.AccountNumber == postingLine.ContactAccount.AccountNumber);
            }

            return postingLineModel;
        }

        protected override async Task PrepareReadAsync(AccountingIdentificationState accountingIdentificationState)
        {
            NullGuard.NotNull(accountingIdentificationState, nameof(accountingIdentificationState));

            _accountingModel ??= await _accountingModelHandler.ForAsync(accountingIdentificationState.AccountingIdentifier);
            _accountModelCollection ??= await _accountModelHandler.ForAsync(accountingIdentificationState.AccountingIdentifier);
            _budgetAccountModelCollection ??= await _budgetAccountModelHandler.ForAsync(accountingIdentificationState.AccountingIdentifier);
            _contactAccountModelCollection ??= await _contactAccountModelHandler.ForAsync(accountingIdentificationState.AccountingIdentifier);

            if (_numberOfPostingLines.HasValue || _applyingPostingLines)
            {
                await ForAsync(accountingIdentificationState.AccountingIdentifier, false);
            }
        }

        protected override Task PrepareReadAsync(string primaryKey, AccountingIdentificationState accountingIdentificationState)
        {
            NullGuard.NotNull(accountingIdentificationState, nameof(accountingIdentificationState));

            return PrepareReadAsync(accountingIdentificationState);
        }

        protected override async Task<PostingLineModel> OnReadAsync(PostingLineModel postingLineModel)
        {
            NullGuard.NotNull(postingLineModel, nameof(postingLineModel));

            postingLineModel.Accounting = await OnReadAsync(postingLineModel, _accountingModel);
            postingLineModel.Account = await OnReadAsync(postingLineModel, _toDate, _accountModelCollection);
            postingLineModel.BudgetAccount = await OnReadAsync(postingLineModel, _toDate, _budgetAccountModelCollection);
            postingLineModel.ContactAccount = await OnReadAsync(postingLineModel, _toDate, _contactAccountModelCollection);

            return postingLineModel;
        }

        protected override Task OnUpdateAsync(IPostingLine postingLine, PostingLineModel postingLineModel) => throw new NotSupportedException();

        protected override Task<bool> CanDeleteAsync(PostingLineModel postingLineModel) => throw new NotSupportedException();

        protected override Task<PostingLineModel> OnDeleteAsync(PostingLineModel postingLineModel) => throw new NotSupportedException();

        private IQueryable<PostingLineModel> CreateReader(int? numberOfPostingLines)
        {
            IQueryable<PostingLineModel> reader = Entities;

            if (numberOfPostingLines == null)
            {
                return reader;
            }

            return reader.OrderByDescending(postingLineModel => postingLineModel.PostingDate)
                .ThenByDescending(postingLineModel => postingLineModel.PostingLineIdentifier)
                .Take(numberOfPostingLines.Value);
        }

        private static Task<AccountingModel> OnReadAsync(PostingLineModel postingLineModel, AccountingModel accountModel)
        {
            NullGuard.NotNull(postingLineModel, nameof(postingLineModel));

            return accountModel == null
                ? Task.FromResult(postingLineModel.Accounting)
                : Task.FromResult(postingLineModel.Accounting ?? accountModel);
        }

        private static async Task<AccountModel> OnReadAsync(PostingLineModel postingLineModel, DateTime statusDate, IReadOnlyCollection<AccountModel> accountModelCollection)
        {
            NullGuard.NotNull(postingLineModel, nameof(postingLineModel));

            if (accountModelCollection == null || postingLineModel.Account != null)
            {
                return await OnReadAsync(postingLineModel.Account, statusDate);
            }

            return await OnReadAsync(accountModelCollection.Single(m => m.AccountIdentifier == postingLineModel.AccountIdentifier), statusDate);
        }

        private static async Task<BudgetAccountModel> OnReadAsync(PostingLineModel postingLineModel, DateTime statusDate, IReadOnlyCollection<BudgetAccountModel> budgetAccountModelCollection)
        {
            NullGuard.NotNull(postingLineModel, nameof(postingLineModel));

            if (budgetAccountModelCollection == null || postingLineModel.BudgetAccount != null)
            {
                return await OnReadAsync(postingLineModel.BudgetAccount, statusDate);
            }

            if (postingLineModel.BudgetAccountIdentifier == null)
            {
                return null;
            }

            return await OnReadAsync(budgetAccountModelCollection.Single(m => m.BudgetAccountIdentifier == postingLineModel.BudgetAccountIdentifier.Value), statusDate);
        }

        private static async Task<ContactAccountModel> OnReadAsync(PostingLineModel postingLineModel, DateTime statusDate, IReadOnlyCollection<ContactAccountModel> contactAccountModelCollection)
        {
            NullGuard.NotNull(postingLineModel, nameof(postingLineModel));

            if (contactAccountModelCollection == null || postingLineModel.ContactAccount != null)
            {
                return await OnReadAsync(postingLineModel.ContactAccount, statusDate);
            }

            if (postingLineModel.ContactAccountIdentifier == null)
            {
                return null;
            }

            return await OnReadAsync(contactAccountModelCollection.Single(m => m.ContactAccountIdentifier == postingLineModel.ContactAccountIdentifier.Value), statusDate);
        }

        private static Task<TAccountModel> OnReadAsync<TAccountModel>(TAccountModel accountModel, DateTime statusDate) where TAccountModel : AccountModelBase
        {
            if (accountModel == null)
            {
                return Task.FromResult<TAccountModel>(null);
            }

            accountModel.StatusDate = statusDate;
            accountModel.Deletable = false;

            return Task.FromResult(accountModel);
        }

        #endregion
    }
}