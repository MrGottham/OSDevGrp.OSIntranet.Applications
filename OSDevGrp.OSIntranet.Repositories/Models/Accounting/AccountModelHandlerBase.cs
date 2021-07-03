using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal abstract class AccountModelHandlerBase<TAccount, TAccountModel> : ModelHandlerBase<TAccount, RepositoryContext, TAccountModel, Tuple<int, string>, AccountingIdentificationState> where TAccount : class, IAccountBase where TAccountModel : AccountModelBase, new() 
    {
        #region Private variables

        private readonly bool _includePostingLines;
        private AccountingModel _accountingModel;
        private IReadOnlyCollection<PostingLineModel> _postingLineModelCollection;

        #endregion

        #region Constructor

        protected AccountModelHandlerBase(RepositoryContext dbContext, IConverter modelConverter, IEventPublisher eventPublisher, DateTime statusDate, bool includePostingLines, PostingLineModelHandler postingLineModelHandler) 
            : base(dbContext, modelConverter)
        {
            NullGuard.NotNull(eventPublisher, nameof(eventPublisher));

            _includePostingLines = includePostingLines;

            EventPublisher = eventPublisher;
            StatusDate = statusDate.Date;
            AccountingModelHandler = new AccountingModelHandler(dbContext, modelConverter, eventPublisher, statusDate, false, false);
            PostingLineModelHandler = postingLineModelHandler;
            SyncRoot = new object();
        }

        #endregion

        #region Properties

        protected override Func<TAccount, Tuple<int, string>> PrimaryKey => account => new Tuple<int, string>(account.Accounting.Number, account.AccountNumber);

        protected IEventPublisher EventPublisher { get; }

        protected DateTime StatusDate { get; }

        protected AccountingModelHandler AccountingModelHandler { get; }

        protected PostingLineModelHandler PostingLineModelHandler { get; }

        protected object SyncRoot { get; }

        #endregion

        #region Methods

        internal async Task<bool> ExistsAsync(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return await ReadAsync(accountingNumber, accountNumber) != null;
        }

        internal Task<TAccount> ReadAsync(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return ReadAsync(new Tuple<int, string>(accountingNumber, accountNumber), new AccountingIdentificationState(accountingNumber));
        }

        internal async Task<IEnumerable<TAccountModel>> ReadAsync(IEnumerable<TAccountModel> accountModelCollection)
        {
            NullGuard.NotNull(accountModelCollection, nameof(accountModelCollection));

            return await Task.WhenAll(accountModelCollection.Select(OnReadAsync));
        }

        internal async Task<IReadOnlyCollection<TAccountModel>> ForAsync(int accountingNumber)
        {
            await PrepareReadAsync(new AccountingIdentificationState(accountingNumber));

            IReadOnlyCollection<TAccountModel> accountModelCollection = (await ReadAsync(Reader.Where(accountModel => accountModel.AccountingIdentifier == accountingNumber))).ToArray();

            await PublishModelCollectionLoadedEvent(accountModelCollection);

            return accountModelCollection;
        }

        internal async Task DeleteAsync(IList<TAccountModel> accountModelCollection)
        {
            NullGuard.NotNull(accountModelCollection, nameof(accountModelCollection));

            TAccountModel accountModelToDelete = accountModelCollection.FirstOrDefault();
            while (accountModelToDelete != null)
            {
                if (await CanDeleteAsync(accountModelToDelete) == false)
                {
                    throw new IntranetExceptionBuilder(ErrorCode.UnableToDeleteOneOrMoreObjects, nameof(TAccountModel))
                        .WithMethodBase(MethodBase.GetCurrentMethod())
                        .Build();
                }

                if (await DeleteAsync(new Tuple<int, string>(accountModelToDelete.AccountingIdentifier, accountModelToDelete.AccountNumber), new AccountingIdentificationState(accountModelToDelete.AccountingIdentifier)) != null)
                {
                    throw new IntranetExceptionBuilder(ErrorCode.UnableToDeleteOneOrMoreObjects, nameof(TAccountModel))
                        .WithMethodBase(MethodBase.GetCurrentMethod())
                        .Build();
                }

                accountModelToDelete = accountModelCollection.FirstOrDefault();
            }
        }

        protected override Expression<Func<TAccountModel, bool>> EntitySelector(Tuple<int, string> primaryKey) => accountModel => accountModel.AccountingIdentifier == primaryKey.Item1 && accountModel.AccountNumber == primaryKey.Item2;

        protected override Task<IEnumerable<TAccount>> SortAsync(IEnumerable<TAccount> accountCollection)
        {
            NullGuard.NotNull(accountCollection, nameof(accountCollection));

            return Task.FromResult(accountCollection.OrderBy(account => account.Accounting.Number).ThenBy(account => account.AccountNumber).AsEnumerable());
        }

        protected override void OnDispose()
        {
            AccountingModelHandler.Dispose();
            PostingLineModelHandler?.Dispose();
        }

        protected override async Task<TAccountModel> OnCreateAsync(TAccount account, TAccountModel accountModel)
        {
            NullGuard.NotNull(account, nameof(account))
                .NotNull(accountModel, nameof(accountModel));

            EntityEntry<BasicAccountModel> basicAccountEntityEntry = await DbContext.BasicAccounts.AddAsync(accountModel.BasicAccount);

            accountModel.Accounting = await AccountingModelHandler.ForAsync(account.Accounting.Number);
            accountModel.BasicAccount = basicAccountEntityEntry.Entity;

            return accountModel;
        }

        protected override async Task PrepareReadAsync(AccountingIdentificationState accountingIdentificationState)
        {
            NullGuard.NotNull(accountingIdentificationState, nameof(accountingIdentificationState));

            _accountingModel ??= await AccountingModelHandler.ForAsync(accountingIdentificationState.AccountingIdentifier);

            if (_includePostingLines == false || PostingLineModelHandler == null)
            {
                return;
            }

            _postingLineModelCollection ??= await PostingLineModelHandler.ForAsync(accountingIdentificationState.AccountingIdentifier);
        }

        protected override Task PrepareReadAsync(Tuple<int, string> primaryKey, AccountingIdentificationState accountingIdentificationState)
        {
            NullGuard.NotNull(primaryKey, nameof(primaryKey))
                .NotNull(accountingIdentificationState, nameof(accountingIdentificationState));

            return PrepareReadAsync(new AccountingIdentificationState(primaryKey.Item1));
        }

        protected override async Task<TAccountModel> OnReadAsync(TAccountModel accountModel)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel));

            accountModel.Accounting ??= _accountingModel;
            accountModel.StatusDate = StatusDate;

            accountModel.PostingLines = await OnReadAsync(accountModel, _postingLineModelCollection, PostingLineModelHandler);

            return accountModel;
        }

        protected override async Task OnUpdateAsync(TAccount account, TAccountModel accountModel)
        {
            NullGuard.NotNull(account, nameof(account))
                .NotNull(accountModel, nameof(accountModel));

            accountModel.AccountingIdentifier = account.Accounting.Number;
            accountModel.Accounting = await AccountingModelHandler.ForAsync(account.Accounting.Number);
            accountModel.AccountNumber = account.AccountNumber;
            accountModel.BasicAccount.AccountName = account.AccountName;
            accountModel.BasicAccount.Description = account.Description;
            accountModel.BasicAccount.Note = account.Note;
        }

        protected override async Task<TAccountModel> OnDeleteAsync(TAccountModel accountModel)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel));

            BasicAccountModel basicAccountModel = await DbContext.BasicAccounts.SingleAsync(model => model.BasicAccountIdentifier == accountModel.BasicAccountIdentifier);
            DbContext.BasicAccounts.Remove(basicAccountModel);

            return accountModel;
        }

        protected abstract Task PublishModelCollectionLoadedEvent(IReadOnlyCollection<TAccountModel> accountModelCollection);

        protected abstract void ExtractPostingLines(TAccountModel accountModel, IReadOnlyCollection<PostingLineModel> postingLineCollection);

        private async Task<List<PostingLineModel>> OnReadAsync(TAccountModel accountModel, IReadOnlyCollection<PostingLineModel> postingLineModelCollection, PostingLineModelHandler postingLineModelHandler)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel));

            if (postingLineModelCollection == null || postingLineModelHandler == null)
            {
                return accountModel.PostingLines;
            }

            if (accountModel.PostingLines == null)
            {
                ExtractPostingLines(accountModel, postingLineModelCollection);
            }

            return (await postingLineModelHandler.ReadAsync(accountModel.PostingLines)).ToList();
        }

        #endregion
    }
}