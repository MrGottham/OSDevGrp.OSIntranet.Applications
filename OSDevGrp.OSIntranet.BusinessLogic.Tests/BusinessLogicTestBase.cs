using Microsoft.Extensions.Configuration;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests
{
    public abstract class BusinessLogicTestBase
    {
        #region Methods

        protected IConfiguration CreateTestConfiguration()
        {
            return new ConfigurationBuilder()
                .AddUserSecrets<BusinessLogicTestBase>()
                .Build();
        }

        #endregion
    }
}
