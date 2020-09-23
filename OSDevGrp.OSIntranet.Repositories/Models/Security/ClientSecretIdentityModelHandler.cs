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
    internal class ClientSecretIdentityModelHandler : ModelHandlerBase<IClientSecretIdentity, RepositoryContext, ClientSecretIdentityModel, int>
    {
        #region Constructor

        public ClientSecretIdentityModelHandler(RepositoryContext dbContext, IConverter modelConverter) 
            : base(dbContext, modelConverter)
        {
        }

        #endregion

        #region Properties

        protected override DbSet<ClientSecretIdentityModel> Entities => DbContext.ClientSecretIdentities;

        protected override Func<IClientSecretIdentity, int> PrimaryKey => clientSecretIdentity => clientSecretIdentity.Identifier;

        protected override IQueryable<ClientSecretIdentityModel> Reader => Entities
            .Include(clientSecretIdentityModel => clientSecretIdentityModel.ClientSecretIdentityClaims)
            .ThenInclude(clientSecretIdentityClaimModel => clientSecretIdentityClaimModel.Claim);

        #endregion

        #region Methods

        internal async Task<IClientSecretIdentity> ReadAsync(string clientId)
        {
            NullGuard.NotNullOrWhiteSpace(clientId, nameof(clientId));

            return (await ReadAsync(clientSecretIdentityModel => clientSecretIdentityModel.ClientId == clientId)).FirstOrDefault();
        }

        protected override Expression<Func<ClientSecretIdentityModel, bool>> EntitySelector(int primaryKey) => clientSecretIdentityModel => clientSecretIdentityModel.ClientSecretIdentityIdentifier == primaryKey;

        protected override Task<IEnumerable<IClientSecretIdentity>> SortAsync(IEnumerable<IClientSecretIdentity> clientSecretIdentityCollection)
        {
            NullGuard.NotNull(clientSecretIdentityCollection, nameof(clientSecretIdentityCollection));

            return Task.FromResult(clientSecretIdentityCollection.OrderBy(clientSecretIdentity => clientSecretIdentity.FriendlyName).AsEnumerable());
        }

        protected override Task OnUpdateAsync(IClientSecretIdentity clientSecretIdentity, ClientSecretIdentityModel targetClientSecretIdentityModel)
        {
            NullGuard.NotNull(clientSecretIdentity, nameof(clientSecretIdentity))
                .NotNull(targetClientSecretIdentityModel, nameof(targetClientSecretIdentityModel));

            ClientSecretIdentityModel sourceClientSecretIdentityModel = ModelConverter.Convert<IClientSecretIdentity, ClientSecretIdentityModel>(clientSecretIdentity).With(clientSecretIdentity.ToClaimsIdentity().Claims, DbContext, ModelConverter);

            targetClientSecretIdentityModel.FriendlyName = sourceClientSecretIdentityModel.FriendlyName;
            targetClientSecretIdentityModel.ClientId = sourceClientSecretIdentityModel.ClientId;
            targetClientSecretIdentityModel.ClientSecret = sourceClientSecretIdentityModel.ClientSecret;

            ClientSecretIdentityClaimModel targetClientSecretIdentityClaimModel;
            foreach (ClientSecretIdentityClaimModel sourceClientSecretIdentityClaimModel in sourceClientSecretIdentityModel.ClientSecretIdentityClaims)
            {
                targetClientSecretIdentityClaimModel = targetClientSecretIdentityModel.ClientSecretIdentityClaims.SingleOrDefault(claim => sourceClientSecretIdentityClaimModel.ClaimIdentifier == claim.ClaimIdentifier);
                if (targetClientSecretIdentityClaimModel == null)
                {
                    targetClientSecretIdentityModel.ClientSecretIdentityClaims.Add(sourceClientSecretIdentityClaimModel);
                    continue;
                }

                targetClientSecretIdentityClaimModel.ClaimValue = sourceClientSecretIdentityClaimModel.ClaimValue ?? sourceClientSecretIdentityClaimModel.Claim.ClaimValue;
            }

            targetClientSecretIdentityClaimModel = targetClientSecretIdentityModel.ClientSecretIdentityClaims.FirstOrDefault(claim => sourceClientSecretIdentityModel.ClientSecretIdentityClaims.Any(c => claim.ClaimIdentifier == c.ClaimIdentifier) == false);
            while (targetClientSecretIdentityClaimModel != null)
            {
                targetClientSecretIdentityModel.ClientSecretIdentityClaims.Remove(targetClientSecretIdentityClaimModel);
                targetClientSecretIdentityClaimModel = targetClientSecretIdentityModel.ClientSecretIdentityClaims.FirstOrDefault(claim => sourceClientSecretIdentityModel.ClientSecretIdentityClaims.Any(c => claim.ClaimIdentifier == c.ClaimIdentifier) == false);
            }

            return Task.CompletedTask;
        }

        protected override Task<ClientSecretIdentityModel> OnDeleteAsync(ClientSecretIdentityModel clientSecretIdentityModel)
        {
            NullGuard.NotNull(clientSecretIdentityModel, nameof(clientSecretIdentityModel));

            ClientSecretIdentityClaimModel clientSecretIdentityClaimModel = clientSecretIdentityModel.ClientSecretIdentityClaims.FirstOrDefault();
            while (clientSecretIdentityClaimModel != null)
            {
                clientSecretIdentityModel.ClientSecretIdentityClaims.Remove(clientSecretIdentityClaimModel);
                clientSecretIdentityClaimModel = clientSecretIdentityModel.ClientSecretIdentityClaims.FirstOrDefault();
            }

            return Task.FromResult(clientSecretIdentityModel);
        }

        protected override Task<IClientSecretIdentity> OnReloadAsync(IClientSecretIdentity clientSecretIdentity, ClientSecretIdentityModel clientSecretIdentityModel)
        {
            NullGuard.NotNull(clientSecretIdentity, nameof(clientSecretIdentity))
                .NotNull(clientSecretIdentityModel, nameof(clientSecretIdentityModel));

            return ReadAsync(clientSecretIdentity.ClientId);
        }

        #endregion
    }
}