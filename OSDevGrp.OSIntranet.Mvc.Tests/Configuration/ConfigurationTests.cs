using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Configuration;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Configuration
{
    [TestFixture]
    public class ConfigurationTests
    {
        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityMicrosoftClientId_ReturnsClientId()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.MicrosoftClientId];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityMicrosoftClientSecret_ReturnsClientSecret()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.MicrosoftClientSecret];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityGoogleClientId_ReturnsClientId()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.GoogleClientId];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityGoogleClientSecret_ReturnsClientSecret()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.GoogleClientSecret];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityTrustedDomainCollection_ReturnsTrustedDomainCollection()
        {
            IConfiguration sut = CreateSut();

            string result = sut["Security:TrustedDomainCollection"];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        private IConfiguration CreateSut()
        {
            return new ConfigurationBuilder()
                .AddUserSecrets<ConfigurationTests>()
                .Build();
        }
    }
}