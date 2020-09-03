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
    internal class AccountGroupModelHandler : ModelHandlerBase<IAccountGroup, AccountingContext, AccountGroupModel, int>
    {
        #region Constructor

        public AccountGroupModelHandler(AccountingContext dbContext, IConverter modelConverter) 
            : base(dbContext, modelConverter)
        {
        }

        #endregion

        #region Properties

        protected override DbSet<AccountGroupModel> Entities => DbContext.AccountGroups;

        protected override Func<IAccountGroup, int> PrimaryKey => accountGroup => accountGroup.Number;

        #endregion

        #region Methods

        protected override Expression<Func<AccountGroupModel, bool>> EntitySelector(int primaryKey) => accountGroupModel => accountGroupModel.AccountGroupIdentifier == primaryKey;

        protected override Task<IEnumerable<IAccountGroup>> SortAsync(IEnumerable<IAccountGroup> accountGroupCollection)
        {
            NullGuard.NotNull(accountGroupCollection, nameof(accountGroupCollection));

            return Task.FromResult(accountGroupCollection.OrderBy(accountGroup => accountGroup.Number).AsEnumerable());
        }

        protected override async Task<AccountGroupModel> OnReadAsync(AccountGroupModel accountGroupModel)
        {
            NullGuard.NotNull(accountGroupModel, nameof(accountGroupModel));

            accountGroupModel.Deletable = await CanDeleteAsync(accountGroupModel);

            return accountGroupModel;
        }

        protected override Task OnUpdateAsync(IAccountGroup accountGroup, AccountGroupModel accountGroupModel)
        {
            NullGuard.NotNull(accountGroup, nameof(accountGroup))
                .NotNull(accountGroupModel, nameof(accountGroupModel));

            accountGroupModel.Name = accountGroup.Name;
            accountGroupModel.AccountGroupType = accountGroup.AccountGroupType;

            return Task.CompletedTask;
        }

        protected override Task<bool> CanDeleteAsync(AccountGroupModel accountGroupModel)
        {
            NullGuard.NotNull(accountGroupModel, nameof(accountGroupModel));

            return Task.FromResult(false);
        }

        #endregion
    }
}