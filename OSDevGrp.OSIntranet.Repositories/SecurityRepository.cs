using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Converters;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Models.Security;

namespace OSDevGrp.OSIntranet.Repositories
{
    public class SecurityRepository : RepositoryBase, ISecurityRepository
    {
        #region Private variables

        private readonly IConverter _securityModelConverter = new SecurityModelConverter();

        #endregion

        #region Constructor

        public SecurityRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        #endregion

        #region Methods

        public Task<IEnumerable<IUserIdentity>> GetUserIdentitiesAsync()
        {
            return Task.Run(() => GetUserIdentities());
        }

        public Task<IUserIdentity> GetUserIdentityAsync(string externalUserIdentifier)
        {
            NullGuard.NotNullOrWhiteSpace(externalUserIdentifier, nameof(externalUserIdentifier));

            return Task.Run(() => GetUserIdentity(externalUserIdentifier));
        }

        public Task<IEnumerable<IClientSecretIdentity>> GetClientSecretIdentitiesAsync()
        {
            return Task.Run(( )=> GetClientSecretIdentities());
        }

        public Task<IClientSecretIdentity> GetClientSecretIdentityAsync(string clientId)
        {
            NullGuard.NotNullOrWhiteSpace(clientId, nameof(clientId));

            return Task.Run(() => GetClientSecretIdentity(clientId));
        }

        public Task<IEnumerable<Claim>> GetClaimsAsync()
        {
            return Task.Run(() => GetClaims());
        }

        private IEnumerable<IUserIdentity> GetUserIdentities()
        {
            return Execute(() =>
                {
                    using (SecurityContext context = new SecurityContext(Configuration))
                    {
                        return context.UserIdentities
                            .Include(userIdentityModel => userIdentityModel.UserIdentityClaims).ThenInclude(e => e.Claim)
                            .AsParallel()
                            .Select(userIdentityModel => _securityModelConverter.Convert<UserIdentityModel, IUserIdentity>(userIdentityModel))
                            .OrderBy(userIdentity => userIdentity.ExternalUserIdentifier)
                            .ToList();
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private IUserIdentity GetUserIdentity(string externalUserIdentifier)
        {
            NullGuard.NotNullOrWhiteSpace(externalUserIdentifier, nameof(externalUserIdentifier));

            return Execute(() =>
                {
                    using (SecurityContext context = new SecurityContext(Configuration))
                    {
                        UserIdentityModel userIdentityModel = context.UserIdentities
                            .Include(model => model.UserIdentityClaims).ThenInclude(e => e.Claim)
                            .SingleOrDefault(model => model.ExternalUserIdentifier == externalUserIdentifier);
                        if (userIdentityModel == null)
                        {
                            return null;
                        }

                        return _securityModelConverter.Convert<UserIdentityModel, IUserIdentity>(userIdentityModel);
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private IEnumerable<IClientSecretIdentity> GetClientSecretIdentities()
        {
            return Execute(() =>
                {
                    using (SecurityContext context = new SecurityContext(Configuration))
                    {
                        return context.ClientSecretIdentities
                            .Include(clientSecretIdentityModel => clientSecretIdentityModel.ClientSecretIdentityClaims).ThenInclude(e => e.Claim)
                            .AsParallel()
                            .Select(clientSecretIdentityModel => _securityModelConverter.Convert<ClientSecretIdentityModel, IClientSecretIdentity>(clientSecretIdentityModel))
                            .OrderBy(clientSecretIdentity => clientSecretIdentity.FriendlyName)
                            .ToList();
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private IClientSecretIdentity GetClientSecretIdentity(string clientId)
        {
            NullGuard.NotNullOrWhiteSpace(clientId, nameof(clientId));

            return Execute(() =>
                {
                    using (SecurityContext context = new SecurityContext(Configuration))
                    {
                        ClientSecretIdentityModel clientSecretIdentityModel= context.ClientSecretIdentities
                            .Include(model=> model.ClientSecretIdentityClaims).ThenInclude(e => e.Claim)
                            .SingleOrDefault(model => model.ClientId == clientId);
                        if (clientSecretIdentityModel == null)
                        {
                            return null;
                        }

                        return _securityModelConverter.Convert<ClientSecretIdentityModel, IClientSecretIdentity>(clientSecretIdentityModel);
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private IEnumerable<Claim> GetClaims()
        {
            return Execute(() =>
                {
                    using (SecurityContext context = new SecurityContext(Configuration))
                    {
                        return context.Claims.AsParallel()
                            .Select(claimModel => _securityModelConverter.Convert<ClaimModel, Claim>(claimModel))
                            .OrderBy(claim => claim.Type)
                            .ToList();
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        #endregion
    }
}
