using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests.SecurityContext;

internal class LocalSecurityContext : ISecurityContext
{
    #region Constructor

    public LocalSecurityContext(IToken accessToken)
    {
        AccessToken = accessToken;
    }

    #endregion

    #region Properties

    public IToken AccessToken { get; }

    #endregion
}