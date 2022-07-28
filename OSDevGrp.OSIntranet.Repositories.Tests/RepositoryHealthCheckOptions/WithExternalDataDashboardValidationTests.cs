using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.HealthChecks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.RepositoryHealthCheckOptions
{
    [TestFixture]
    public class WithExternalDataDashboardValidationTests
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
        public void WithExternalDataDashboardValidation_WhenConfigurationIsNull_ThrowsArgumentNullException()
        {
            Repositories.RepositoryHealthCheckOptions sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithExternalDataDashboardValidation(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("configuration"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void WithExternalDataDashboardValidation_WhenConfigurationIsNotNull_ReturnsNotNull()
        {
            Repositories.RepositoryHealthCheckOptions sut = CreateSut();

            Repositories.RepositoryHealthCheckOptions result = sut.WithExternalDataDashboardValidation(_configurationMock.Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithExternalDataDashboardValidation_WhenConfigurationIsNotNull_ReturnsSameRepositoryHealthCheckOptions()
        {
            Repositories.RepositoryHealthCheckOptions sut = CreateSut();

            Repositories.RepositoryHealthCheckOptions result = sut.WithExternalDataDashboardValidation(_configurationMock.Object);

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public void WithExternalDataDashboardValidation_WhenConfigurationIsNotNull_ReturnsSameRepositoryHealthCheckOptionsWhereConfigurationValueValidatorsIsNotNull()
        {
            Repositories.RepositoryHealthCheckOptions sut = CreateSut();

            Repositories.RepositoryHealthCheckOptions result = sut.WithExternalDataDashboardValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithExternalDataDashboardValidation_WhenConfigurationIsNotNull_ReturnsSameRepositoryHealthCheckOptionsWhereConfigurationValueValidatorsIsNotEmpty()
        {
            Repositories.RepositoryHealthCheckOptions sut = CreateSut();

            Repositories.RepositoryHealthCheckOptions result = sut.WithExternalDataDashboardValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void WithExternalDataDashboardValidation_WhenConfigurationIsNotNull_ReturnsSameRepositoryHealthCheckOptionsWhereOneConfigurationValueValidatorWasAddedToConfigurationValueValidators()
        {
            Repositories.RepositoryHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Repositories.RepositoryHealthCheckOptions result = sut.WithExternalDataDashboardValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators.Count(), Is.EqualTo(count + 1));
        }

        [Test]
        [Category("UnitTest")]
        public void WithExternalDataDashboardValidation_WhenConfigurationIsNotNull_ReturnsSameRepositoryHealthCheckOptionsWhereEndpointConfigurationValidatorWasAddedToConfigurationValueValidators()
        {
            Repositories.RepositoryHealthCheckOptions sut = CreateSut();

            Repositories.RepositoryHealthCheckOptions result = sut.WithExternalDataDashboardValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators.Last(), Is.TypeOf<EndpointConfigurationValidator>());
        }

        private Repositories.RepositoryHealthCheckOptions CreateSut()
        {
            return new Repositories.RepositoryHealthCheckOptions();
        }
    }
}