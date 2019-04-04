using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IUserIdentity : IIdentity
    {
        int Identifier { get; }

        string ExternalUserIdentifier { get; }

        void AddClaims(IEnumerable<Claim> claims);

        ClaimsIdentity ToClaimsIdentity();

        void ClearSensitiveData();
    }
}
