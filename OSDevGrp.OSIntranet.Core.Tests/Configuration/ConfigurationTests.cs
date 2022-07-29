using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Configuration;

namespace OSDevGrp.OSIntranet.Core.Tests.Configuration
{
    [TestFixture]
    public class ConfigurationTests
    {
        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityAcmeChallengeWellKnownChallengeToken_ReturnsWellKnownChallengeToken()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.AcmeChallengeWellKnownChallengeToken];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityAcmeChallengeConstructedKeyAuthorization_ReturnsConstructedKeyAuthorization()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.AcmeChallengeConstructedKeyAuthorization];

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