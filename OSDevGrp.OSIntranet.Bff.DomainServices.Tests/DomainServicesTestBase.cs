using Microsoft.Extensions.Configuration;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests;

public abstract class DomainServicesTestBase
{
    #region Methods

    protected static IConfiguration CreateTestConfiguration()
    {
        return new ConfigurationBuilder()
            .AddUserSecrets<DomainServicesTestBase>()
            .Build();
    }

    #endregion
}