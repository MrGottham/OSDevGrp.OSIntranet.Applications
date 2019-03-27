using Microsoft.Extensions.Configuration;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Configuration
{
    public abstract class ConfigurationTestBase : BusinessLogicTestBase
    {
        protected IConfiguration CreateSut()
        {
            return CreateTestConfiguration();
        }
    }
}
