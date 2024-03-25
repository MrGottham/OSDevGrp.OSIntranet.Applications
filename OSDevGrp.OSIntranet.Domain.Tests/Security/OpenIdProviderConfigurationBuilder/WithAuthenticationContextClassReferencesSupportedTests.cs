using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.OpenIdProviderConfigurationBuilder
{
    [TestFixture]
    public class WithAuthenticationContextClassReferencesSupportedTests : OpenIdProviderConfigurationBuilderTestBase
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
        public void WithAuthenticationContextClassReferencesSupported_WhenAuthenticationContextClassReferencesSupportedIsNull_ThrowsArgumentNullException()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithAuthenticationContextClassReferencesSupported(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("authenticationContextClassReferencesSupported"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithAuthenticationContextClassReferencesSupported_WhenAuthenticationContextClassReferencesSupportedIsNotNull_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithAuthenticationContextClassReferencesSupported(CreateAuthenticationContextClassReferencesSupported(_fixture));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithAuthenticationContextClassReferencesSupported_WhenAuthenticationContextClassReferencesSupportedIsNotNull_ReturnsSameOpenIdProviderConfigurationBuilder()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithAuthenticationContextClassReferencesSupported(CreateAuthenticationContextClassReferencesSupported(_fixture));

            Assert.That(result, Is.SameAs(sut));
        }
    }
}