using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.OpenIdProviderConfigurationBuilder
{
    [TestFixture]
    public class WithRegistrationTermsOfServiceEndpointTests : OpenIdProviderConfigurationBuilderTestBase
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
        public void WithRegistrationTermsOfServiceEndpoint_WhenRegistrationTermsOfServiceEndpointIsNull_ThrowsArgumentNullException()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithRegistrationTermsOfServiceEndpoint(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("registrationTermsOfServiceEndpoint"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithRegistrationTermsOfServiceEndpoint_WhenRegistrationTermsOfServiceEndpointIsNotNull_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithRegistrationTermsOfServiceEndpoint(CreateRegistrationTermsOfServiceEndpoint(_fixture));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithRegistrationTermsOfServiceEndpoint_WhenRegistrationTermsOfServiceEndpointIsNotNull_ReturnsSameOpenIdProviderConfigurationBuilder()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithRegistrationTermsOfServiceEndpoint(CreateRegistrationTermsOfServiceEndpoint(_fixture));

            Assert.That(result, Is.SameAs(sut));
        }
    }
}