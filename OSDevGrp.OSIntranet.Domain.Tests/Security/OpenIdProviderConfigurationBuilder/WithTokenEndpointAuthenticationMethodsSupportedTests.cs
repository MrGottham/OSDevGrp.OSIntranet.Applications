using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.OpenIdProviderConfigurationBuilder
{
    [TestFixture]
    public class WithTokenEndpointAuthenticationMethodsSupportedTests : OpenIdProviderConfigurationBuilderTestBase
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
        public void WithTokenEndpointAuthenticationMethodsSupported_WhenTokenEndpointAuthenticationMethodsSupportedIsNull_ThrowsArgumentNullException()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithTokenEndpointAuthenticationMethodsSupported(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("tokenEndpointAuthenticationMethodsSupported"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithTokenEndpointAuthenticationMethodsSupported_WhenTokenEndpointAuthenticationMethodsSupportedIsNotNull_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithTokenEndpointAuthenticationMethodsSupported(CreateTokenEndpointAuthenticationMethodsSupported(_fixture));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithTokenEndpointAuthenticationMethodsSupported_WhenTokenEndpointAuthenticationMethodsSupportedIsNotNull_ReturnsSameOpenIdProviderConfigurationBuilder()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithTokenEndpointAuthenticationMethodsSupported(CreateTokenEndpointAuthenticationMethodsSupported(_fixture));

            Assert.That(result, Is.SameAs(sut));
        }
    }
}