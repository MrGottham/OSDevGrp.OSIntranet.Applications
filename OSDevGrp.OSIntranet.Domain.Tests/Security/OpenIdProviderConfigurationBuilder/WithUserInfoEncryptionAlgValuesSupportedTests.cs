using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.OpenIdProviderConfigurationBuilder
{
    [TestFixture]
    public class WithUserInfoEncryptionAlgValuesSupportedTests : OpenIdProviderConfigurationBuilderTestBase
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
        public void WithUserInfoEncryptionAlgValuesSupported_WhenUserInfoEncryptionAlgValuesSupportedIsNull_ThrowsArgumentNullException()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithUserInfoEncryptionAlgValuesSupported(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("userInfoEncryptionAlgValuesSupported"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithUserInfoEncryptionAlgValuesSupported_WhenUserInfoEncryptionAlgValuesSupportedIsNotNull_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithUserInfoEncryptionAlgValuesSupported(CreateUserInfoEncryptionAlgValuesSupported(_fixture));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithUserInfoEncryptionAlgValuesSupported_WhenUserInfoEncryptionAlgValuesSupportedIsNotNull_ReturnsSameOpenIdProviderConfigurationBuilder()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithUserInfoEncryptionAlgValuesSupported(CreateUserInfoEncryptionAlgValuesSupported(_fixture));

            Assert.That(result, Is.SameAs(sut));
        }
    }
}