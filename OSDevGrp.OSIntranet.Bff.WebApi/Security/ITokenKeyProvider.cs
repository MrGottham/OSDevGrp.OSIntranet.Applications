using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Security;

public interface ITokenKeyProvider
{
    Task<string> ResolveAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default);
}