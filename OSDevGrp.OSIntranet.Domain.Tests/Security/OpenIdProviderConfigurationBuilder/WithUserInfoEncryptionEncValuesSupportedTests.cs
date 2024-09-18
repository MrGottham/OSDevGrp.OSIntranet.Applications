using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.OpenIdProviderConfigurationBuilder
{
    [TestFixture]
    public class WithUserInfoEncryptionEncValuesSupportedTests : OpenIdProviderConfigurationBuilderTestBase
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
        public void WithUserInfoEncryptionEncValuesSupported_WhenUserInfoEncryptionEncValuesSupportedIsNull_ThrowsArgumentNullException()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithUserInfoEncryptionEncValuesSupported(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("userInfoEncryptionEncValuesSupported"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithUserInfoEncryptionEncValuesSupported_WhenUserInfoEncryptionEncValuesSupportedIsNotNull_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithUserInfoEncryptionEncValuesSupported(CreateUserInfoEncryptionEncValuesSupported(_fixture));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithUserInfoEncryptionEncValuesSupported_WhenUserInfoEncryptionEncValuesSupportedIsNotNull_ReturnsSameOpenIdProviderConfigurationBuilder()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithUserInfoEncryptionEncValuesSupported(CreateUserInfoEncryptionEncValuesSupported(_fixture));

            Assert.That(result, Is.SameAs(sut));
        }
    }
}