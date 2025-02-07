using OSDevGrp.OSIntranet.Bff.ServiceGateways.Extensions;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways;

internal class SecurityGateway : ServiceGatewayBase, ISecurityGateway
{
    #region Constructor

    public SecurityGateway(IWebApiClient webApiClient)
        : base(webApiClient)
    {
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