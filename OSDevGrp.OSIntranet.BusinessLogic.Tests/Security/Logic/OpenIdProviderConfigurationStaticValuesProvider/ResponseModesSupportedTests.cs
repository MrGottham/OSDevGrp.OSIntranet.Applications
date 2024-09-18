using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.OpenIdProviderConfigurationStaticValuesProvider
{
    [TestFixture]
    public class ResponseModesSupportedTests : OpenIdProviderConfigurationStaticValuesProviderTestBase
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
        public void ResponseModesSupported_WhenCalled_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.ResponseModesSupported;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ResponseModesSupported_WhenCalled_ReturnsNotEmpty()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.ResponseModesSupported;

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ResponseModesSupported_WhenCalled_ReturnsCollectionContainingOneSupportedResponseMode()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.ResponseModesSupported;

            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        public void ResponseModesSupported_WhenCalled_ReturnsCollectionContainingQueryAsSupportedResponseMode()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.ResponseModesSupported;

            Assert.That(result.Contains("query"), Is.True);
        }

        private IOpenIdProviderConfigurationStaticValuesProvider CreateSut()
        {
            return CreateSut(_supportedScopesProviderMock, _fixture);
        }
    }
}