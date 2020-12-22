using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class CreditInfoModelHandler : InfoModelHandlerBase<ICreditInfo, CreditInfoModel>
    {
        #region Constructor

        public CreditInfoModelHandler(RepositoryContext dbContext, IConverter modelConverter) 
            : base(dbContext, modelConverter)
        {
        }

        #endregion

        #region Properties

        protected override DbSet<CreditInfoModel> Entities => DbContext.CreditInfos;

        protected override Func<ICreditInfo, Tuple<int, string, short, short>> PrimaryKey => creditInfo => new Tuple<int, string, short, short>(creditInfo.Account.Accounting.Number, creditInfo.Account.AccountNumber, creditInfo.Year, creditInfo.Month);

        protected override IQueryable<CreditInfoModel> Reader => MinimalReader
            .Include(creditInfoModel => creditInfoModel.Account).ThenInclude(accountModel => accountModel.Accounting).ThenInclude(accountingModel => accountingModel.LetterHead)
            .Include(creditInfoModel => creditInfoModel.Account).ThenInclude(accountModel => accountModel.BasicAccount)
            .Include(creditInfoModel => creditInfoModel.Account).ThenInclude(accountModel => accountModel.AccountGroup);

        private IQueryable<CreditInfoModel> MinimalReader => Entities.Include(creditInfoModel => creditInfoModel.YearMonth);

        #endregion

        #region Methods

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

        internal Task<IEnumerable<CreditInfoModel>> ForAsync(AccountingModel accountingModel)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel));

            return Task.FromResult<IEnumerable<CreditInfoModel>>(MinimalReader.Include(creditInfoModel => creditInfoModel.Account)
                .Where(creditInfoModel => creditInfoModel.Account.AccountingIdentifier == accountingModel.AccountingIdentifier)
                .ToArray());
        }

        protected override Expression<Func<CreditInfoModel, bool>> EntitySelector(Tuple<int, string, short, short> primaryKey) => creditInfoModel => creditInfoModel.Account.Accounting.AccountingIdentifier == primaryKey.Item1 && creditInfoModel.Account.AccountNumber == primaryKey.Item2 && creditInfoModel.YearMonth.Year == primaryKey.Item3 && creditInfoModel.YearMonth.Month == primaryKey.Item4;

        protected override Task<IEnumerable<ICreditInfo>> SortAsync(IEnumerable<ICreditInfo> creditInfoCollection)
        {
            NullGuard.NotNull(creditInfoCollection, nameof(creditInfoCollection));

            return Task.FromResult(creditInfoCollection.OrderBy(creditInfo => creditInfo.Account.Accounting.Number).ThenBy(creditInfo => creditInfo.Account.AccountNumber).ThenByDescending(creditInfo => creditInfo.Year).ThenByDescending(creditInfo => creditInfo.Month).AsEnumerable());
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

            EntityEntry<CreditInfoModel> creditInfoModelEntityEntry = await DbContext.CreditInfos.AddAsync(await OnCreateAsync(creditInfo, creditInfoModel));

            if (accountModel.CreditInfos.Contains(creditInfoModelEntityEntry.Entity) == false)
            {
                accountModel.CreditInfos.Add(creditInfoModelEntityEntry.Entity);
            }
        }

        #endregion
    }
}