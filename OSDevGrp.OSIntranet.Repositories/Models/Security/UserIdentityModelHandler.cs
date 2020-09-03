using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Security
{
    internal class UserIdentityModelHandler : ModelHandlerBase<IUserIdentity, SecurityContext, UserIdentityModel, int>
    {
        #region Constructor

        public UserIdentityModelHandler(SecurityContext dbContext, IConverter modelConverter) 
            : base(dbContext, modelConverter)
        {
        }

        #endregion

        #region Properties

        protected override DbSet<UserIdentityModel> Entities => DbContext.UserIdentities;

        protected override Func<IUserIdentity, int> PrimaryKey => userIdentity => userIdentity.Identifier;

        protected override IQueryable<UserIdentityModel> Reader => Entities
            .Include(userIdentityModel => userIdentityModel.UserIdentityClaims)
            .ThenInclude(userIdentityClaimModel => userIdentityClaimModel.Claim);

        #endregion

        #region Methods

        internal async Task<IUserIdentity> ReadAsync(string externalUserIdentifier)
        {
            NullGuard.NotNullOrWhiteSpace(externalUserIdentifier, nameof(externalUserIdentifier));

            return (await ReadAsync(userIdentityModel => userIdentityModel.ExternalUserIdentifier == externalUserIdentifier)).FirstOrDefault();
        }

        protected override Expression<Func<UserIdentityModel, bool>> EntitySelector(int primaryKey) => userIdentityModel => userIdentityModel.UserIdentityIdentifier == primaryKey;

        protected override Task<IEnumerable<IUserIdentity>> SortAsync(IEnumerable<IUserIdentity> userIdentityCollection)
        {
            NullGuard.NotNull(userIdentityCollection, nameof(userIdentityCollection));

            return Task.FromResult(userIdentityCollection.OrderBy(userIdentity => userIdentity.ExternalUserIdentifier).AsEnumerable());
        }

        protected override Task OnUpdateAsync(IUserIdentity userIdentity, UserIdentityModel targetUserIdentityModel)
        {
            NullGuard.NotNull(userIdentity, nameof(userIdentity))
                .NotNull(targetUserIdentityModel, nameof(targetUserIdentityModel));

            UserIdentityModel sourceUserIdentityModel = ModelConverter.Convert<IUserIdentity, UserIdentityModel>(userIdentity).With(userIdentity.ToClaimsIdentity().Claims, DbContext, ModelConverter);

            targetUserIdentityModel.ExternalUserIdentifier = sourceUserIdentityModel.ExternalUserIdentifier;

            UserIdentityClaimModel targetUserIdentityClaimModel;
            foreach (UserIdentityClaimModel sourceUserIdentityClaimModel in sourceUserIdentityModel.UserIdentityClaims)
            {
                targetUserIdentityClaimModel = targetUserIdentityModel.UserIdentityClaims.SingleOrDefault(claim => sourceUserIdentityClaimModel.ClaimIdentifier == claim.ClaimIdentifier);
                if (targetUserIdentityClaimModel == null)
                {
                    targetUserIdentityModel.UserIdentityClaims.Add(sourceUserIdentityClaimModel);
                    continue;
                }

                targetUserIdentityClaimModel.ClaimValue = sourceUserIdentityClaimModel.ClaimValue ?? sourceUserIdentityClaimModel.Claim.ClaimValue;
            }

            targetUserIdentityClaimModel = targetUserIdentityModel.UserIdentityClaims.FirstOrDefault(claim => sourceUserIdentityModel.UserIdentityClaims.Any(c => claim.ClaimIdentifier == c.ClaimIdentifier) == false);
            while (targetUserIdentityClaimModel != null)
            {
                targetUserIdentityModel.UserIdentityClaims.Remove(targetUserIdentityClaimModel);
                targetUserIdentityClaimModel = targetUserIdentityModel.UserIdentityClaims.FirstOrDefault(claim => sourceUserIdentityModel.UserIdentityClaims.Any(c => claim.ClaimIdentifier == c.ClaimIdentifier) == false);
            }

            return Task.CompletedTask;
        }

        protected override Task<UserIdentityModel> OnDeleteAsync(UserIdentityModel userIdentityModel)
        {
            NullGuard.NotNull(userIdentityModel, nameof(userIdentityModel));

            UserIdentityClaimModel userIdentityClaimModel = userIdentityModel.UserIdentityClaims.FirstOrDefault();
            while (userIdentityClaimModel != null)
            {
                userIdentityModel.UserIdentityClaims.Remove(userIdentityClaimModel);
                userIdentityClaimModel = userIdentityModel.UserIdentityClaims.FirstOrDefault();
            }

            return Task.FromResult(userIdentityModel);
        }

        protected override Task<IUserIdentity> OnReloadAsync(IUserIdentity userIdentity, UserIdentityModel userIdentityModel)
        {
            NullGuard.NotNull(userIdentity, nameof(userIdentity))
                .NotNull(userIdentityModel, nameof(userIdentityModel));

            return ReadAsync(userIdentity.ExternalUserIdentifier);
        }

        #endregion
    }
}