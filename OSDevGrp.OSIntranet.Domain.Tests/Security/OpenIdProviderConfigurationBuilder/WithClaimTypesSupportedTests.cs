using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.OpenIdProviderConfigurationBuilder
{
    [TestFixture]
    public class WithClaimTypesSupportedTests : OpenIdProviderConfigurationBuilderTestBase
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
        public void WithClaimTypesSupported_WhenClaimTypesSupportedIsNull_ThrowsArgumentNullException()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithClaimTypesSupported(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("claimTypesSupported"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithClaimTypesSupported_WhenClaimTypesSupportedIsNotNull_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithClaimTypesSupported(CreateClaimTypesSupported(_fixture));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithClaimTypesSupported_WhenClaimTypesSupportedIsNotNull_ReturnsSameOpenIdProviderConfigurationBuilder()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfigurationBuilder result = sut.WithClaimTypesSupported(CreateClaimTypesSupported(_fixture));

            Assert.That(result, Is.SameAs(sut));
        }
    }
}