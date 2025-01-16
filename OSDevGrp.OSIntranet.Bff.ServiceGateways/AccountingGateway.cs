using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways;

internal class AccountingGateway : ServiceGatewayBase, IAccountingGateway
{
    #region Constructor

    public AccountingGateway(IWebApiClient webApiClient)
        : base(webApiClient)
    {
    }

    #endregion

    #region Methods
    #endregion
}