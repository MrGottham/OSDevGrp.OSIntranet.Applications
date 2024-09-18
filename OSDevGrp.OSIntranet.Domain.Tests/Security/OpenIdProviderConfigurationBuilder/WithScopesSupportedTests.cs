using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.OpenIdProviderConfigurationBuilder
{
    [TestFixture]
    public class WithScopesSupportedTests : OpenIdProviderConfigurationBuilderTestBase
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
        public void WithScopesSupported_WhenScopesSupportedIsNull_ThrowsArgumentNullException()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithScopesSupported(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("scopesSupported"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithScopesSupported_WhenScopesSupportedIsNotNull_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithScopesSupported(CreateScopesSupported(_fixture));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithScopesSupported_WhenScopesSupportedIsNotNull_ReturnsSameOpenIdProviderConfigurationBuilder()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithScopesSupported(CreateScopesSupported(_fixture));

            Assert.That(result, Is.SameAs(sut));
        }
    }
}