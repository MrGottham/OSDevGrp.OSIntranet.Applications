using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
	public interface IClientSecretIdentity : IIdentity, IAuditable
    {
        int Identifier { get; }

        string FriendlyName { get; }

        string ClientId { get; }

        string ClientSecret { get; }

        void AddClaims(IEnumerable<Claim> claims);

        ClaimsIdentity ToClaimsIdentity();

        void ClearSensitiveData();
    }
}