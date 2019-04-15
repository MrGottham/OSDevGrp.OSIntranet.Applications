using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IClientSecretIdentity : IIdentity, IAuditable
    {
        int Identifier { get; }

        string FriendlyName { get; }

        string ClientId { get; }

        string ClientSecret { get; }

        IToken Token { get; }

        void AddClaims(IEnumerable<Claim> claims);

        ClaimsIdentity ToClaimsIdentity();

        void ClearSensitiveData();

        void AddToken(IToken token);
    }
}
