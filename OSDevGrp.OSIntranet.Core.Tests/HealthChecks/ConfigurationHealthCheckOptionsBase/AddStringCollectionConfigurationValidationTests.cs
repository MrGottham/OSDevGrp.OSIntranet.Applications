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
    public class AddStringCollectionConfigurationValidationTests
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
        public void AddStringCollectionConfigurationValidation_WhenConfigurationIsNull_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddStringCollectionConfigurationValidation(null, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<int>(), _fixture.Create<int>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("configuration"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void AddStringCollectionConfigurationValidation_WhenKeyIsNull_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddStringCollectionConfigurationValidation(_configurationMock.Object, null, _fixture.Create<string>(), _fixture.Create<int>(), _fixture.Create<int>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("key"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void AddStringCollectionConfigurationValidation_WhenKeyIsEmpty_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddStringCollectionConfigurationValidation(_configurationMock.Object, string.Empty, _fixture.Create<string>(), _fixture.Create<int>(), _fixture.Create<int>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("key"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void AddStringCollectionConfigurationValidation_WhenKeyIsWhiteSpace_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddStringCollectionConfigurationValidation(_configurationMock.Object, " ", _fixture.Create<string>(), _fixture.Create<int>(), _fixture.Create<int>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("key"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void AddStringCollectionConfigurationValidation_WhenSeparatorIsNull_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddStringCollectionConfigurationValidation(_configurationMock.Object, _fixture.Create<string>(), null, _fixture.Create<int>(), _fixture.Create<int>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("separator"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void AddStringCollectionConfigurationValidation_WhenSeparatorIsEmpty_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddStringCollectionConfigurationValidation(_configurationMock.Object, _fixture.Create<string>(), string.Empty, _fixture.Create<int>(), _fixture.Create<int>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("separator"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void AddStringCollectionConfigurationValidation_WhenSeparatorIsWhiteSpace_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddStringCollectionConfigurationValidation(_configurationMock.Object, _fixture.Create<string>(), " ", _fixture.Create<int>(), _fixture.Create<int>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("separator"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void AddStringCollectionConfigurationValidation_WhenCalled_ExpectOneConfigurationValueValidatorAddedToConfigurationValueValidators()
        {
            Sut sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            sut.AddStringCollectionConfigurationValidation(_configurationMock.Object, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<int>(), _fixture.Create<int>());

            Assert.That(sut.ConfigurationValueValidators.Count(), Is.EqualTo(count + 1));
        }

        [Test]
        [Category("UnitTest")]
        public void AddStringCollectionConfigurationValidation_WhenCalled_ExpectStringCollectionConfigurationValidatorAddedToConfigurationValueValidators()
        {
            Sut sut = CreateSut();

            sut.AddStringCollectionConfigurationValidation(_configurationMock.Object, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<int>(), _fixture.Create<int>());

            Assert.That(sut.ConfigurationValueValidators.Last(), Is.TypeOf<Core.HealthChecks.StringCollectionConfigurationValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void AddStringCollectionConfigurationValidation_WhenCalled_ReturnsNotNull()
        {
            Sut sut = CreateSut();

            Sut result = sut.AddStringCollectionConfigurationValidation(_configurationMock.Object, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<int>(), _fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void AddStringCollectionConfigurationValidation_WhenCalled_ReturnsSameSut()
        {
            Sut sut = CreateSut();

            Sut result = sut.AddStringCollectionConfigurationValidation(_configurationMock.Object, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<int>(), _fixture.Create<int>());

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

            public new Sut AddStringCollectionConfigurationValidation(IConfiguration configuration, string key, string separator, int minLength = 1, int maxLength = 32)
            {
                return base.AddStringCollectionConfigurationValidation(configuration, key, separator, minLength, maxLength);
            }

            #endregion
        }
    }
}