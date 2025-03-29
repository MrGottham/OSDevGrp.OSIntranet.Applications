using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Security;

internal class LocalSecurityContext : ISecurityContext
{
    #region Private variables

    private readonly Func<ClaimsPrincipal> _userGetter;
    private readonly Func<IToken> _tokenGetter;

    #endregion

    #region Constructor

    private LocalSecurityContext(Func<ClaimsPrincipal> userGetter, Func<IToken> accessToken)
    {
        _userGetter = userGetter;
        _tokenGetter = accessToken;
    }

    #endregion

    #region Properties

    public ClaimsPrincipal User => _userGetter();

    public IToken AccessToken => _tokenGetter();

    internal static ISecurityContext None => new LocalSecurityContext(() => throw new NotSupportedException(), () => throw new NotSupportedException());

    #endregion

    #region Methods

    internal static ISecurityContext Create(ClaimsPrincipal user, IToken accessToken)
    {
        return new LocalSecurityContext(() => user, () => accessToken);
    }

    #endregion
}