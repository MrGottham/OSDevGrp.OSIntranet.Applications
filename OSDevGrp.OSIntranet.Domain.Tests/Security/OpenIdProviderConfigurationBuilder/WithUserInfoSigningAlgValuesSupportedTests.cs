using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.OpenIdProviderConfigurationBuilder
{
    [TestFixture]
    public class WithUserInfoSigningAlgValuesSupportedTests : OpenIdProviderConfigurationBuilderTestBase
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
        public void WithUserInfoSigningAlgValuesSupported_WhenUserInfoSigningAlgValuesSupportedIsNull_ThrowsArgumentNullException()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithUserInfoSigningAlgValuesSupported(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("userInfoSigningAlgValuesSupported"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithUserInfoSigningAlgValuesSupported_WhenUserInfoSigningAlgValuesSupportedIsNotNull_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithUserInfoSigningAlgValuesSupported(CreateUserInfoSigningAlgValuesSupported(_fixture));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithUserInfoSigningAlgValuesSupported_WhenUserInfoSigningAlgValuesSupportedIsNotNull_ReturnsSameOpenIdProviderConfigurationBuilder()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithUserInfoSigningAlgValuesSupported(CreateUserInfoSigningAlgValuesSupported(_fixture));

            Assert.That(result, Is.SameAs(sut));
        }
    }
}