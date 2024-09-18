using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.OpenIdProviderConfigurationBuilder
{
    [TestFixture]
    public class WithRequestObjectEncryptionAlgValuesSupportedTests : OpenIdProviderConfigurationBuilderTestBase
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
        public void WithRequestObjectEncryptionAlgValuesSupported_WhenRequestObjectEncryptionAlgValuesSupportedIsNull_ThrowsArgumentNullException()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithRequestObjectEncryptionAlgValuesSupported(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("requestObjectEncryptionAlgValuesSupported"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithRequestObjectEncryptionAlgValuesSupported_WhenRequestObjectEncryptionAlgValuesSupportedIsNotNull_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithRequestObjectEncryptionAlgValuesSupported(CreateRequestObjectEncryptionAlgValuesSupported(_fixture));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithRequestObjectEncryptionAlgValuesSupported_WhenRequestObjectEncryptionAlgValuesSupportedIsNotNull_ReturnsSameOpenIdProviderConfigurationBuilder()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithRequestObjectEncryptionAlgValuesSupported(CreateRequestObjectEncryptionAlgValuesSupported(_fixture));

            Assert.That(result, Is.SameAs(sut));
        }
    }
}