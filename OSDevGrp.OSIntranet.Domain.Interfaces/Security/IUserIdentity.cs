using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IUserIdentity : IIdentity, IAuditable
    {
        int Identifier { get; }

        string ExternalUserIdentifier { get; }

        void AddClaims(IEnumerable<Claim> claims);

        ClaimsIdentity ToClaimsIdentity();

        void ClearSensitiveData();
    }
}
