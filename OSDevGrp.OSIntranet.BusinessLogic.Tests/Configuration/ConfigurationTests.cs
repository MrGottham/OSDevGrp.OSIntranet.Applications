using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Configuration
{
    [TestFixture]
    public class ConfigurationTests : ConfigurationTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtKey_ReturnsKey()
        {
            IConfiguration sut = CreateSut();

            string result = sut["Security:JWT:Key"];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }
    }
}
