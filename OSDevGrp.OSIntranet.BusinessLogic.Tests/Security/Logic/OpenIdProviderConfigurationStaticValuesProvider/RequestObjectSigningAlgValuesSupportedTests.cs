using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.OpenIdProviderConfigurationStaticValuesProvider
{
    [TestFixture]
    public class RequestObjectSigningAlgValuesSupportedTests : OpenIdProviderConfigurationStaticValuesProviderTestBase
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
        public void RequestObjectSigningAlgValuesSupported_WhenCalled_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.RequestObjectSigningAlgValuesSupported;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void RequestObjectSigningAlgValuesSupported_WhenCalled_ReturnsNotEmpty()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.RequestObjectSigningAlgValuesSupported;

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void RequestObjectSigningAlgValuesSupported_WhenCalled_ReturnsCollectionContainingTwoSupportedRequestObjectSigningAlgValues()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.RequestObjectSigningAlgValuesSupported;

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        [Category("UnitTest")]
        public void RequestObjectSigningAlgValuesSupported_WhenCalled_ReturnsCollectionContainingRS256AsSupportedRequestObjectSigningAlgValue()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.RequestObjectSigningAlgValuesSupported;

            Assert.That(HasRs256Algorithm(result), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void RequestObjectSigningAlgValuesSupported_WhenCalled_ReturnsCollectionContainingNoneAsSupportedRequestObjectSigningAlgValue()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.RequestObjectSigningAlgValuesSupported;

            Assert.That(HasNoneAlgorithm(result), Is.True);
        }

        private IOpenIdProviderConfigurationStaticValuesProvider CreateSut()
        {
            return CreateSut(_supportedScopesProviderMock, _fixture);
        }
    }
}