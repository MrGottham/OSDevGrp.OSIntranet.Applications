using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Security;

public interface ITokenStorage
{
    Task<IToken> GetTokenAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default);

    Task StoreTokenAsync(ClaimsPrincipal user, IToken token, CancellationToken cancellationToken = default);

    Task DeleteTokenAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default);
}