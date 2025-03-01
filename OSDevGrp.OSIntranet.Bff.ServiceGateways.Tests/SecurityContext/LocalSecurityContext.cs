using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests.SecurityContext;

internal class LocalSecurityContext : ISecurityContext
{
    #region Constructor

    public LocalSecurityContext(ClaimsPrincipal user, IToken accessToken)
    {
        User = user;
        AccessToken = accessToken;
    }

    #endregion

    #region Properties

    public ClaimsPrincipal User { get; }

    public IToken AccessToken { get; }

    #endregion
}