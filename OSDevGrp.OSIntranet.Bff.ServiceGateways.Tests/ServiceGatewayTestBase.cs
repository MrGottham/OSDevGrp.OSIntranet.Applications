using Microsoft.Extensions.Configuration;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests;

public abstract class ServiceGatewayTestBase
{
    #region Methods

    protected IConfiguration CreateTestConfiguration()
    {
        return new ConfigurationBuilder()
            .AddUserSecrets<ServiceGatewayTestBase>()
            .Build();
    }

    #endregion
}