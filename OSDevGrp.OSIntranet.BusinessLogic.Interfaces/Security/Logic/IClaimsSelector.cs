using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic
{
    public interface IClaimsSelector
    {
        IReadOnlyCollection<Claim> Select(IReadOnlyDictionary<string, IScope> supportedScopes, IEnumerable<string> scopes, IEnumerable<Claim> claims);
    }
}