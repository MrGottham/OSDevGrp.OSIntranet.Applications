using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Extensions;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Options;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways;

internal class SecurityGateway : ServiceGatewayBase, ISecurityGateway
{
    #region Private variables

    private readonly IOptions<WebApiOptions> _webApiOptions;

    #endregion

    #region Constructor

    public SecurityGateway(IWebApiClient webApiClient, IOptions<WebApiOptions> webApiOptions)
        : base(webApiClient)
    {
        _webApiOptions = webApiOptions;
    }

    #endregion

    #region Methods

    public async Task<AccessTokenModel> AquireTokenAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await WebApiClient.TokenAsync("client_credentials", string.Empty, string.Empty, string.Empty, string.Empty, cancellationToken);
        }
        catch (WebApiClientException<ErrorResponseModel> ex)
        {
            throw ex.ToServiceGatewayException();
        }
        catch (WebApiClientException ex)
        {
            throw ex.ToServiceGatewayException();
        }
    }

    #endregion
}