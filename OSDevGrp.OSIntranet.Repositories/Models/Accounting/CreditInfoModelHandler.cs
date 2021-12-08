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
    internal class CreditInfoModelHandler : InfoModelHandlerBase<ICreditInfo, CreditInfoModel>, IEventHandler<AccountModelCollectionLoadedEvent>
    {
        #region Private variables

        private readonly IEventPublisher _eventPublisher;
        private readonly DateTime _statusDate;
        private readonly AccountModelHandler _accountModelHandler;
        private readonly object _syncRoot = new object();
        private IReadOnlyCollection<AccountModel> _accountModelCollection;

        #endregion

        #region Constructor

        public CreditInfoModelHandler(RepositoryContext dbContext, IConverter modelConverter, IEventPublisher eventPublisher, DateTime statusDate) 
            : base(dbContext, modelConverter)
        {
            NullGuard.NotNull(eventPublisher, nameof(eventPublisher));

            _eventPublisher = eventPublisher;
            _statusDate = statusDate.Date;
            _accountModelHandler = new AccountModelHandler(dbContext, modelConverter, _eventPublisher, _statusDate, false, false);

            _eventPublisher.AddSubscriber(this);
        }

        #endregion

        #region Properties

        protected override DbSet<CreditInfoModel> Entities => DbContext.CreditInfos;

        protected override Func<ICreditInfo, Tuple<int, string, short, short>> PrimaryKey => creditInfo => new Tuple<int, string, short, short>(creditInfo.Account.Accounting.Number, creditInfo.Account.AccountNumber, creditInfo.Year, creditInfo.Month);

        protected override IQueryable<CreditInfoModel> Reader => CreateReader(true);

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

                if (accountModelCollectionLoadedEvent.StatusDate != _statusDate)
                {
                    return Task.CompletedTask;
                }

                _accountModelCollection = accountModelCollectionLoadedEvent.ModelCollection;

                return Task.CompletedTask;
            }
        }

        internal async Task CreateOrUpdateAsync(ICreditInfoCollection creditInfoCollection, AccountModel accountModel)
        {
            NullGuard.NotNull(creditInfoCollection, nameof(creditInfoCollection))
                .NotNull(accountModel, nameof(accountModel));

            ICreditInfo creditInfo = creditInfoCollection.First();
            while (creditInfo != null)
            {
                CreditInfoModel currentCreditInfoModel = accountModel.CreditInfos.SingleOrDefault(creditInfoModel => creditInfoModel.YearMonth.Year == creditInfo.Year && creditInfoModel.YearMonth.Month == creditInfo.Month);
                CreditInfoModel previousCreditInfoModel = accountModel.CreditInfos.Where(creditInfoModel => creditInfoModel.YearMonth.Year < creditInfo.Year || creditInfoModel.YearMonth.Year == creditInfo.Year && creditInfoModel.YearMonth.Month < creditInfo.Month)
                    .OrderByDescending(creditInfoModel => creditInfoModel.YearMonth.Year)
                    .ThenByDescending(creditInfoModel => creditInfoModel.YearMonth.Month)
                    .FirstOrDefault();

                if (currentCreditInfoModel != null)
                {
                    if (previousCreditInfoModel != null && creditInfo.Credit == previousCreditInfoModel.Credit)
                    {
                        accountModel.CreditInfos.Remove(await OnDeleteAsync(currentCreditInfoModel));

                        creditInfo = creditInfoCollection.Next(creditInfo);
                        continue;
                    }

                    if (creditInfo.Credit == currentCreditInfoModel.Credit)
                    {
                        creditInfo = creditInfoCollection.Next(creditInfo);
                        continue;
                    }

                    await OnUpdateAsync(creditInfo, currentCreditInfoModel);

                    creditInfo = creditInfoCollection.Next(creditInfo);
                    continue;
                }

                if (previousCreditInfoModel != null)
                {
                    if (creditInfo.Credit == previousCreditInfoModel.Credit)
                    {
                        creditInfo = creditInfoCollection.Next(creditInfo);
                        continue;
                    }

                    await CreateAsync(creditInfo, accountModel);

                    creditInfo = creditInfoCollection.Next(creditInfo);
                    continue;
                }

                if (creditInfo.Credit == 0M)
                {
                    creditInfo = creditInfoCollection.Next(creditInfo);
                    continue;
                }

                await CreateAsync(creditInfo, accountModel);

                creditInfo = creditInfoCollection.Next(creditInfo);
            }
        }

        internal override Task<ICreditInfo> DeleteAsync(CreditInfoModel creditInfoModel)
        {
            NullGuard.NotNull(creditInfoModel, nameof(creditInfoModel));

            return DeleteAsync(new Tuple<int, string, short, short>(creditInfoModel.Account.Accounting.AccountingIdentifier, creditInfoModel.Account.AccountNumber, creditInfoModel.YearMonth.Year, creditInfoModel.YearMonth.Month));
        }

        internal async Task<IReadOnlyCollection<CreditInfoModel>> ForAsync(int accountingNumber)
        {
            await PrepareReadAsync(new AccountingIdentificationState(accountingNumber));

            IReadOnlyCollection<CreditInfoModel> creditInfoModelCollection = (await ReadAsync(CreateReader(false).Where(creditInfoModel => creditInfoModel.Account.AccountingIdentifier == accountingNumber))).ToArray();

            lock (_syncRoot)
            {
                _eventPublisher.PublishAsync(new CreditInfoModelCollectionLoadedEvent(DbContext, creditInfoModelCollection, _statusDate))
                    .GetAwaiter()
                    .GetResult();
            }

            return creditInfoModelCollection;
        }

        protected override Expression<Func<CreditInfoModel, bool>> EntitySelector(Tuple<int, string, short, short> primaryKey) => creditInfoModel => creditInfoModel.Account.Accounting.AccountingIdentifier == primaryKey.Item1 && creditInfoModel.Account.AccountNumber == primaryKey.Item2 && creditInfoModel.YearMonth.Year == primaryKey.Item3 && creditInfoModel.YearMonth.Month == primaryKey.Item4;

        protected override Task<IEnumerable<ICreditInfo>> SortAsync(IEnumerable<ICreditInfo> creditInfoCollection)
        {
            NullGuard.NotNull(creditInfoCollection, nameof(creditInfoCollection));

            return Task.FromResult(creditInfoCollection.OrderBy(creditInfo => creditInfo.Account.Accounting.Number).ThenBy(creditInfo => creditInfo.Account.AccountNumber).ThenByDescending(creditInfo => creditInfo.Year).ThenByDescending(creditInfo => creditInfo.Month).AsEnumerable());
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

            _accountModelCollection ??= await _accountModelHandler.ForAsync(accountingIdentificationState.AccountingIdentifier);
        }

        protected override Task PrepareReadAsync(Tuple<int, string, short, short> primaryKey, AccountingIdentificationState accountingIdentificationState)
        {
            NullGuard.NotNull(primaryKey, nameof(primaryKey))
                .NotNull(accountingIdentificationState, nameof(accountingIdentificationState));

            return PrepareReadAsync(new AccountingIdentificationState(primaryKey.Item1));
        }

        protected override Task<CreditInfoModel> OnReadAsync(CreditInfoModel creditInfoModel)
        {
            NullGuard.NotNull(creditInfoModel, nameof(creditInfoModel));

            if (creditInfoModel.Account == null && _accountModelCollection != null)
            {
                creditInfoModel.Account = _accountModelCollection.Single(accountModel => accountModel.AccountIdentifier == creditInfoModel.AccountIdentifier);
            }

            return Task.FromResult(creditInfoModel);
        }

        protected override async Task OnUpdateAsync(ICreditInfo creditInfo, CreditInfoModel creditInfoModel)
        {
            NullGuard.NotNull(creditInfo, nameof(creditInfo))
                .NotNull(creditInfoModel, nameof(creditInfoModel));

            await base.OnUpdateAsync(creditInfo, creditInfoModel);

            creditInfoModel.Credit = creditInfo.Credit;
        }

        private async Task CreateAsync(ICreditInfo creditInfo, AccountModel accountModel)
        {
            NullGuard.NotNull(creditInfo, nameof(creditInfo))
                .NotNull(accountModel, nameof(accountModel));

            CreditInfoModel creditInfoModel = ModelConverter.Convert<ICreditInfo, CreditInfoModel>(creditInfo);
            creditInfoModel.AccountIdentifier = accountModel.AccountIdentifier;
            creditInfoModel.Account = accountModel;

            EntityEntry<CreditInfoModel> creditInfoModelEntityEntry = await Entities.AddAsync(await OnCreateAsync(creditInfo, creditInfoModel));

            if (accountModel.CreditInfos.Contains(creditInfoModelEntityEntry.Entity) == false)
            {
                accountModel.CreditInfos.Add(creditInfoModelEntityEntry.Entity);
            }
        }

        private IQueryable<CreditInfoModel> CreateReader(bool includeAccount)
        {
            IQueryable<CreditInfoModel> reader = Entities
                .Include(creditInfoModel => creditInfoModel.Account)
                .Include(creditInfoModel => creditInfoModel.YearMonth);

            if (includeAccount == false || _accountModelCollection != null)
            {
                return reader;
            }

            return reader.Include(creditInfoModel => creditInfoModel.Account).ThenInclude(accountModel => accountModel.Accounting).ThenInclude(accountingModel => accountingModel.LetterHead)
                .Include(creditInfoModel => creditInfoModel.Account).ThenInclude(accountModel => accountModel.BasicAccount)
                .Include(creditInfoModel => creditInfoModel.Account).ThenInclude(accountModel => accountModel.AccountGroup);
        }

        #endregion
    }
}