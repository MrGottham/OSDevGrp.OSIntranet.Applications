using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Options;
using System.Net.Http.Headers;
using System.Text;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Handlers;

internal class WebApiClientSecurityHandler : DelegatingHandler
{
    #region Private variables

    private readonly IOptions<WebApiOptions> _webApiOptions;
    private readonly ISecurityContextProvider _securityContextProvider;

    #endregion

    #region Constructor

    public WebApiClientSecurityHandler(IOptions<WebApiOptions> webApiOptions, ISecurityContextProvider securityContextProvider)
    {
        _webApiOptions = webApiOptions;
        _securityContextProvider = securityContextProvider;
    }

    #endregion

    #region Methods

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        AuthenticationHeaderValue? authenticationHeaderValue = await GetAuthenticationHeaderValueAsync(request.RequestUri, cancellationToken);
        if (authenticationHeaderValue != null)
        {
            request.Headers.Authorization = authenticationHeaderValue;
        }

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<AuthenticationHeaderValue?> GetAuthenticationHeaderValueAsync(Uri? requestUri, CancellationToken cancellationToken)
    {
        if (requestUri == null)
        {
            return null;
        }

        if (requestUri.LocalPath.ToLower().EndsWith("/api/oauth/token", StringComparison.InvariantCulture))
        {
            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_webApiOptions.Value.ClientId}:{_webApiOptions.Value.ClientSecret}")));
        }

        ISecurityContext securityContext = await _securityContextProvider.GetCurrentSecurityContextAsync(cancellationToken);

        if (requestUri.LocalPath.ToLower().StartsWith("/api/accounting", StringComparison.InvariantCulture))
        {
            return new AuthenticationHeaderValue(securityContext.AccessToken.TokenType, securityContext.AccessToken.Token);
        }

        if (requestUri.LocalPath.ToLower().StartsWith("/api/common", StringComparison.InvariantCulture))
        {
            return new AuthenticationHeaderValue(securityContext.AccessToken.TokenType, securityContext.AccessToken.Token);
        }

        return null;
    }

    #endregion
}