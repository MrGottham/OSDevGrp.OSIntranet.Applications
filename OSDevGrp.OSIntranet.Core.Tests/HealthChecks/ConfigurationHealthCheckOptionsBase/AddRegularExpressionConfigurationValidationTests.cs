using System;
using System.Linq;
using System.Text.RegularExpressions;
using AutoFixture;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.HealthChecks;

namespace OSDevGrp.OSIntranet.Core.Tests.HealthChecks.ConfigurationHealthCheckOptionsBase
{
    [TestFixture]
    public class AddRegularExpressionConfigurationValidationTests
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
        public void AddRegularExpressionConfigurationValidation_WhenConfigurationIsNull_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddRegularExpressionConfigurationValidation(null, _fixture.Create<string>(), new Regex($"^{_fixture.Create<string>()}$", RegexOptions.Compiled)));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("configuration"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void AddRegularExpressionConfigurationValidation_WhenKeyIsNull_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddRegularExpressionConfigurationValidation(_configurationMock.Object, null, new Regex($"^{_fixture.Create<string>()}$", RegexOptions.Compiled)));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("key"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void AddRegularExpressionConfigurationValidation_WhenKeyIsEmpty_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddRegularExpressionConfigurationValidation(_configurationMock.Object, string.Empty, new Regex($"^{_fixture.Create<string>()}$", RegexOptions.Compiled)));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("key"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void AddRegularExpressionConfigurationValidation_WhenKeyIsWhiteSpace_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddRegularExpressionConfigurationValidation(_configurationMock.Object, " ", new Regex($"^{_fixture.Create<string>()}$", RegexOptions.Compiled)));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("key"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void AddRegularExpressionConfigurationValidation_WhenRegularExpressionIsNull_ThrowsArgumentNullException()
        {
            Sut sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddRegularExpressionConfigurationValidation(_configurationMock.Object, _fixture.Create<string>(), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("regularExpression"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void AddRegularExpressionConfigurationValidation_WhenCalled_ExpectOneConfigurationValueValidatorAddedToConfigurationValueValidators()
        {
            Sut sut = CreateSut();
            int count = sut.ConfigurationValueValidators.Count();

            sut.AddRegularExpressionConfigurationValidation(_configurationMock.Object, _fixture.Create<string>(), new Regex($"^{_fixture.Create<string>()}$", RegexOptions.Compiled));

            Assert.That(sut.ConfigurationValueValidators.Count(), Is.EqualTo(count + 1));
        }

        [Test]
        [Category("UnitTest")]
        public void AddRegularExpressionConfigurationValidation_WhenCalled_ExpectRegularExpressionConfigurationValidatorAddedToConfigurationValueValidators()
        {
            Sut sut = CreateSut();

            sut.AddRegularExpressionConfigurationValidation(_configurationMock.Object, _fixture.Create<string>(), new Regex($"^{_fixture.Create<string>()}$", RegexOptions.Compiled));

            Assert.That(sut.ConfigurationValueValidators.Last(), Is.TypeOf<Core.HealthChecks.RegularExpressionConfigurationValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void AddRegularExpressionConfigurationValidation_WhenCalled_ReturnsNotNull()
        {
            Sut sut = CreateSut();

            Sut result = sut.AddRegularExpressionConfigurationValidation(_configurationMock.Object, _fixture.Create<string>(), new Regex($"^{_fixture.Create<string>()}$", RegexOptions.Compiled));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void AddRegularExpressionConfigurationValidation_WhenCalled_ReturnsSameSut()
        {
            Sut sut = CreateSut();

            Sut result = sut.AddRegularExpressionConfigurationValidation(_configurationMock.Object, _fixture.Create<string>(), new Regex($"^{_fixture.Create<string>()}$", RegexOptions.Compiled));

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

            public new Sut AddRegularExpressionConfigurationValidation(IConfiguration configuration, string key, Regex regularExpression)
            {
                return base.AddRegularExpressionConfigurationValidation(configuration, key, regularExpression);
            }

            #endregion
        }
    }
}