using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.OpenIdProviderConfigurationBuilder
{
    [TestFixture]
    public class WithIdTokenEncryptionEncValuesSupportedTests : OpenIdProviderConfigurationBuilderTestBase
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
        public void WithIdTokenEncryptionEncValuesSupported_WhenIdTokenEncryptionEncValuesSupportedIsNull_ThrowsArgumentNullException()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithIdTokenEncryptionEncValuesSupported(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("idTokenEncryptionEncValuesSupported"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithIdTokenEncryptionEncValuesSupported_WhenIdTokenEncryptionEncValuesSupportedIsNotNull_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithIdTokenEncryptionEncValuesSupported(CreateIdTokenEncryptionEncValuesSupported(_fixture));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithIdTokenEncryptionEncValuesSupported_WhenIdTokenEncryptionEncValuesSupportedIsNotNull_ReturnsSameOpenIdProviderConfigurationBuilder()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithIdTokenEncryptionEncValuesSupported(CreateIdTokenEncryptionEncValuesSupported(_fixture));

            Assert.That(result, Is.SameAs(sut));
        }
    }
}