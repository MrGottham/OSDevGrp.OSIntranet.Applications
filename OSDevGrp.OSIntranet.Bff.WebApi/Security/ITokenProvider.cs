using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Security;

public interface ITokenProvider
{
    Task<IToken> ResolveAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default);
}