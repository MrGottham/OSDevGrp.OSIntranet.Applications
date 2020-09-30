using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class BudgetAccountModelHandler : AccountModelHandlerBase<IBudgetAccount, BudgetAccountModel>
    {
        #region Private variables

        private readonly bool _includeBudgetInformation;

        #endregion

        #region Constructor

        public BudgetAccountModelHandler(RepositoryContext dbContext, IConverter modelConverter, DateTime statusDate, bool includeBudgetInformation, bool includePostingLines) 
            : base(dbContext, modelConverter, statusDate, includePostingLines)
        {
            _includeBudgetInformation = includeBudgetInformation;
        }

        #endregion

        #region Properties

        protected override DbSet<BudgetAccountModel> Entities => DbContext.BudgetAccounts;

        protected override IQueryable<BudgetAccountModel> Reader => Entities
            .Include(accountModel => accountModel.Accounting).ThenInclude(accountingModel => accountingModel.LetterHead)
            .Include(accountModel => accountModel.BasicAccount)
            .Include(accountModel => accountModel.BudgetAccountGroup);

        protected override IQueryable<BudgetAccountModel> UpdateReader => Reader; // TODO: Include budget information.

        protected override IQueryable<BudgetAccountModel> DeleteReader => Reader; // TODO: Include budget information and posting lines.

        #endregion

        #region Methods

        protected override async Task<BudgetAccountModel> OnCreateAsync(IBudgetAccount budgetAccount, BudgetAccountModel budgetAccountModel)
        {
            NullGuard.NotNull(budgetAccount, nameof(budgetAccount))
                .NotNull(budgetAccountModel, nameof(budgetAccountModel));

            budgetAccountModel = await base.OnCreateAsync(budgetAccount, budgetAccountModel);
            budgetAccountModel.BudgetAccountGroup = await DbContext.BudgetAccountGroups.SingleAsync(budgetAccountGroupModel => budgetAccountGroupModel.BudgetAccountGroupIdentifier == budgetAccount.BudgetAccountGroup.Number);

            // TODO: Create budget information.

            return budgetAccountModel;
        }

        protected override async Task<BudgetAccountModel> OnReadAsync(BudgetAccountModel budgetAccountModel)
        {
            NullGuard.NotNull(budgetAccountModel, nameof(budgetAccountModel));

            budgetAccountModel = await base.OnReadAsync(budgetAccountModel);

            if (_includeBudgetInformation)
            {
                // TODO: Read all budget information for the given status date.
            }

            if (IncludePostingLines)
            {
                // TODO: Include all posting lines for the given status date.
            }

            return budgetAccountModel;
        }

        protected override async Task OnUpdateAsync(IBudgetAccount budgetAccount, BudgetAccountModel budgetAccountModel)
        {
            NullGuard.NotNull(budgetAccount, nameof(budgetAccount))
                .NotNull(budgetAccountModel, nameof(budgetAccountModel));

            await base.OnUpdateAsync(budgetAccount, budgetAccountModel);

            budgetAccountModel.BudgetAccountGroupIdentifier = budgetAccount.BudgetAccountGroup.Number;
            budgetAccountModel.BudgetAccountGroup = await DbContext.BudgetAccountGroups.SingleAsync(budgetAccountGroupModel => budgetAccountGroupModel.BudgetAccountGroupIdentifier == budgetAccount.BudgetAccountGroup.Number);

            // TODO: Create budget information.
        }

        protected override async Task<BudgetAccountModel> OnDeleteAsync(BudgetAccountModel budgetAccountModel)
        {
            NullGuard.NotNull(budgetAccountModel, nameof(budgetAccountModel));

            budgetAccountModel = await base.OnDeleteAsync(budgetAccountModel);

            // TODO: Delete all budget information.
            // TODO: Delete all posting lines.

            return budgetAccountModel;
        }

        protected override Task<bool> CanDeleteAsync(BudgetAccountModel budgetAccountModel)
        {
            NullGuard.NotNull(budgetAccountModel, nameof(budgetAccountModel));

            // TODO: Validate the existence of budget information.
            // TODO: Validate the existence of posting lines.

            return Task.FromResult(false);
        }

        #endregion
    }
}