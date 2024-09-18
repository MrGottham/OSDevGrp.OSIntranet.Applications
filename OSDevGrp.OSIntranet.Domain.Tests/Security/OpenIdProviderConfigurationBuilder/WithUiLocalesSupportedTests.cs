using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.OpenIdProviderConfigurationBuilder
{
    [TestFixture]
    public class WithUiLocalesSupportedTests : OpenIdProviderConfigurationBuilderTestBase
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void WithUiLocalesSupported_WhenUiLocalesSupportedIsNull_ThrowsArgumentNullException()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithUiLocalesSupported(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("uiLocalesSupported"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithUiLocalesSupported_WhenUiLocalesSupportedIsNotNull_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithUiLocalesSupported(CreateUiLocalesSupported(_fixture));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithUiLocalesSupported_WhenUiLocalesSupportedIsNotNull_ReturnsSameOpenIdProviderConfigurationBuilder()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithUiLocalesSupported(CreateUiLocalesSupported(_fixture));

            Assert.That(result, Is.SameAs(sut));
        }
    }
}