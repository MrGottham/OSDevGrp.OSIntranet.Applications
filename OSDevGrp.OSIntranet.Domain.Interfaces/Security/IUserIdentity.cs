using System.Security.Claims;
using System.Security.Principal;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IUserIdentity : IIdentity
    {
        int Identifier { get; }

        string ExternalUserIdentifier { get; }

        ClaimsIdentity ToClaimsIdentity();
    }
}
