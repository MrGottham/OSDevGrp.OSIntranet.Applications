using Microsoft.Extensions.Configuration;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Configuration
{
    public abstract class ConfigurationTestBase
    {
		#region Methods

		protected IConfiguration CreateSut()
		{
			return new ConfigurationBuilder()
				.AddUserSecrets<ConfigurationTestBase>()
				.Build();
		}

		#endregion
	}
}