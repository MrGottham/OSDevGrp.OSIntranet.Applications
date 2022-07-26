﻿using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Core.Tests.HealthChecks.SecurityHealthCheckOptions
{
    [TestFixture]
    public class WithGoogleValidationTests
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
        public void WithGoogleValidation_WhenConfigurationIsNull_ThrowsArgumentNullException()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithGoogleValidation(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("configuration"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void WithGoogleValidation_WhenConfigurationIsNotNull_ReturnsNotNull()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithGoogleValidation(_configurationMock.Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithGoogleValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptions()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithGoogleValidation(_configurationMock.Object);

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public void WithGoogleValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereConfigurationValueValidatorsIsNotNull()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithGoogleValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithGoogleValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereConfigurationValueValidatorsIsNotEmpty()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithGoogleValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void WithGoogleValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereTwoConfigurationValueValidatorWasAddedToConfigurationValueValidators()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithGoogleValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators.Count(), Is.EqualTo(count + 2));
        }

        [Test]
        [Category("UnitTest")]
        public void WithGoogleValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereStringConfigurationValidatorWasAddedToConfigurationValueValidatorsForGoogleClientId()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithGoogleValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators.ElementAt(count), Is.TypeOf<Core.HealthChecks.StringConfigurationValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void WithGoogleValidation_WhenConfigurationIsNotNull_ReturnsSameSecurityHealthCheckOptionsWhereStringConfigurationValidatorWasAddedToConfigurationValueValidatorsForGoogleClientSecret()
        {
            Core.HealthChecks.SecurityHealthCheckOptions sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            Core.HealthChecks.SecurityHealthCheckOptions result = sut.WithGoogleValidation(_configurationMock.Object);

            Assert.That(result.ConfigurationValueValidators.ElementAt(count + 1), Is.TypeOf<Core.HealthChecks.StringConfigurationValidator>());
        }

        private Core.HealthChecks.SecurityHealthCheckOptions CreateSut()
        {
            return new Core.HealthChecks.SecurityHealthCheckOptions();
        }
    }
}