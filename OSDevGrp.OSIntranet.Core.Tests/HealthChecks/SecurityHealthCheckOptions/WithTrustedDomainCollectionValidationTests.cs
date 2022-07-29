using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Core.Tests.HealthChecks.SecurityHealthCheckOptions
{
    [TestFixture]
    public class WithTrustedDomainCollectionValidationTests
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
        public void WithTrustedDomainCollectionValidation_WhenConfigurationIsNull_ThrowsArgumentNullException()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithTrustedDomainCollectionValidation(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("configuration"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void WithTrustedDomainCollectionValidation_WhenConfigurationIsNotNull_ReturnsNotNull()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithTrustedDomainCollectionValidation(_configurationMock.Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithTrustedDomainCollectionValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptions()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithTrustedDomainCollectionValidation(_configurationMock.Object);

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public void WithTrustedDomainCollectionValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereConfigurationValueValidatorsIsNotNull()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithTrustedDomainCollectionValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithTrustedDomainCollectionValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereConfigurationValueValidatorsIsNotEmpty()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithTrustedDomainCollectionValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void WithTrustedDomainCollectionValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereOneConfigurationValueValidatorWasAddedToConfigurationValueValidators()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithTrustedDomainCollectionValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators.Count(), Is.EqualTo(count + 1));
        }

        [Test]
        [Category("UnitTest")]
        public void WithTrustedDomainCollectionValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereStringCollectionConfigurationValidatorWasAddedToConfigurationValueValidators()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithTrustedDomainCollectionValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators.Last(), Is.TypeOf<Core.HealthChecks.StringCollectionConfigurationValidator>());
        }

        private Core.HealthChecks.SecurityHealthCheckOptions CreateSut()
        {
            return new Core.HealthChecks.SecurityHealthCheckOptions();
        }
    }
}