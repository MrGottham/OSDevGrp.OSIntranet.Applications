using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Converters;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Models.Security;

namespace OSDevGrp.OSIntranet.Repositories
{
    internal class SecurityRepository : DatabaseRepositoryBase<RepositoryContext>, ISecurityRepository
    {
        #region Constructor

        public SecurityRepository(RepositoryContext repositoryContext, IConfiguration configuration, ILoggerFactory loggerFactory)
            : base(repositoryContext, configuration, loggerFactory)
        {
        }

        #endregion

        #region Methods

        public Task<IEnumerable<IUserIdentity>> GetUserIdentitiesAsync()
        {
            return ExecuteAsync(async () =>
                {
                    using UserIdentityModelHandler userIdentityModelHandler = new UserIdentityModelHandler(DbContext, SecurityModelConverter.Create());
                    return await userIdentityModelHandler.ReadAsync();
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IUserIdentity> GetUserIdentityAsync(int userIdentityIdentifier)
        {
            return ExecuteAsync(async () =>
                {
                    using UserIdentityModelHandler userIdentityModelHandler = new UserIdentityModelHandler(DbContext, SecurityModelConverter.Create());
                    return await userIdentityModelHandler.ReadAsync(userIdentityIdentifier);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IUserIdentity> GetUserIdentityAsync(string externalUserIdentifier)
        {
            NullGuard.NotNullOrWhiteSpace(externalUserIdentifier, nameof(externalUserIdentifier));

            return ExecuteAsync(async () =>
                {
                    using UserIdentityModelHandler userIdentityModelHandler = new UserIdentityModelHandler(DbContext, SecurityModelConverter.Create());
                    return await userIdentityModelHandler.ReadAsync(externalUserIdentifier);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IUserIdentity> CreateUserIdentityAsync(IUserIdentity userIdentity)
        {
            NullGuard.NotNull(userIdentity, nameof(userIdentity));

            return ExecuteAsync(async () =>
                {
                    using UserIdentityModelHandler userIdentityModelHandler = new UserIdentityModelHandler(DbContext, SecurityModelConverter.Create());
                    return await userIdentityModelHandler.CreateAsync(userIdentity);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IUserIdentity> UpdateUserIdentityAsync(IUserIdentity userIdentity)
        {
            NullGuard.NotNull(userIdentity, nameof(userIdentity));

            return ExecuteAsync(async () =>
                {
                    using UserIdentityModelHandler userIdentityModelHandler = new UserIdentityModelHandler(DbContext, SecurityModelConverter.Create());
                    return await userIdentityModelHandler.UpdateAsync(userIdentity);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IUserIdentity> DeleteUserIdentityAsync(int userIdentityIdentifier)
        {
            return ExecuteAsync(async () =>
                {
                    using UserIdentityModelHandler userIdentityModelHandler = new UserIdentityModelHandler(DbContext, SecurityModelConverter.Create());
                    return await userIdentityModelHandler.DeleteAsync(userIdentityIdentifier);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IEnumerable<IClientSecretIdentity>> GetClientSecretIdentitiesAsync()
        {
            return ExecuteAsync(async () =>
                {
                    using ClientSecretIdentityModelHandler clientSecretIdentityModelHandler = new ClientSecretIdentityModelHandler(DbContext, SecurityModelConverter.Create());
                    return await clientSecretIdentityModelHandler.ReadAsync();
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IClientSecretIdentity> GetClientSecretIdentityAsync(int clientSecretIdentityIdentifier)
        {
            return ExecuteAsync(async () =>
                {
                    using ClientSecretIdentityModelHandler clientSecretIdentityModelHandler = new ClientSecretIdentityModelHandler(DbContext, SecurityModelConverter.Create());
                    return await clientSecretIdentityModelHandler.ReadAsync(clientSecretIdentityIdentifier);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IClientSecretIdentity> GetClientSecretIdentityAsync(string clientId)
        {
            NullGuard.NotNullOrWhiteSpace(clientId, nameof(clientId));

            return ExecuteAsync(async () =>
                {
                    using ClientSecretIdentityModelHandler clientSecretIdentityModelHandler = new ClientSecretIdentityModelHandler(DbContext, SecurityModelConverter.Create());
                    return await clientSecretIdentityModelHandler.ReadAsync(clientId);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IClientSecretIdentity> CreateClientSecretIdentityAsync(IClientSecretIdentity clientSecretIdentity)
        {
            NullGuard.NotNull(clientSecretIdentity, nameof(clientSecretIdentity));

            return ExecuteAsync(async () =>
                {
                    using ClientSecretIdentityModelHandler clientSecretIdentityModelHandler = new ClientSecretIdentityModelHandler(DbContext, SecurityModelConverter.Create());
                    return await clientSecretIdentityModelHandler.CreateAsync(clientSecretIdentity);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IClientSecretIdentity> UpdateClientSecretIdentityAsync(IClientSecretIdentity clientSecretIdentity)
        {
            NullGuard.NotNull(clientSecretIdentity, nameof(clientSecretIdentity));

            return ExecuteAsync(async () =>
                {
                    using ClientSecretIdentityModelHandler clientSecretIdentityModelHandler = new ClientSecretIdentityModelHandler(DbContext, SecurityModelConverter.Create());
                    return await clientSecretIdentityModelHandler.UpdateAsync(clientSecretIdentity);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IClientSecretIdentity> DeleteClientSecretIdentityAsync(int clientSecretIdentityIdentifier)
        {
            return ExecuteAsync(async () =>
                {
                    using ClientSecretIdentityModelHandler clientSecretIdentityModelHandler = new ClientSecretIdentityModelHandler(DbContext, SecurityModelConverter.Create());
                    return await clientSecretIdentityModelHandler.DeleteAsync(clientSecretIdentityIdentifier);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IEnumerable<Claim>> GetClaimsAsync()
        {
            return ExecuteAsync(async () =>
                {
                    using ClaimModelHandler claimModelHandler = new ClaimModelHandler(DbContext, SecurityModelConverter.Create());
                    return await claimModelHandler.ReadAsync();
                },
                MethodBase.GetCurrentMethod());
        }

        #endregion
    }
}