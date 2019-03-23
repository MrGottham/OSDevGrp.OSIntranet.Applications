using Microsoft.Extensions.Configuration;

namespace OSDevGrp.OSIntranet.Repositories.Tests.Configuration
{
    public abstract class ConfigurationTestBase : RepositoryTestBase
    {
        protected IConfiguration CreateSut()
        {
            return CreateTestConfiguration();
        }
    }
}
