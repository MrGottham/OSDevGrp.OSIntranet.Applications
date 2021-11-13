using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Converters.Extensions;
using OSDevGrp.OSIntranet.Repositories.Events;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class AccountModelHandler : AccountModelHandlerBase<IAccount, AccountModel>, IEventHandler<CreditInfoModelCollectionLoadedEvent>
    {
        #region Private variables

        private readonly bool _includeCreditInformation;
        private readonly CreditInfoModelHandler _creditInfoModelHandler;
        private IReadOnlyCollection<CreditInfoModel> _creditInfoModelCollection;

        #endregion

        #region Constructor

        public AccountModelHandler(RepositoryContext dbContext, IConverter modelConverter, IEventPublisher eventPublisher, DateTime statusDate, bool includeCreditInformation, bool includePostingLines) 
            : base(dbContext, modelConverter, eventPublisher, statusDate, includePostingLines, includePostingLines ? new PostingLineModelHandler(dbContext, modelConverter, eventPublisher, DateTime.MinValue, statusDate, false, false) : null)
        {
            _includeCreditInformation = includeCreditInformation;

            if (_includeCreditInformation)
            {
                _creditInfoModelHandler = new CreditInfoModelHandler(dbContext, modelConverter, EventPublisher, StatusDate);
            }
        }

        #endregion

        #region Properties

        protected override DbSet<AccountModel> Entities => DbContext.Accounts;

        protected override IQueryable<AccountModel> Reader => CreateReader(false, _includeCreditInformation);

        protected override IQueryable<AccountModel> UpdateReader => CreateReader(true, true);

        protected override IQueryable<AccountModel> DeleteReader => CreateReader(true, true);

        #endregion

        #region Methods

        public Task HandleAsync(CreditInfoModelCollectionLoadedEvent creditInfoModelCollectionLoadedEvent)
        {
            NullGuard.NotNull(creditInfoModelCollectionLoadedEvent, nameof(creditInfoModelCollectionLoadedEvent));

            if (creditInfoModelCollectionLoadedEvent.FromSameDbContext(DbContext) == false)
            {
                return Task.CompletedTask;
            }

            lock (SyncRoot)
            {
                if (_creditInfoModelCollection != null)
                {
                    return Task.CompletedTask;
                }

                if (creditInfoModelCollectionLoadedEvent.StatusDate != StatusDate || _includeCreditInformation == false)
                {
                    return Task.CompletedTask;
                }

                _creditInfoModelCollection = creditInfoModelCollectionLoadedEvent.ModelCollection;

                return Task.CompletedTask;
            }
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            _creditInfoModelHandler?.Dispose();
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

        protected override async Task PrepareReadAsync(AccountingIdentificationState accountingIdentificationState)
        {
            NullGuard.NotNull(accountingIdentificationState, nameof(accountingIdentificationState));

            await base.PrepareReadAsync(accountingIdentificationState);

            if (_includeCreditInformation == false)
            {
                return;
            }

            _creditInfoModelCollection ??= await _creditInfoModelHandler.ForAsync(accountingIdentificationState.AccountingIdentifier);
        }

        protected override async Task<AccountModel> OnReadAsync(AccountModel accountModel)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel));

            accountModel.CreditInfos = await OnReadAsync(accountModel, _creditInfoModelCollection, _creditInfoModelHandler);

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

            if (accountModel.CreditInfos == null || accountModel.PostingLines == null)
            {
                return false;
            }

            if (accountModel.PostingLines.Any() || await _creditInfoModelHandler.IsDeletable(accountModel.CreditInfos) == false)
            {
                return false;
            }

            return await DbContext.PostingLines.FirstOrDefaultAsync(postingLineModel => postingLineModel.AccountIdentifier == accountModel.AccountIdentifier) == null;
        }

        protected override async Task<AccountModel> OnDeleteAsync(AccountModel accountModel)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel));

            await PostingLineModelHandler.DeleteAsync(accountModel.PostingLines);
            await _creditInfoModelHandler.DeleteAsync(accountModel.CreditInfos);

            return await base.OnDeleteAsync(accountModel);
        }

        protected override Task PublishModelCollectionLoadedEvent(IReadOnlyCollection<AccountModel> accountModelCollection)
        {
            NullGuard.NotNull(accountModelCollection, nameof(accountModelCollection));

            lock (SyncRoot)
            {
                EventPublisher.PublishAsync(new AccountModelCollectionLoadedEvent(DbContext, accountModelCollection, StatusDate, _includeCreditInformation))
                    .GetAwaiter()
                    .GetResult();
            }

            return Task.CompletedTask;
        }

        protected override void ExtractPostingLines(AccountModel accountModel, IReadOnlyCollection<PostingLineModel> postingLineCollection)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel))
                .NotNull(postingLineCollection, nameof(postingLineCollection));

            accountModel.ExtractPostingLines(postingLineCollection);
        }

        private IQueryable<AccountModel> CreateReader(bool includeAccounting, bool includeCreditInformation)
        {
            IQueryable<AccountModel> reader = Entities
                .Include(accountModel => accountModel.BasicAccount)
                .Include(accountModel => accountModel.AccountGroup);

            if (includeAccounting)
            {
                reader = reader.Include(accountModel => accountModel.Accounting).ThenInclude(accountingModel => accountingModel.LetterHead);
            }

            if (includeCreditInformation == false || _creditInfoModelCollection != null)
            {
                return reader;
            }

            return reader.Include(accountModel => accountModel.CreditInfos).ThenInclude(creditInfoModel => creditInfoModel.YearMonth);
        }

        private static async Task<List<CreditInfoModel>> OnReadAsync(AccountModel accountModel, IReadOnlyCollection<CreditInfoModel> creditInfoModelCollection, CreditInfoModelHandler creditInfoModelHandler)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel));

            if (creditInfoModelCollection == null || creditInfoModelHandler == null)
            {
                return accountModel.CreditInfos;
            }

            if (accountModel.CreditInfos == null)
            {
                accountModel.ExtractCreditInfos(creditInfoModelCollection);
            }

            return (await creditInfoModelHandler.ReadAsync(accountModel.CreditInfos)).ToList();
        }

        #endregion
    }
}