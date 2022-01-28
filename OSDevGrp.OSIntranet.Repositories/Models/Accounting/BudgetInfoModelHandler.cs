using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Events;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class BudgetInfoModelHandler : InfoModelHandlerBase<IBudgetInfo, BudgetInfoModel>, IEventHandler<BudgetAccountModelCollectionLoadedEvent>
    {
        #region Private variables

        private readonly IEventPublisher _eventPublisher;
        private readonly DateTime _statusDate;
        private readonly BudgetAccountModelHandler _budgetAccountModelHandler;
        private readonly object _syncRoot = new object();
        private IReadOnlyCollection<BudgetAccountModel> _budgetAccountModelCollection;

        #endregion

        #region Constructor

        public BudgetInfoModelHandler(RepositoryContext dbContext, IConverter modelConverter, IEventPublisher eventPublisher, DateTime statusDate) 
            : base(dbContext, modelConverter)
        {
            NullGuard.NotNull(eventPublisher, nameof(eventPublisher));

            _eventPublisher = eventPublisher;
            _statusDate = statusDate.Date;
            _budgetAccountModelHandler = new BudgetAccountModelHandler(dbContext, modelConverter, _eventPublisher, _statusDate, false, false);

            _eventPublisher.AddSubscriber(this);
        }

        #endregion

        #region Properties

        protected override DbSet<BudgetInfoModel> Entities => DbContext.BudgetInfos;

        protected override Func<IBudgetInfo, Tuple<int, string, short, short>> PrimaryKey => budgetInfo => new Tuple<int, string, short, short>(budgetInfo.BudgetAccount.Accounting.Number, budgetInfo.BudgetAccount.AccountNumber, budgetInfo.Year, budgetInfo.Month);

        protected override IQueryable<BudgetInfoModel> Reader => CreateReader(true);

        #endregion

        #region Methods

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

                if (budgetAccountModelCollectionLoadedEvent.StatusDate != _statusDate)
                {
                    return Task.CompletedTask;
                }

                _budgetAccountModelCollection = budgetAccountModelCollectionLoadedEvent.ModelCollection;

                return Task.CompletedTask;
            }
        }

        internal async Task CreateOrUpdateAsync(IBudgetInfoCollection budgetInfoCollection, BudgetAccountModel budgetAccountModel)
        {
            NullGuard.NotNull(budgetInfoCollection, nameof(budgetInfoCollection))
                .NotNull(budgetAccountModel, nameof(budgetAccountModel));

            IBudgetInfo budgetInfo = budgetInfoCollection.First();
            while (budgetInfo != null)
            {
                BudgetInfoModel currentBudgetInfoModel = budgetAccountModel.BudgetInfos.SingleOrDefault(budgetInfoModel => budgetInfoModel.YearMonth.Year == budgetInfo.Year && budgetInfoModel.YearMonth.Month == budgetInfo.Month);
                BudgetInfoModel previousBudgetInfoModel = budgetAccountModel.BudgetInfos.Where(budgetInfoModel => budgetInfoModel.YearMonth.Year < budgetInfo.Year || budgetInfoModel.YearMonth.Year == budgetInfo.Year && budgetInfoModel.YearMonth.Month < budgetInfo.Month)
                    .OrderByDescending(budgetInfoModel => budgetInfoModel.YearMonth.Year)
                    .ThenByDescending(budgetInfoModel => budgetInfoModel.YearMonth.Month)
                    .FirstOrDefault();

                if (currentBudgetInfoModel != null)
                {
                    if (previousBudgetInfoModel != null && budgetInfo.Income == previousBudgetInfoModel.Income && budgetInfo.Expenses == previousBudgetInfoModel.Expenses)
                    {
                        budgetAccountModel.BudgetInfos.Remove(await OnDeleteAsync(currentBudgetInfoModel));

                        budgetInfo = budgetInfoCollection.Next(budgetInfo);
                        continue;
                    }

                    if (budgetInfo.Income == currentBudgetInfoModel.Income && budgetInfo.Expenses == currentBudgetInfoModel.Expenses)
                    {
                        budgetInfo = budgetInfoCollection.Next(budgetInfo);
                        continue;
                    }

                    await OnUpdateAsync(budgetInfo, currentBudgetInfoModel);

                    budgetInfo = budgetInfoCollection.Next(budgetInfo);
                    continue;
                }

                if (previousBudgetInfoModel != null)
                {
                    if (budgetInfo.Income == previousBudgetInfoModel.Income && budgetInfo.Expenses == previousBudgetInfoModel.Expenses)
                    {
                        budgetInfo = budgetInfoCollection.Next(budgetInfo);
                        continue;
                    }

                    await CreateAsync(budgetInfo, budgetAccountModel);

                    budgetInfo = budgetInfoCollection.Next(budgetInfo);
                    continue;
                }

                if (budgetInfo.Income == 0M && budgetInfo.Expenses == 0M)
                {
                    budgetInfo = budgetInfoCollection.Next(budgetInfo);
                    continue;
                }

                await CreateAsync(budgetInfo, budgetAccountModel);

                budgetInfo = budgetInfoCollection.Next(budgetInfo);
            }
        }

        internal override Task<IBudgetInfo> DeleteAsync(BudgetInfoModel budgetInfoModel)
        {
            NullGuard.NotNull(budgetInfoModel, nameof(budgetInfoModel));

            return DeleteAsync(new Tuple<int, string, short, short>(budgetInfoModel.BudgetAccount.Accounting.AccountingIdentifier, budgetInfoModel.BudgetAccount.AccountNumber, budgetInfoModel.YearMonth.Year, budgetInfoModel.YearMonth.Month));
        }

        internal async Task<IReadOnlyCollection<BudgetInfoModel>> ForAsync(int accountingNumber)
        {
            await PrepareReadAsync(new AccountingIdentificationState(accountingNumber));

            IReadOnlyCollection<BudgetInfoModel> budgetInfoModelCollection = (await ReadAsync(await CreateReader(false).Where(budgetInfoModel => budgetInfoModel.BudgetAccount.AccountingIdentifier == accountingNumber).ToArrayAsync())).ToArray();

            lock (_syncRoot)
            {
                _eventPublisher.PublishAsync(new BudgetInfoModelCollectionLoadedEvent(DbContext, budgetInfoModelCollection, _statusDate))
                    .GetAwaiter()
                    .GetResult();
            }

            return budgetInfoModelCollection;
        }

        protected override Expression<Func<BudgetInfoModel, bool>> EntitySelector(Tuple<int, string, short, short> primaryKey) => budgetInfoModel => budgetInfoModel.BudgetAccount.Accounting.AccountingIdentifier == primaryKey.Item1 && budgetInfoModel.BudgetAccount.AccountNumber == primaryKey.Item2 && budgetInfoModel.YearMonth.Year == primaryKey.Item3 && budgetInfoModel.YearMonth.Month == primaryKey.Item4;

        protected override Task<IEnumerable<IBudgetInfo>> SortAsync(IEnumerable<IBudgetInfo> budgetInfoCollection)
        {
            NullGuard.NotNull(budgetInfoCollection, nameof(budgetInfoCollection));

            return Task.FromResult(budgetInfoCollection.OrderBy(budgetInfo => budgetInfo.BudgetAccount.Accounting.Number).ThenBy(budgetInfo => budgetInfo.BudgetAccount.AccountNumber).ThenByDescending(budgetInfo => budgetInfo.Year).ThenByDescending(budgetInfo => budgetInfo.Month).AsEnumerable());
        }

        protected override void OnDispose()
        {
            lock (_syncRoot)
            {
                _eventPublisher.RemoveSubscriber(this);
            }

            base.OnDispose();
        }

        protected override async Task PrepareReadAsync(AccountingIdentificationState accountingIdentificationState)
        {
            NullGuard.NotNull(accountingIdentificationState, nameof(accountingIdentificationState));

            _budgetAccountModelCollection ??= await _budgetAccountModelHandler.ForAsync(accountingIdentificationState.AccountingIdentifier);
        }

        protected override Task PrepareReadAsync(Tuple<int, string, short, short> primaryKey, AccountingIdentificationState accountingIdentificationState)
        {
            NullGuard.NotNull(primaryKey, nameof(primaryKey))
                .NotNull(accountingIdentificationState, nameof(accountingIdentificationState));

            return PrepareReadAsync(new AccountingIdentificationState(primaryKey.Item1));
        }

        protected override Task<BudgetInfoModel> OnReadAsync(BudgetInfoModel budgetInfoModel)
        {
            NullGuard.NotNull(budgetInfoModel, nameof(budgetInfoModel));

            if (budgetInfoModel.BudgetAccount == null && _budgetAccountModelCollection != null)
            {
                budgetInfoModel.BudgetAccount = _budgetAccountModelCollection.Single(budgetAccountModel => budgetAccountModel.BudgetAccountIdentifier == budgetInfoModel.BudgetAccountIdentifier);
            }

            return Task.FromResult(budgetInfoModel);
        }

        protected override async Task OnUpdateAsync(IBudgetInfo budgetInfo, BudgetInfoModel budgetInfoModel)
        {
            NullGuard.NotNull(budgetInfo, nameof(budgetInfo))
                .NotNull(budgetInfoModel, nameof(budgetInfoModel));

            await base.OnUpdateAsync(budgetInfo, budgetInfoModel);

            budgetInfoModel.Income = budgetInfo.Income;
            budgetInfoModel.Expenses = budgetInfo.Expenses;
        }

        private async Task CreateAsync(IBudgetInfo budgetInfo, BudgetAccountModel budgetAccountModel)
        {
            NullGuard.NotNull(budgetInfo, nameof(budgetInfo))
                .NotNull(budgetAccountModel, nameof(budgetAccountModel));

            BudgetInfoModel budgetInfoModel = ModelConverter.Convert<IBudgetInfo, BudgetInfoModel>(budgetInfo);
            budgetInfoModel.BudgetAccountIdentifier = budgetAccountModel.BudgetAccountIdentifier;
            budgetInfoModel.BudgetAccount = budgetAccountModel;

            EntityEntry<BudgetInfoModel> budgetInfoModelEntityEntry = await Entities.AddAsync(await OnCreateAsync(budgetInfo, budgetInfoModel));

            if (budgetAccountModel.BudgetInfos.Contains(budgetInfoModelEntityEntry.Entity) == false)
            {
                budgetAccountModel.BudgetInfos.Add(budgetInfoModelEntityEntry.Entity);
            }
        }

        private IQueryable<BudgetInfoModel> CreateReader(bool includeBudgetAccount)
        {
            IQueryable<BudgetInfoModel> reader = Entities
                .Include(budgetInfoModel => budgetInfoModel.BudgetAccount)
                .Include(budgetInfoModel => budgetInfoModel.YearMonth);

            if (includeBudgetAccount == false || _budgetAccountModelCollection != null)
            {
                return reader;
            }

            return reader.Include(budgetInfoModel => budgetInfoModel.BudgetAccount).ThenInclude(budgetAccountModel => budgetAccountModel.Accounting).ThenInclude(accountingModel => accountingModel.LetterHead)
                .Include(budgetInfoModel => budgetInfoModel.BudgetAccount).ThenInclude(budgetAccountModel => budgetAccountModel.BasicAccount)
                .Include(budgetInfoModel => budgetInfoModel.BudgetAccount).ThenInclude(budgetAccountModel => budgetAccountModel.BudgetAccountGroup);
        }

        #endregion
    }
}