using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;

public interface IPermissionValidator
{
    bool IsAuthenticated(ClaimsPrincipal user);

    bool HasClaim(ClaimsPrincipal user, Predicate<Claim> match);
}