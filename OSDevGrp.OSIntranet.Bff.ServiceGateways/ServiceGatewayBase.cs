using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways;

internal abstract class ServiceGatewayBase : IServiceGateway
{
    #region Constructor

    protected ServiceGatewayBase(IWebApiClient webApiClient)
    {
        WebApiClient = webApiClient;
    }

    #endregion

    #region Properties

    protected IWebApiClient WebApiClient { get; }

    #endregion

    #region Methods
    #endregion
}