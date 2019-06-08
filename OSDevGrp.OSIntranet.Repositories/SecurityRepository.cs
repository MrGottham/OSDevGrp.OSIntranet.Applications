using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
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

        public SecurityRepository(IConfiguration configuration, IPrincipalResolver principalResolver)
            : base(configuration, principalResolver)
        {
        }

        #endregion

        #region Methods

        public Task<IEnumerable<IUserIdentity>> GetUserIdentitiesAsync()
        {
            return Task.Run(() => GetUserIdentities());
        }

        public Task<IUserIdentity> GetUserIdentityAsync(int userIdentityIdentifier)
        {
            return Task.Run(() => GetUserIdentity(userIdentityIdentifier));
        }

        public Task<IUserIdentity> GetUserIdentityAsync(string externalUserIdentifier)
        {
            NullGuard.NotNullOrWhiteSpace(externalUserIdentifier, nameof(externalUserIdentifier));

            return Task.Run(() => GetUserIdentity(externalUserIdentifier));
        }

        public Task<IUserIdentity> CreateUserIdentityAsync(IUserIdentity userIdentity)
        {
            NullGuard.NotNull(userIdentity, nameof(userIdentity));

            return Task.Run(() => CreateUserIdentity(userIdentity));
        }

        public Task<IUserIdentity> UpdateUserIdentityAsync(IUserIdentity userIdentity)
        {
            NullGuard.NotNull(userIdentity, nameof(userIdentity));

            return Task.Run(() => UpdateUserIdentity(userIdentity));
        }

        public Task<IUserIdentity> DeleteUserIdentityAsync(int userIdentityIdentifier)
        {
            return Task.Run(() => DeleteUserIdentity(userIdentityIdentifier));
        }

        public Task<IEnumerable<IClientSecretIdentity>> GetClientSecretIdentitiesAsync()
        {
            return Task.Run(( )=> GetClientSecretIdentities());
        }

        public Task<IClientSecretIdentity> GetClientSecretIdentityAsync(int clientSecretIdentityIdentifier)
        {
            return Task.Run(() => GetClientSecretIdentity(clientSecretIdentityIdentifier));
        }

        public Task<IClientSecretIdentity> GetClientSecretIdentityAsync(string clientId)
        {
            NullGuard.NotNullOrWhiteSpace(clientId, nameof(clientId));

            return Task.Run(() => GetClientSecretIdentity(clientId));
        }

        public Task<IClientSecretIdentity> CreateClientSecretIdentityAsync(IClientSecretIdentity clientSecretIdentity)
        {
            NullGuard.NotNull(clientSecretIdentity, nameof(clientSecretIdentity));

            return Task.Run(() => CreateClientSecretIdentity(clientSecretIdentity));
        }

        public Task<IClientSecretIdentity> UpdateClientSecretIdentityAsync(IClientSecretIdentity clientSecretIdentity)
        {
            NullGuard.NotNull(clientSecretIdentity, nameof(clientSecretIdentity));

            return Task.Run(() => UpdateClientSecretIdentity(clientSecretIdentity));
        }

        public Task<IClientSecretIdentity> DeleteClientSecretIdentityAsync(int clientSecretIdentityIdentifier)
        {
            return Task.Run(() => DeleteClientSecretIdentity(clientSecretIdentityIdentifier));
        }

        public Task<IEnumerable<Claim>> GetClaimsAsync()
        {
            return Task.Run(() => GetClaims());
        }

        private IEnumerable<IUserIdentity> GetUserIdentities()
        {
            return Execute(() =>
                {
                    using (SecurityContext context = new SecurityContext(Configuration, PrincipalResolver))
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

        private IUserIdentity GetUserIdentity(int userIdentityIdentifier)
        {
            return Execute(() =>
                {
                    using (SecurityContext context = new SecurityContext(Configuration, PrincipalResolver))
                    {
                        UserIdentityModel userIdentityModel = context.UserIdentities
                            .Include(model => model.UserIdentityClaims).ThenInclude(e => e.Claim)
                            .SingleOrDefault(model => model.UserIdentityIdentifier == userIdentityIdentifier);
                        if (userIdentityModel == null)
                        {
                            return null;
                        }

                        return _securityModelConverter.Convert<UserIdentityModel, IUserIdentity>(userIdentityModel);
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private IUserIdentity GetUserIdentity(string externalUserIdentifier)
        {
            NullGuard.NotNullOrWhiteSpace(externalUserIdentifier, nameof(externalUserIdentifier));

            return Execute(() =>
                {
                    using (SecurityContext context = new SecurityContext(Configuration, PrincipalResolver))
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

        private IUserIdentity CreateUserIdentity(IUserIdentity userIdentity)
        {
            NullGuard.NotNull(userIdentity, nameof(userIdentity));

            return Execute(() =>
                {
                    using (SecurityContext context = new SecurityContext(Configuration, PrincipalResolver))
                    {
                        UserIdentityModel userIdentityModel = _securityModelConverter.Convert<IUserIdentity, UserIdentityModel>(userIdentity).WithDefaultIdentifier().With(userIdentity.ToClaimsIdentity().Claims, context, _securityModelConverter);

                        context.UserIdentities.Add(userIdentityModel);

                        context.SaveChanges();

                        return GetUserIdentity(userIdentity.ExternalUserIdentifier);
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private IUserIdentity UpdateUserIdentity(IUserIdentity userIdentity)
        {
            NullGuard.NotNull(userIdentity, nameof(userIdentity));

            return Execute(() =>
                {
                    using (SecurityContext context = new SecurityContext(Configuration, PrincipalResolver))
                    {
                        UserIdentityModel sourceUserIdentityModel = _securityModelConverter.Convert<IUserIdentity, UserIdentityModel>(userIdentity).With(userIdentity.ToClaimsIdentity().Claims, context, _securityModelConverter);

                        UserIdentityModel targetUserIdentityModel = context.UserIdentities
                            .Include(model => model.UserIdentityClaims).ThenInclude(e => e.Claim)
                            .SingleOrDefault(model => model.UserIdentityIdentifier == sourceUserIdentityModel.UserIdentityIdentifier);
                        if (targetUserIdentityModel == null)
                        {
                            return null;
                        }

                        targetUserIdentityModel.ExternalUserIdentifier = sourceUserIdentityModel.ExternalUserIdentifier;

                        UserIdentityClaimModel targetUserIdentityClaimModel;
                        foreach(UserIdentityClaimModel sourceUserIdentityClaimModel in sourceUserIdentityModel.UserIdentityClaims)
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

                        context.SaveChanges();

                        return GetUserIdentity(targetUserIdentityModel.UserIdentityIdentifier);
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private IUserIdentity DeleteUserIdentity(int userIdentityIdentifier)
        {
            return Execute(() =>
                {
                    using (SecurityContext context = new SecurityContext(Configuration, PrincipalResolver))
                    {
                        UserIdentityModel userIdentityModel = context.UserIdentities.Find(userIdentityIdentifier);
                        if (userIdentityModel == null)
                        {
                            return null;
                        }

                        context.UserIdentities.Remove(userIdentityModel);

                        context.SaveChanges();
                        
                        return (IUserIdentity) null;
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private IEnumerable<IClientSecretIdentity> GetClientSecretIdentities()
        {
            return Execute(() =>
                {
                    using (SecurityContext context = new SecurityContext(Configuration, PrincipalResolver))
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

        private IClientSecretIdentity GetClientSecretIdentity(int clientSecretIdentityIdentifier)
        {
            return Execute(() =>
                {
                    using (SecurityContext context = new SecurityContext(Configuration, PrincipalResolver))
                    {
                        ClientSecretIdentityModel clientSecretIdentityModel= context.ClientSecretIdentities
                            .Include(model=> model.ClientSecretIdentityClaims).ThenInclude(e => e.Claim)
                            .SingleOrDefault(model => model.ClientSecretIdentityIdentifier == clientSecretIdentityIdentifier);
                        if (clientSecretIdentityModel == null)
                        {
                            return null;
                        }

                        return _securityModelConverter.Convert<ClientSecretIdentityModel, IClientSecretIdentity>(clientSecretIdentityModel);
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private IClientSecretIdentity GetClientSecretIdentity(string clientId)
        {
            NullGuard.NotNullOrWhiteSpace(clientId, nameof(clientId));

            return Execute(() =>
                {
                    using (SecurityContext context = new SecurityContext(Configuration, PrincipalResolver))
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

        private IClientSecretIdentity CreateClientSecretIdentity(IClientSecretIdentity clientSecretIdentity)
        {
            NullGuard.NotNull(clientSecretIdentity, nameof(clientSecretIdentity));

            return Execute(() =>
                {
                    using (SecurityContext context = new SecurityContext(Configuration, PrincipalResolver))
                    {
                        ClientSecretIdentityModel clientSecretIdentityModel = _securityModelConverter.Convert<IClientSecretIdentity, ClientSecretIdentityModel>(clientSecretIdentity).WithDefaultIdentifier().With(clientSecretIdentity.ToClaimsIdentity().Claims, context, _securityModelConverter);

                        context.ClientSecretIdentities.Add(clientSecretIdentityModel);

                        context.SaveChanges();

                        return GetClientSecretIdentity(clientSecretIdentity.ClientId);
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private IClientSecretIdentity UpdateClientSecretIdentity(IClientSecretIdentity clientSecretIdentity)
        {
            NullGuard.NotNull(clientSecretIdentity, nameof(clientSecretIdentity));

            return Execute(() =>
                {
                    using (SecurityContext context = new SecurityContext(Configuration, PrincipalResolver))
                    {
                        ClientSecretIdentityModel sourceClientSecretIdentityModel = _securityModelConverter.Convert<IClientSecretIdentity, ClientSecretIdentityModel>(clientSecretIdentity).With(clientSecretIdentity.ToClaimsIdentity().Claims, context, _securityModelConverter);

                        ClientSecretIdentityModel targetClientSecretIdentityModel = context.ClientSecretIdentities
                            .Include(model=> model.ClientSecretIdentityClaims).ThenInclude(e => e.Claim)
                            .SingleOrDefault(model => model.ClientSecretIdentityIdentifier == sourceClientSecretIdentityModel.ClientSecretIdentityIdentifier);
                        if (targetClientSecretIdentityModel == null)
                        {
                            return null;
                        }

                        targetClientSecretIdentityModel.FriendlyName = sourceClientSecretIdentityModel.FriendlyName;
                        targetClientSecretIdentityModel.ClientId = sourceClientSecretIdentityModel.ClientId;
                        targetClientSecretIdentityModel.ClientSecret = sourceClientSecretIdentityModel.ClientSecret;

                        ClientSecretIdentityClaimModel targetClientSecretIdentityClaimModel;
                        foreach(ClientSecretIdentityClaimModel sourceClientSecretIdentityClaimModel in sourceClientSecretIdentityModel.ClientSecretIdentityClaims)
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

                        context.SaveChanges();

                        return GetClientSecretIdentity(targetClientSecretIdentityModel.ClientSecretIdentityIdentifier);
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private IClientSecretIdentity DeleteClientSecretIdentity(int clientSecretIdentityIdentifier)
        {
            return Execute(() =>
                {
                    using (SecurityContext context = new SecurityContext(Configuration, PrincipalResolver))
                    {
                        ClientSecretIdentityModel clientSecretIdentityModel = context.ClientSecretIdentities.Find(clientSecretIdentityIdentifier);
                        if (clientSecretIdentityModel == null)
                        {
                            return null;
                        }

                        context.ClientSecretIdentities.Remove(clientSecretIdentityModel);

                        context.SaveChanges();
                        
                        return (IClientSecretIdentity) null;
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private IEnumerable<Claim> GetClaims()
        {
            return Execute(() =>
                {
                    using (SecurityContext context = new SecurityContext(Configuration, PrincipalResolver))
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
