using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Security;

internal class PermissionValidator : IPermissionValidator
{
    #region Methods

    public bool HasClaim(ClaimsPrincipal user, Predicate<Claim> match)
    {
        return user.HasClaim(match);
    }

    #endregion
}