using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Security;

internal class SecurityContextProvider : ISecurityContextProvider
{
    #region Private variables

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenStorage _tokenStorage;

    #endregion

    #region Constructor

    public SecurityContextProvider(IHttpContextAccessor httpContextAccessor, ITokenStorage tokenStorage)
    {
        _httpContextAccessor = httpContextAccessor;
        _tokenStorage = tokenStorage;
    }

    #endregion

    #region Methods

    public async Task<ISecurityContext> GetCurrentSecurityContextAsync(CancellationToken cancellationToken = default)
    {
        HttpContext? httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new InvalidOperationException("No HTTP context was resolved from the HTTP context accessor.");
        }

        ClaimsPrincipal user = httpContext.User;
        IToken accessToken = await _tokenStorage.GetTokenAsync(user, cancellationToken);

        return LocalSecurityContext.Create(user, accessToken);
    }

    #endregion
}