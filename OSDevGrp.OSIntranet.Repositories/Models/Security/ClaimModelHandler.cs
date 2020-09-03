using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Security
{
    internal class ClaimModelHandler : ModelHandlerBase<Claim, SecurityContext, ClaimModel, string>
    {
        #region Constructor

        public ClaimModelHandler(SecurityContext dbContext, IConverter modelConverter) 
            : base(dbContext, modelConverter)
        {
        }

        #endregion

        #region Properties

        protected override DbSet<ClaimModel> Entities => DbContext.Claims;

        protected override Func<Claim, string> PrimaryKey => claim => claim.Type;

        #endregion

        #region Methods

        protected override Expression<Func<ClaimModel, bool>> EntitySelector(string primaryKey) => claimModel => claimModel.ClaimType == primaryKey;

        protected override Task<IEnumerable<Claim>> SortAsync(IEnumerable<Claim> claimCollection)
        {
            NullGuard.NotNull(claimCollection, nameof(claimCollection));

            return Task.FromResult(claimCollection.OrderBy(claim => claim.Type).AsEnumerable());
        }

        protected override Task<ClaimModel> OnCreateAsync(Claim claim, ClaimModel claimModel) => throw new NotSupportedException();

        protected override Task OnUpdateAsync(Claim claim, ClaimModel claimModel) => throw new NotSupportedException();

        protected override Task<bool> CanDeleteAsync(ClaimModel entityModel)
        {
            NullGuard.NotNull(entityModel, nameof(entityModel));

            return Task.FromResult(false);
        }

        protected override Task<ClaimModel> OnDeleteAsync(ClaimModel claimModel) => throw new NotSupportedException();

        #endregion
    }
}