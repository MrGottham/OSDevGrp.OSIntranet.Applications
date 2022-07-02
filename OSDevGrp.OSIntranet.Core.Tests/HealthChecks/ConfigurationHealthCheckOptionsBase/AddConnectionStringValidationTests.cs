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
    public class AddConnectionStringValidationTests
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
        public void AddConnectionStringValidation_WhenConfigurationIsNull_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddConnectionStringValidation(null, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("configuration"));
        }

        [Test]
        [Category("UnitTest")]
        public void AddConnectionStringValidation_WhenNameIsNull_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddConnectionStringValidation(_configurationMock.Object, null));

            Assert.That(result.ParamName, Is.EqualTo("name"));
        }

        [Test]
        [Category("UnitTest")]
        public void AddConnectionStringValidation_WhenNameIsEmpty_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddConnectionStringValidation(_configurationMock.Object, string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("name"));
        }

        [Test]
        [Category("UnitTest")]
        public void AddConnectionStringValidation_WhenNameIsWhiteSpace_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddConnectionStringValidation(_configurationMock.Object, " "));

            Assert.That(result.ParamName, Is.EqualTo("name"));
        }

        [Test]
        [Category("UnitTest")]
        public void AddConnectionStringValidation_WhenCalled_ExpectOneConfigurationValueValidatorAddedToConfigurationValueValidators()
        {
            Sut sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            sut.AddConnectionStringValidation(_configurationMock.Object, _fixture.Create<string>());

            Assert.That(sut.ConfigurationValueValidators.Count(), Is.EqualTo(count + 1));
        }

        [Test]
        [Category("UnitTest")]
        public void AddConnectionStringValidation_WhenCalled_ExpectConnectionStringValidatorAddedToConfigurationValueValidators()
        {
            Sut sut = CreateSut();

            sut.AddConnectionStringValidation(_configurationMock.Object, _fixture.Create<string>());

            Assert.That(sut.ConfigurationValueValidators.Last(), Is.TypeOf<OSDevGrp.OSIntranet.Core.HealthChecks.ConnectionStringValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void AddConnectionStringValidation_WhenCalled_ReturnsNotNull()
        {
            Sut sut = CreateSut();

            Sut result = sut.AddConnectionStringValidation(_configurationMock.Object, _fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void AddConnectionStringValidation_WhenCalled_ReturnsSameSut()
        {
            Sut sut = CreateSut();

            Sut result = sut.AddConnectionStringValidation(_configurationMock.Object, _fixture.Create<string>());

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

            public new Sut AddConnectionStringValidation(IConfiguration configuration, string name)
            {
                return base.AddConnectionStringValidation(configuration, name);
            }

            #endregion
        }
    }
}