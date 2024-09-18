using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;

namespace OSDevGrp.OSIntranet.Core.Tests.HealthChecks.SecurityHealthCheckOptions
{
    [TestFixture]
    public class WithMicrosoftValidationTests
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
        [TestCase(true)]
        [TestCase(false)]
        public void WithMicrosoftValidation_WhenConfigurationIsNull_ThrowsArgumentNullException(bool requireTenant)
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithMicrosoftValidation(null, requireTenant));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("configuration"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void WithMicrosoftValidation_WhenConfigurationIsNotNull_ReturnsNotNull(bool requireTenant)
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithMicrosoftValidation(_configurationMock.Object, requireTenant);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void WithMicrosoftValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptions(bool requireTenant)
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithMicrosoftValidation(_configurationMock.Object, requireTenant);

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void WithMicrosoftValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereConfigurationValueValidatorsIsNotNull(bool requireTenant)
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithMicrosoftValidation(_configurationMock.Object, requireTenant);

            Assert.That(result.ConfigurationValueValidators, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void WithMicrosoftValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereConfigurationValueValidatorsIsNotEmpty(bool requireTenant)
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithMicrosoftValidation(_configurationMock.Object, requireTenant);

            Assert.That(result.ConfigurationValueValidators, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true, 3)]
        [TestCase(false, 2)]
        public void WithMicrosoftValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereExpectedNumberOfConfigurationValueValidatorsWasAddedToConfigurationValueValidators(bool requireTenant, int expectedNumberOfConfigurationValueValidators)
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithMicrosoftValidation(_configurationMock.Object, requireTenant);

            Assert.That(result.ConfigurationValueValidators.Count(), Is.EqualTo(count + expectedNumberOfConfigurationValueValidators));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void WithMicrosoftValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereStringConfigurationValidatorWasAddedToConfigurationValueValidatorsForMicrosoftClientId(bool requireTenant)
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithMicrosoftValidation(_configurationMock.Object, requireTenant);

            Assert.That(result.ConfigurationValueValidators.ElementAt(count), Is.TypeOf<Core.HealthChecks.StringConfigurationValidator>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void WithMicrosoftValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereStringConfigurationValidatorWasAddedToConfigurationValueValidatorsForMicrosoftClientSecret(bool requireTenant)
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithMicrosoftValidation(_configurationMock.Object, requireTenant);

            Assert.That(result.ConfigurationValueValidators.ElementAt(count + 1), Is.TypeOf<Core.HealthChecks.StringConfigurationValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void WithMicrosoftValidation_WhenConfigurationIsNotNullAndRequireTenantIsTrue_ReturnsSameSecurityHealthCheckOptionsWhereStringConfigurationValidatorWasAddedToConfigurationValueValidatorsForMicrosoftTenant()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithMicrosoftValidation(_configurationMock.Object, true);

            Assert.That(result.ConfigurationValueValidators.ElementAt(count + 2), Is.TypeOf<Core.HealthChecks.StringConfigurationValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void WithMicrosoftValidation_WhenConfigurationIsNotNullAndRequireTenantIsFalse_ReturnsSameSecurityHealthCheckOptionsWhereStringConfigurationValidatorWasNotAddedToConfigurationValueValidatorsForMicrosoftTenant()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithMicrosoftValidation(_configurationMock.Object, false);

            Assert.That(result.ConfigurationValueValidators.Count(), Is.EqualTo(count + 2));
        }

        private Core.HealthChecks.SecurityHealthCheckOptions CreateSut()
        {
            return new Core.HealthChecks.SecurityHealthCheckOptions();
        }
    }
}