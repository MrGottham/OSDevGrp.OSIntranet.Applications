using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;

namespace OSDevGrp.OSIntranet.Core.Tests.HealthChecks.SecurityHealthCheckOptions
{
    [TestFixture]
    public class WithJwtValidationTests
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
        public void WithJwtValidation_WhenConfigurationIsNull_ThrowsArgumentNullException()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithJwtValidation(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("configuration"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithJwtValidation_WhenConfigurationIsNotNull_ReturnsNotNull()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithJwtValidation(_configurationMock.Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithJwtValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptions()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithJwtValidation(_configurationMock.Object);

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public void WithJwtValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereConfigurationValueValidatorsIsNotNull()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithJwtValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithJwtValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereConfigurationValueValidatorsIsNotEmpty()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithJwtValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void WithJwtValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereElevenConfigurationValueValidatorWasAddedToConfigurationValueValidators()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithJwtValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators.Count(), Is.EqualTo(count + 11));
        }

        [Test]
        [Category("UnitTest")]
        public void WithJwtValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereRegularExpressionConfigurationValidatorWasAddedAsElementOneToConfigurationValueValidators()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithJwtValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators.ElementAt(count), Is.TypeOf<Core.HealthChecks.RegularExpressionConfigurationValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void WithJwtValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereRegularExpressionConfigurationValidatorWasAddedAsElementTwoToConfigurationValueValidators()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithJwtValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators.ElementAt(count + 1), Is.TypeOf<Core.HealthChecks.RegularExpressionConfigurationValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void WithJwtValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereRegularExpressionConfigurationValidatorWasAddedAsElementThreeToConfigurationValueValidators()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithJwtValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators.ElementAt(count + 2), Is.TypeOf<Core.HealthChecks.RegularExpressionConfigurationValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void WithJwtValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereRegularExpressionConfigurationValidatorWasAddedAsElementFourToConfigurationValueValidators()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithJwtValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators.ElementAt(count + 3), Is.TypeOf<Core.HealthChecks.RegularExpressionConfigurationValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void WithJwtValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereRegularExpressionConfigurationValidatorWasAddedAsElementFiveToConfigurationValueValidators()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithJwtValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators.ElementAt(count + 4), Is.TypeOf<Core.HealthChecks.RegularExpressionConfigurationValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void WithJwtValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereRegularExpressionConfigurationValidatorWasAddedAsElementSixToConfigurationValueValidators()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithJwtValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators.ElementAt(count + 5), Is.TypeOf<Core.HealthChecks.RegularExpressionConfigurationValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void WithJwtValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereRegularExpressionConfigurationValidatorWasAddedAsElementSevenToConfigurationValueValidators()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithJwtValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators.ElementAt(count + 6), Is.TypeOf<Core.HealthChecks.RegularExpressionConfigurationValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void WithJwtValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereRegularExpressionConfigurationValidatorWasAddedAsElementEightToConfigurationValueValidators()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithJwtValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators.ElementAt(count + 7), Is.TypeOf<Core.HealthChecks.RegularExpressionConfigurationValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void WithJwtValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereRegularExpressionConfigurationValidatorWasAddedAsElementNineToConfigurationValueValidators()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithJwtValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators.ElementAt(count + 8), Is.TypeOf<Core.HealthChecks.RegularExpressionConfigurationValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void WithJwtValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereEndpointConfigurationValidatorWasAddedAsElementTenToConfigurationValueValidators()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithJwtValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators.ElementAt(count + 9), Is.TypeOf<Core.HealthChecks.EndpointConfigurationValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void WithJwtValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereEndpointConfigurationValidatorWasAddedAsElementElevenToConfigurationValueValidators()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithJwtValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators.ElementAt(count + 10), Is.TypeOf<Core.HealthChecks.EndpointConfigurationValidator>());
        }

        private Core.HealthChecks.SecurityHealthCheckOptions CreateSut()
        {
            return new Core.HealthChecks.SecurityHealthCheckOptions();
        }
    }
}