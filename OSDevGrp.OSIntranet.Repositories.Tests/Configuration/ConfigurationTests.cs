using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Repositories.Tests.Configuration
{
    [TestFixture]
    public class ConfigurationTests : ConfigurationTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityMicrosoftClientId_ReturnsClientId()
        {
            IConfiguration sut = CreateSut();

            string result = sut["Security:Microsoft:ClientId"];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityMicrosoftClientSecret_ReturnsClientSecret()
        {
            IConfiguration sut = CreateSut();

            string result = sut["Security:Microsoft:ClientSecret"];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityMicrosoftTenant_ReturnsTenant()
        {
            IConfiguration sut = CreateSut();

            string result = sut["Security:Microsoft:Tenant"];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }
    }
}
