using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.OpenIdProviderConfigurationStaticValuesProvider
{
    [TestFixture]
    public class ClaimTypesSupportedTests : OpenIdProviderConfigurationStaticValuesProviderTestBase
    {
        #region Private variables

        private Mock<ISupportedScopesProvider> _supportedScopesProviderMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _supportedScopesProviderMock = new Mock<ISupportedScopesProvider>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ClaimTypesSupported_WhenCalled_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.ClaimTypesSupported;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ClaimTypesSupported_WhenCalled_ReturnsNotEmpty()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.ClaimTypesSupported;

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ClaimTypesSupported_WhenCalled_ReturnsCollectionContainingTwoSupportedClaimTypes()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.ClaimTypesSupported;

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        [Category("UnitTest")]
        public void ClaimTypesSupported_WhenCalled_ReturnsCollectionContainingNormalAsSupportedClaimType()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.ClaimTypesSupported;

            Assert.That(result.Contains("normal"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ClaimTypesSupported_WhenCalled_ReturnsCollectionContainingAggregatedAsSupportedClaimType()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.ClaimTypesSupported;

            Assert.That(result.Contains("aggregated"), Is.True);
        }

        private IOpenIdProviderConfigurationStaticValuesProvider CreateSut()
        {
            return CreateSut(_supportedScopesProviderMock, _fixture);
        }
    }
}