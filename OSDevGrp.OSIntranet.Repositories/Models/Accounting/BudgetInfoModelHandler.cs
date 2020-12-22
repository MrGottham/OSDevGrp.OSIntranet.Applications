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
    internal class BudgetInfoModelHandler : InfoModelHandlerBase<IBudgetInfo, BudgetInfoModel>
    {
        #region Constructor

        public BudgetInfoModelHandler(RepositoryContext dbContext, IConverter modelConverter) 
            : base(dbContext, modelConverter)
        {
        }

        #endregion

        #region Properties

        protected override DbSet<BudgetInfoModel> Entities => DbContext.BudgetInfos;

        protected override Func<IBudgetInfo, Tuple<int, string, short, short>> PrimaryKey => budgetInfo => new Tuple<int, string, short, short>(budgetInfo.BudgetAccount.Accounting.Number, budgetInfo.BudgetAccount.AccountNumber, budgetInfo.Year, budgetInfo.Month);

        protected override IQueryable<BudgetInfoModel> Reader => MinimalReader
            .Include(budgetInfoModel => budgetInfoModel.BudgetAccount).ThenInclude(budgetAccountModel => budgetAccountModel.Accounting).ThenInclude(accountingModel => accountingModel.LetterHead)
            .Include(budgetInfoModel => budgetInfoModel.BudgetAccount).ThenInclude(budgetAccountModel => budgetAccountModel.BasicAccount)
            .Include(budgetInfoModel => budgetInfoModel.BudgetAccount).ThenInclude(budgetAccountModel => budgetAccountModel.BudgetAccountGroup);

        private IQueryable<BudgetInfoModel> MinimalReader => Entities.Include(budgetInfoModel => budgetInfoModel.YearMonth);

        #endregion

        #region Methods

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

        internal Task<IEnumerable<BudgetInfoModel>> ForAsync(AccountingModel accountingModel)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel));

            return Task.FromResult<IEnumerable<BudgetInfoModel>>(MinimalReader.Include(budgetInfoModel => budgetInfoModel.BudgetAccount)
                .Where(budgetInfoModel => budgetInfoModel.BudgetAccount.AccountingIdentifier == accountingModel.AccountingIdentifier)
                .ToArray());
        }

        protected override Expression<Func<BudgetInfoModel, bool>> EntitySelector(Tuple<int, string, short, short> primaryKey) => budgetInfoModel => budgetInfoModel.BudgetAccount.Accounting.AccountingIdentifier == primaryKey.Item1 && budgetInfoModel.BudgetAccount.AccountNumber == primaryKey.Item2 && budgetInfoModel.YearMonth.Year == primaryKey.Item3 && budgetInfoModel.YearMonth.Month == primaryKey.Item4;

        protected override Task<IEnumerable<IBudgetInfo>> SortAsync(IEnumerable<IBudgetInfo> budgetInfoCollection)
        {
            NullGuard.NotNull(budgetInfoCollection, nameof(budgetInfoCollection));

            return Task.FromResult(budgetInfoCollection.OrderBy(budgetInfo => budgetInfo.BudgetAccount.Accounting.Number).ThenBy(budgetInfo => budgetInfo.BudgetAccount.AccountNumber).ThenByDescending(budgetInfo => budgetInfo.Year).ThenByDescending(budgetInfo => budgetInfo.Month).AsEnumerable());
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

            EntityEntry<BudgetInfoModel> budgetInfoModelEntityEntry = await DbContext.BudgetInfos.AddAsync(await OnCreateAsync(budgetInfo, budgetInfoModel));

            if (budgetAccountModel.BudgetInfos.Contains(budgetInfoModelEntityEntry.Entity) == false)
            {
                budgetAccountModel.BudgetInfos.Add(budgetInfoModelEntityEntry.Entity);
            }
        }

        #endregion
    }
}