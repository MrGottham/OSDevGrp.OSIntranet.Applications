using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;

namespace OSDevGrp.OSIntranet.Core.Tests.HealthChecks.SecurityHealthCheckOptions
{
    [TestFixture]
    public class WithOpenIdConnectTests
    {
        #region Private variables

        private Mock<IConfiguration> _configurationMock;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _configurationMock = new Mock<IConfiguration>();
        }

        [Test]
        [Category("UnitTest")]
        public void WithOpenIdConnect_WhenConfigurationIsNull_ThrowsArgumentNullException()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithOpenIdConnect(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("configuration"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithOpenIdConnect_WhenConfigurationIsNotNull_ReturnsNotNull()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithOpenIdConnect(_configurationMock.Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithOpenIdConnect_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptions()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithOpenIdConnect(_configurationMock.Object);

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public void WithOpenIdConnect_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereConfigurationValueValidatorsIsNotNull()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithOpenIdConnect(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithOpenIdConnect_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereConfigurationValueValidatorsIsNotEmpty()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithOpenIdConnect(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void WithOpenIdConnect_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereThreeConfigurationValueValidatorsWasAddedToConfigurationValueValidators()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithOpenIdConnect(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators.Count(), Is.EqualTo(count + 3));
        }

        [Test]
        [Category("UnitTest")]
        public void WithOpenIdConnect_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereEndpointConfigurationValidatorWasAddedToConfigurationValueValidatorsForOpenIdConnectAuthority()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithOpenIdConnect(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators.ElementAt(count), Is.TypeOf<Core.HealthChecks.EndpointConfigurationValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void WithOpenIdConnect_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereStringConfigurationValidatorWasAddedToConfigurationValueValidatorsForOpenIdConnectClientId()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithOpenIdConnect(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators.ElementAt(count + 1), Is.TypeOf<Core.HealthChecks.StringConfigurationValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void WithOpenIdConnect_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereStringConfigurationValidatorWasAddedToConfigurationValueValidatorsForOpenIdConnectClientSecret()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithOpenIdConnect(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators.ElementAt(count + 2), Is.TypeOf<Core.HealthChecks.StringConfigurationValidator>());
        }

        private Core.HealthChecks.SecurityHealthCheckOptions CreateSut()
        {
            return new Core.HealthChecks.SecurityHealthCheckOptions();
        }
    }
}
