using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Converters.Extensions;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class BudgetAccountModelHandler : AccountModelHandlerBase<IBudgetAccount, BudgetAccountModel>
    {
        #region Private variables

        private readonly bool _includeBudgetInformation;
        private readonly BudgetInfoModelHandler _budgetInfoModelHandler;

        #endregion

        #region Constructor

        public BudgetAccountModelHandler(RepositoryContext dbContext, IConverter modelConverter, DateTime statusDate, bool includeBudgetInformation, bool includePostingLines) 
            : base(dbContext, modelConverter, statusDate, includePostingLines)
        {
            _includeBudgetInformation = includeBudgetInformation;
            _budgetInfoModelHandler = new BudgetInfoModelHandler(dbContext, modelConverter);
        }

        #endregion

        #region Properties

        protected override DbSet<BudgetAccountModel> Entities => DbContext.BudgetAccounts;

        protected override IQueryable<BudgetAccountModel> Reader => CreateReader(_includeBudgetInformation, IncludePostingLines);

        protected override IQueryable<BudgetAccountModel> UpdateReader => CreateReader(true, false);

        protected override IQueryable<BudgetAccountModel> DeleteReader => CreateReader(true, true);

        #endregion

        #region Methods

        protected override void OnDispose()
        {
            _budgetInfoModelHandler.Dispose();
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

        protected override async Task<BudgetAccountModel> OnReadAsync(BudgetAccountModel budgetAccountModel)
        {
            NullGuard.NotNull(budgetAccountModel, nameof(budgetAccountModel));

            if (budgetAccountModel.BudgetInfos == null)
            {
                return await base.OnReadAsync(budgetAccountModel);
            }

            budgetAccountModel.BudgetInfos = (await _budgetInfoModelHandler.ReadAsync(budgetAccountModel.BudgetInfos)).ToList();

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

            // TODO: Validate the existence of posting lines.

            if (budgetAccountModel.BudgetInfos == null)
            {
                return false;
            }

            return await _budgetInfoModelHandler.IsDeletable(budgetAccountModel.BudgetInfos);
        }

        protected override async Task<BudgetAccountModel> OnDeleteAsync(BudgetAccountModel budgetAccountModel)
        {
            NullGuard.NotNull(budgetAccountModel, nameof(budgetAccountModel));

            // TODO: Delete all posting lines.
            await _budgetInfoModelHandler.DeleteAsync(budgetAccountModel.BudgetInfos);

            return await base.OnDeleteAsync(budgetAccountModel);
        }

        private IQueryable<BudgetAccountModel> CreateReader(bool includeBudgetInformation, bool includePostingLines)
        {
            IQueryable<BudgetAccountModel> reader = Entities
                .Include(budgetAccountModel => budgetAccountModel.Accounting).ThenInclude(accountingModel => accountingModel.LetterHead)
                .Include(budgetAccountModel => budgetAccountModel.BasicAccount)
                .Include(budgetAccountModel => budgetAccountModel.BudgetAccountGroup);

            if (includeBudgetInformation)
            {
                reader = reader.Include(budgetAccountModel => budgetAccountModel.BudgetInfos).ThenInclude(budgetInfoModel => budgetInfoModel.YearMonth);
            }

            if (includePostingLines)
            {
                // TODO: Include posting lines.
            }

            return reader;
        }

        #endregion
    }
}