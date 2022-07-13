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
    public class AddStringConfigurationValidationTests
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
        public void AddStringConfigurationValidation_WhenConfigurationIsNull_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddStringConfigurationValidation(null, _fixture.Create<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("configuration"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void AddStringConfigurationValidation_WhenKeyIsNull_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddStringConfigurationValidation(_configurationMock.Object, null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("key"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void AddStringConfigurationValidation_WhenKeyIsEmpty_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddStringConfigurationValidation(_configurationMock.Object, string.Empty));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("key"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void AddStringConfigurationValidation_WhenKeyIsWhiteSpace_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddStringConfigurationValidation(_configurationMock.Object, " "));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("key"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void AddStringConfigurationValidation_WhenCalled_ExpectOneConfigurationValueValidatorAddedToConfigurationValueValidators()
        {
            Sut sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            sut.AddStringConfigurationValidation(_configurationMock.Object, _fixture.Create<string>());

            Assert.That(sut.ConfigurationValueValidators.Count(), Is.EqualTo(count + 1));
        }

        [Test]
        [Category("UnitTest")]
        public void AddStringConfigurationValidation_WhenCalled_ExpectStringConfigurationValidatorAddedToConfigurationValueValidators()
        {
            Sut sut = CreateSut();

            sut.AddStringConfigurationValidation(_configurationMock.Object, _fixture.Create<string>());

            Assert.That(sut.ConfigurationValueValidators.Last(), Is.TypeOf<Core.HealthChecks.StringConfigurationValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void AddStringConfigurationValidation_WhenCalled_ReturnsNotNull()
        {
            Sut sut = CreateSut();

            Sut result = sut.AddStringConfigurationValidation(_configurationMock.Object, _fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void AddStringConfigurationValidation_WhenCalled_ReturnsSameSut()
        {
            Sut sut = CreateSut();

            Sut result = sut.AddStringConfigurationValidation(_configurationMock.Object, _fixture.Create<string>());

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

            public new Sut AddStringConfigurationValidation(IConfiguration configuration, string key)
            {
                return base.AddStringConfigurationValidation(configuration, key);
            }

            #endregion
        }
    }
}