using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.OpenIdProviderConfigurationBuilder
{
    [TestFixture]
    public class WithRequestObjectEncryptionEncValuesSupportedTests : OpenIdProviderConfigurationBuilderTestBase
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
        public void WithRequestObjectEncryptionEncValuesSupported_WhenRequestObjectEncryptionEncValuesSupportedIsNull_ThrowsArgumentNullException()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithRequestObjectEncryptionEncValuesSupported(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("requestObjectEncryptionEncValuesSupported"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithRequestObjectEncryptionEncValuesSupported_WhenRequestObjectEncryptionEncValuesSupportedIsNotNull_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithRequestObjectEncryptionEncValuesSupported(CreateRequestObjectEncryptionEncValuesSupported(_fixture));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithRequestObjectEncryptionEncValuesSupported_WhenRequestObjectEncryptionEncValuesSupportedIsNotNull_ReturnsSameOpenIdProviderConfigurationBuilder()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithRequestObjectEncryptionEncValuesSupported(CreateRequestObjectEncryptionEncValuesSupported(_fixture));

            Assert.That(result, Is.SameAs(sut));
        }
    }
}