using System;
using System.Linq;
using AutoFixture;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.HealthChecks;

namespace OSDevGrp.OSIntranet.Core.Tests.HealthChecks.ConfigurationHealthCheckOptionsBase
{
    [TestFixture]
    public class AddEndpointConfigurationValidationTests
    {
        #region Private variables

        private Mock<IConfiguration> _configurationMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _configurationMock = new Mock<IConfiguration>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void AddEndpointConfigurationValidation_WhenConfigurationIsNull_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddEndpointConfigurationValidation(null, _fixture.Create<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("configuration"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void AddEndpointConfigurationValidation_WhenKeyIsNull_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddEndpointConfigurationValidation(_configurationMock.Object, null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("key"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void AddEndpointConfigurationValidation_WhenKeyIsEmpty_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddEndpointConfigurationValidation(_configurationMock.Object, string.Empty));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("key"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void AddEndpointConfigurationValidation_WhenKeyIsWhiteSpace_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddEndpointConfigurationValidation(_configurationMock.Object, " "));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("key"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void AddEndpointConfigurationValidation_WhenCalled_ExpectOneConfigurationValueValidatorAddedToConfigurationValueValidators()
        {
            Sut sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            sut.AddEndpointConfigurationValidation(_configurationMock.Object, _fixture.Create<string>());

            Assert.That(sut.ConfigurationValueValidators.Count(), Is.EqualTo(count + 1));
        }

        [Test]
        [Category("UnitTest")]
        public void AddEndpointConfigurationValidation_WhenCalled_ExpectEndpointConfigurationValidatorAddedToConfigurationValueValidators()
        {
            Sut sut = CreateSut();

            sut.AddEndpointConfigurationValidation(_configurationMock.Object, _fixture.Create<string>());

            Assert.That(sut.ConfigurationValueValidators.Last(), Is.TypeOf<Core.HealthChecks.EndpointConfigurationValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void AddEndpointConfigurationValidation_WhenCalled_ReturnsNotNull()
        {
            Sut sut = CreateSut();

            Sut result = sut.AddEndpointConfigurationValidation(_configurationMock.Object, _fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void AddEndpointConfigurationValidation_WhenCalled_ReturnsSameSut()
        {
            Sut sut = CreateSut();

            Sut result = sut.AddEndpointConfigurationValidation(_configurationMock.Object, _fixture.Create<string>());

            Assert.That(result, Is.SameAs(sut));
        }

        private Sut CreateSut()
        {
            return new Sut();
        }

        private class Sut : ConfigurationHealthCheckOptionsBase<Sut>
        {
            #region Properties

            protected override Sut HealthCheckOptions => this;

            #endregion

            #region Methods

            public new Sut AddEndpointConfigurationValidation(IConfiguration configuration, string key)
            {
                return base.AddEndpointConfigurationValidation(configuration, key);
            }

            #endregion
        }
    }
}