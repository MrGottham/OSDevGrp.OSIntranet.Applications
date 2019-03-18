using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Tests.Configuration
{
    [TestFixture]
    public class GetConnectionStringTests
    {
        [Test]
        [Category("IntegrationTest")]
        public void GetConnectionString_WhenCalledWithIntranetName_ReturnsConnectionString()
        {
            IConfiguration sut = CreateSut();

            string result = sut.GetConnectionString(ConnectionStringNames.IntranetName);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        private IConfiguration CreateSut()
        {
            return new ConfigurationBuilder()
                .AddUserSecrets<GetConnectionStringTests>()
                .Build();
        }
    }
}
