using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.OpenIdProviderConfigurationBuilder
{
    [TestFixture]
    public class WithRegistrationPolicyEndpointTests : OpenIdProviderConfigurationBuilderTestBase
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
        public void WithRegistrationPolicyEndpoint_WhenRegistrationPolicyEndpointIsNull_ThrowsArgumentNullException()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithRegistrationPolicyEndpoint(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("registrationPolicyEndpoint"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithRegistrationPolicyEndpoint_WhenRegistrationPolicyEndpointIsNotNull_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithRegistrationPolicyEndpoint(CreateRegistrationPolicyEndpoint(_fixture));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithRegistrationPolicyEndpoint_WhenRegistrationPolicyEndpointIsNotNull_ReturnsSameOpenIdProviderConfigurationBuilder()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithRegistrationPolicyEndpoint(CreateRegistrationPolicyEndpoint(_fixture));

            Assert.That(result, Is.SameAs(sut));
        }
    }
}