using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.OpenIdProviderConfigurationStaticValuesProvider
{
    [TestFixture]
    public class UserInfoSigningAlgValuesSupportedTests : OpenIdProviderConfigurationStaticValuesProviderTestBase
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
        public void UserInfoSigningAlgValuesSupported_WhenCalled_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.UserInfoSigningAlgValuesSupported;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void UserInfoSigningAlgValuesSupported_WhenCalled_ReturnsNotEmpty()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.UserInfoSigningAlgValuesSupported;

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void UserInfoSigningAlgValuesSupported_WhenCalled_ReturnsCollectionContainingOneSupportedUserInfoSigningAlgValue()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.UserInfoSigningAlgValuesSupported;

            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        public void UserInfoSigningAlgValuesSupported_WhenCalled_ReturnsCollectionContainingRS256AsSupportedUserInfoSigningAlgValue()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.UserInfoSigningAlgValuesSupported;

            Assert.That(HasRs256Algorithm(result), Is.True);
        }

        private IOpenIdProviderConfigurationStaticValuesProvider CreateSut()
        {
            return CreateSut(_supportedScopesProviderMock, _fixture);
        }
    }
}