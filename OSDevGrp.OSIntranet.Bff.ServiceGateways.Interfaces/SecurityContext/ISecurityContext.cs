using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

public interface ISecurityContext
{
    ClaimsPrincipal User { get; }

    IToken AccessToken { get; }
}