using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.OpenIdProviderConfigurationStaticValuesProvider
{
    [TestFixture]
    public class ResponseTypesSupportedTests : OpenIdProviderConfigurationStaticValuesProviderTestBase
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
        public void ResponseTypesSupported_WhenCalled_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.ResponseTypesSupported;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ResponseTypesSupported_WhenCalled_ReturnsNotEmpty()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.ResponseTypesSupported;

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ResponseTypesSupported_WhenCalled_ReturnsCollectionContainingThreeSupportedResponseTypes()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.ResponseTypesSupported;

            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test]
        [Category("UnitTest")]
        public void ResponseTypesSupported_WhenCalled_ReturnsCollectionContainingCodeAsSupportedResponseType()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.ResponseTypesSupported;

            Assert.That(result.Contains("code"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ResponseTypesSupported_WhenCalled_ReturnsCollectionContainingCodeIdTokenAsSupportedResponseType()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.ResponseTypesSupported;

            Assert.That(result.Contains("code id_token"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ResponseTypesSupported_WhenCalled_ReturnsCollectionContainingIdTokenAsSupportedResponseType()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.ResponseTypesSupported;

            Assert.That(result.Contains("id_token"), Is.True);
        }

        private IOpenIdProviderConfigurationStaticValuesProvider CreateSut()
        {
            return CreateSut(_supportedScopesProviderMock, _fixture);
        }
    }
}