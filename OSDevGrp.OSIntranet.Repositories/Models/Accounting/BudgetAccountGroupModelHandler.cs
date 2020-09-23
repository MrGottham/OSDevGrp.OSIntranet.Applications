using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class BudgetAccountGroupModelHandler : ModelHandlerBase<IBudgetAccountGroup, RepositoryContext, BudgetAccountGroupModel, int>
    {
        #region Constructor

        public BudgetAccountGroupModelHandler(RepositoryContext dbContext, IConverter modelConverter) 
            : base(dbContext, modelConverter)
        {
        }

        #endregion

        #region Properties

        protected override DbSet<BudgetAccountGroupModel> Entities => DbContext.BudgetAccountGroups;

        protected override Func<IBudgetAccountGroup, int> PrimaryKey => budgetAccountGroup => budgetAccountGroup.Number;

        #endregion

        #region Methods

        protected override Expression<Func<BudgetAccountGroupModel, bool>> EntitySelector(int primaryKey) => budgetAccountGroupModel => budgetAccountGroupModel.BudgetAccountGroupIdentifier == primaryKey;

        protected override Task<IEnumerable<IBudgetAccountGroup>> SortAsync(IEnumerable<IBudgetAccountGroup> budgetAccountGroupCollection)
        {
            NullGuard.NotNull(budgetAccountGroupCollection, nameof(budgetAccountGroupCollection));

            return Task.FromResult(budgetAccountGroupCollection.OrderBy(budgetAccountGroup => budgetAccountGroup.Number).AsEnumerable());
        }

        protected override async Task<BudgetAccountGroupModel> OnReadAsync(BudgetAccountGroupModel budgetAccountGroupModel)
        {
            NullGuard.NotNull(budgetAccountGroupModel, nameof(budgetAccountGroupModel));

            budgetAccountGroupModel.Deletable = await CanDeleteAsync(budgetAccountGroupModel);

            return budgetAccountGroupModel;
        }

        protected override Task OnUpdateAsync(IBudgetAccountGroup budgetAccountGroup, BudgetAccountGroupModel budgetAccountGroupModel)
        {
            NullGuard.NotNull(budgetAccountGroup, nameof(budgetAccountGroup))
                .NotNull(budgetAccountGroupModel, nameof(budgetAccountGroupModel));

            budgetAccountGroupModel.Name = budgetAccountGroup.Name;

            return Task.CompletedTask;
        }

        protected override async Task<bool> CanDeleteAsync(BudgetAccountGroupModel budgetAccountGroupModel)
        {
            NullGuard.NotNull(budgetAccountGroupModel, nameof(budgetAccountGroupModel));

            return await DbContext.BudgetAccounts.FirstOrDefaultAsync(budgetAccountModel => budgetAccountModel.BudgetAccountGroupIdentifier == budgetAccountGroupModel.BudgetAccountGroupIdentifier) == null;
        }

        #endregion
    }
}