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
    internal class BudgetAccountModelHandler : AccountModelHandlerBase<IBudgetAccount, BudgetAccountModel>, IEventHandler<BudgetInfoModelCollectionLoadedEvent>
    {
        #region Private variables

        private readonly bool _includeBudgetInformation;
        private readonly BudgetInfoModelHandler _budgetInfoModelHandler;
        private IReadOnlyCollection<BudgetInfoModel> _budgetInfoModelCollection;

        #endregion

        #region Constructor

        public BudgetAccountModelHandler(RepositoryContext dbContext, IConverter modelConverter, IEventPublisher eventPublisher, DateTime statusDate, bool includeBudgetInformation, bool includePostingLines)
            : base(dbContext, modelConverter, eventPublisher, statusDate, includePostingLines, includePostingLines ? new PostingLineModelHandler(dbContext, modelConverter, eventPublisher, new DateTime(statusDate.AddYears(-1).Year, 1, 1), statusDate, false, false) : null)
        {
            _includeBudgetInformation = includeBudgetInformation;

            if (_includeBudgetInformation)
            {
                _budgetInfoModelHandler = new BudgetInfoModelHandler(dbContext, modelConverter, EventPublisher, StatusDate);
            }
        }

        #endregion

        #region Properties

        protected override DbSet<BudgetAccountModel> Entities => DbContext.BudgetAccounts;

        protected override IQueryable<BudgetAccountModel> Reader => CreateReader(false, _includeBudgetInformation);

        protected override IQueryable<BudgetAccountModel> UpdateReader => CreateReader(true, true);

        protected override IQueryable<BudgetAccountModel> DeleteReader => CreateReader(true, true);

        #endregion

        #region Methods

        public Task HandleAsync(BudgetInfoModelCollectionLoadedEvent budgetInfoModelCollectionLoadedEvent)
        {
            NullGuard.NotNull(budgetInfoModelCollectionLoadedEvent, nameof(budgetInfoModelCollectionLoadedEvent));

            if (budgetInfoModelCollectionLoadedEvent.FromSameDbContext(DbContext) == false)
            {
                return Task.CompletedTask;
            }

            lock (SyncRoot)
            {
                if (_budgetInfoModelCollection != null)
                {
                    return Task.CompletedTask;
                }

                if (budgetInfoModelCollectionLoadedEvent.StatusDate != StatusDate || _includeBudgetInformation == false)
                {
                    return Task.CompletedTask;
                }

                _budgetInfoModelCollection = budgetInfoModelCollectionLoadedEvent.ModelCollection;

                return Task.CompletedTask;
            }
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            _budgetInfoModelHandler?.Dispose();
        }

        protected override async Task<BudgetAccountModel> OnCreateAsync(IBudgetAccount budgetAccount, BudgetAccountModel budgetAccountModel)
        {
            NullGuard.NotNull(budgetAccount, nameof(budgetAccount))
                .NotNull(budgetAccountModel, nameof(budgetAccountModel));

            budgetAccountModel = await base.OnCreateAsync(budgetAccount, budgetAccountModel);
            budgetAccountModel.BudgetAccountGroup = await DbContext.BudgetAccountGroups.SingleAsync(budgetAccountGroupModel => budgetAccountGroupModel.BudgetAccountGroupIdentifier == budgetAccount.BudgetAccountGroup.Number);

            budgetAccount.BudgetInfoCollection.EnsurePopulation(budgetAccount);

            await _budgetInfoModelHandler.CreateOrUpdateAsync(budgetAccount.BudgetInfoCollection, budgetAccountModel);

            return budgetAccountModel;
        }

        protected override async Task PrepareReadAsync(AccountingIdentificationState accountingIdentificationState)
        {
            NullGuard.NotNull(accountingIdentificationState, nameof(accountingIdentificationState));

            await base.PrepareReadAsync(accountingIdentificationState);

            if (_includeBudgetInformation == false)
            {
                return;
            }

            _budgetInfoModelCollection ??= await _budgetInfoModelHandler.ForAsync(accountingIdentificationState.AccountingIdentifier);
        }

        protected override async Task<BudgetAccountModel> OnReadAsync(BudgetAccountModel budgetAccountModel)
        {
            NullGuard.NotNull(budgetAccountModel, nameof(budgetAccountModel));

            budgetAccountModel.BudgetInfos = await OnReadAsync(budgetAccountModel, _budgetInfoModelCollection, _budgetInfoModelHandler);

            return await base.OnReadAsync(budgetAccountModel);
        }

        protected override async Task OnUpdateAsync(IBudgetAccount budgetAccount, BudgetAccountModel budgetAccountModel)
        {
            NullGuard.NotNull(budgetAccount, nameof(budgetAccount))
                .NotNull(budgetAccountModel, nameof(budgetAccountModel));

            await base.OnUpdateAsync(budgetAccount, budgetAccountModel);

            budgetAccountModel.BudgetAccountGroupIdentifier = budgetAccount.BudgetAccountGroup.Number;
            budgetAccountModel.BudgetAccountGroup = await DbContext.BudgetAccountGroups.SingleAsync(budgetAccountGroupModel => budgetAccountGroupModel.BudgetAccountGroupIdentifier == budgetAccount.BudgetAccountGroup.Number);

            budgetAccount.BudgetInfoCollection.EnsurePopulation(budgetAccount);

            await _budgetInfoModelHandler.CreateOrUpdateAsync(budgetAccount.BudgetInfoCollection, budgetAccountModel);
        }

        protected override async Task<bool> CanDeleteAsync(BudgetAccountModel budgetAccountModel)
        {
            NullGuard.NotNull(budgetAccountModel, nameof(budgetAccountModel));

            if (budgetAccountModel.BudgetInfos == null || budgetAccountModel.PostingLines == null)
            {
                return false;
            }

            return budgetAccountModel.PostingLines.Any() == false || await _budgetInfoModelHandler.IsDeletable(budgetAccountModel.BudgetInfos);
        }

        protected override async Task<BudgetAccountModel> OnDeleteAsync(BudgetAccountModel budgetAccountModel)
        {
            NullGuard.NotNull(budgetAccountModel, nameof(budgetAccountModel));

            await PostingLineModelHandler.DeleteAsync(budgetAccountModel.PostingLines);
            await _budgetInfoModelHandler.DeleteAsync(budgetAccountModel.BudgetInfos);

            return await base.OnDeleteAsync(budgetAccountModel);
        }

        protected override Task PublishModelCollectionLoadedEvent(IReadOnlyCollection<BudgetAccountModel> budgetAccountModelCollection)
        {
            NullGuard.NotNull(budgetAccountModelCollection, nameof(budgetAccountModelCollection));

            lock (SyncRoot)
            {
                EventPublisher.PublishAsync(new BudgetAccountModelCollectionLoadedEvent(DbContext, budgetAccountModelCollection, StatusDate, _includeBudgetInformation))
                    .GetAwaiter()
                    .GetResult();
            }

            return Task.CompletedTask;
        }

        protected override void ExtractPostingLines(BudgetAccountModel budgetAccountModel, IReadOnlyCollection<PostingLineModel> postingLineCollection)
        {
            NullGuard.NotNull(budgetAccountModel, nameof(budgetAccountModel))
                .NotNull(postingLineCollection, nameof(postingLineCollection));

            budgetAccountModel.ExtractPostingLines(postingLineCollection);
        }

        private IQueryable<BudgetAccountModel> CreateReader(bool includeAccounting, bool includeBudgetInformation)
        {
            IQueryable<BudgetAccountModel> reader = Entities
                .Include(budgetAccountModel => budgetAccountModel.BasicAccount)
                .Include(budgetAccountModel => budgetAccountModel.BudgetAccountGroup);

            if (includeAccounting)
            {
                reader = reader.Include(budgetAccountModel => budgetAccountModel.Accounting).ThenInclude(accountingModel => accountingModel.LetterHead);
            }

            if (includeBudgetInformation == false || _budgetInfoModelCollection != null)
            {
                return reader;
            }

            return reader.Include(budgetAccountModel => budgetAccountModel.BudgetInfos).ThenInclude(budgetInfoModel => budgetInfoModel.YearMonth);
        }

        private static async Task<List<BudgetInfoModel>> OnReadAsync(BudgetAccountModel budgetAccountModel, IReadOnlyCollection<BudgetInfoModel> budgetInfoModelCollection, BudgetInfoModelHandler budgetInfoModelHandler)
        {
            NullGuard.NotNull(budgetAccountModel, nameof(budgetAccountModel));

            if (budgetInfoModelCollection == null || budgetInfoModelHandler == null)
            {
                return budgetAccountModel.BudgetInfos;
            }

            if (budgetAccountModel.BudgetInfos == null)
            {
                budgetAccountModel.ExtractBudgetInfos(budgetInfoModelCollection);
            }

            return (await budgetInfoModelHandler.ReadAsync(budgetAccountModel.BudgetInfos)).ToList();
        }

        #endregion
    }
}