using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
    public interface ISecurityRepository : IRepository
    {
        Task<IEnumerable<IUserIdentity>> GetUserIdentitiesAsync();

        Task<IUserIdentity> GetUserIdentityAsync(string externalUserIdentifier);

        Task<IEnumerable<IClientSecretIdentity>> GetClientSecretIdentitiesAsync();

        Task<IClientSecretIdentity> GetClientSecretIdentityAsync(string clientId);
    }
}
