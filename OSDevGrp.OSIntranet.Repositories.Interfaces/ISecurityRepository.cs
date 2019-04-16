using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
    public interface ISecurityRepository : IRepository
    {
        Task<IEnumerable<IUserIdentity>> GetUserIdentitiesAsync();

        Task<IUserIdentity> GetUserIdentityAsync(int userIdentityIdentifier);

        Task<IUserIdentity> GetUserIdentityAsync(string externalUserIdentifier);

        Task<IEnumerable<IClientSecretIdentity>> GetClientSecretIdentitiesAsync();

        Task<IClientSecretIdentity> GetClientSecretIdentityAsync(int clientSecretIdentityIdentifier);

        Task<IClientSecretIdentity> GetClientSecretIdentityAsync(string clientId);

        Task<IEnumerable<Claim>> GetClaimsAsync();
    }
}
