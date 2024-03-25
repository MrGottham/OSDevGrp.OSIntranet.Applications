using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.OpenIdProviderConfigurationBuilder
{
    [TestFixture]
    public class WithTokenEndpointAuthenticationSigningAlgValuesSupportedTests : OpenIdProviderConfigurationBuilderTestBase
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
        public void WithTokenEndpointAuthenticationSigningAlgValuesSupported_WhenTokenEndpointAuthenticationSigningAlgValuesSupportedIsNull_ThrowsArgumentNullException()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithTokenEndpointAuthenticationSigningAlgValuesSupported(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("tokenEndpointAuthenticationSigningAlgValuesSupported"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithTokenEndpointAuthenticationSigningAlgValuesSupported_WhenTokenEndpointAuthenticationSigningAlgValuesSupportedIsNotNull_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithTokenEndpointAuthenticationSigningAlgValuesSupported(CreateTokenEndpointAuthenticationSigningAlgValuesSupported(_fixture));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithTokenEndpointAuthenticationSigningAlgValuesSupported_WhenTokenEndpointAuthenticationSigningAlgValuesSupportedIsNotNull_ReturnsSameOpenIdProviderConfigurationBuilder()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithTokenEndpointAuthenticationSigningAlgValuesSupported(CreateTokenEndpointAuthenticationSigningAlgValuesSupported(_fixture));

            Assert.That(result, Is.SameAs(sut));
        }
    }
}