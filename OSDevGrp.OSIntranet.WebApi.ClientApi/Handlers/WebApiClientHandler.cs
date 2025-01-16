using System.Security.Authentication;

namespace OSDevGrp.OSIntranet.WebApi.ClientApi.Handlers;

internal class WebApiClientHandler : HttpClientHandler
{
    #region Constructor

    public WebApiClientHandler()
    {
        SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
    }

    #endregion
}