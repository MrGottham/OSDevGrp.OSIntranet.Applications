using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Security;

internal class TokenProvider : ITokenProvider
{
    #region Private variables

    private readonly IServiceProvider _serviceProvider;

    #endregion

    #region Constructor

    public TokenProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    #endregion

    #region Methods

    public async Task<IToken> ResolveAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default)
    {
        ISecurityGateway securityGateway = _serviceProvider.GetRequiredService<ISecurityGateway>();
        TimeProvider timeProvider = _serviceProvider.GetRequiredService<TimeProvider>();

        AccessTokenModel accessTokenModel = await securityGateway.AquireTokenAsync(cancellationToken);

        return new LocalToken(accessTokenModel.Token_type, accessTokenModel.Access_token, timeProvider.GetUtcNow().AddSeconds(accessTokenModel.Expires_in), timeProvider);
    }

    #endregion
}