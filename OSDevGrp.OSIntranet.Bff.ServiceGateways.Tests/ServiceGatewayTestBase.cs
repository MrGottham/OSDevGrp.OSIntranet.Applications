using Microsoft.Extensions.Configuration;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests;

public abstract class ServiceGatewayTestBase
{
    #region Methods

    protected static IConfiguration CreateTestConfiguration()
    {
        return new ConfigurationBuilder()
            .AddUserSecrets<ServiceGatewayTestBase>()
            .Build();
    }

    #endregion
}