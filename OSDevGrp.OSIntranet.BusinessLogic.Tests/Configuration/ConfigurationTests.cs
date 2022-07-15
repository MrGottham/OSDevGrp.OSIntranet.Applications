using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Configuration;

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

            string result = sut[SecurityConfigurationKeys.JwtKey];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtKey_ReturnsKeyMatchingJwtKeyRegularExpression()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.JwtKey];

            Assert.That(ConfigurationValueRegularExpressions.JwtKeyRegularExpression.IsMatch(result));
        }
    }
}