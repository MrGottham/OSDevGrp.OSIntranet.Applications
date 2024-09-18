using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.OpenIdProviderConfigurationStaticValuesProvider
{
    [TestFixture]
    public class ScopesSupportedTests : OpenIdProviderConfigurationStaticValuesProviderTestBase
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
        public void ScopesSupported_WhenCalled_AssertSupportedScopesWasCalledOnSupportedScopesProvider()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyDictionary<string, IScope> _ = sut.ScopesSupported;

            _supportedScopesProviderMock.Verify(m => m.SupportedScopes, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ScopesSupported_WhenCalledCalledMultipleTimes_AssertSupportedScopesWasCalledOnlyOnceOnSupportedScopesProvider()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            for (int i = 0; i < 5; i++)
            {
                IReadOnlyDictionary<string, IScope> _ = sut.ScopesSupported;
            }

            _supportedScopesProviderMock.Verify(m => m.SupportedScopes, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ScopesSupported_WhenCalled_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyDictionary<string, IScope> result = sut.ScopesSupported;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ScopesSupported_WhenCalled_ReturnsNotEmpty()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyDictionary<string, IScope> result = sut.ScopesSupported;

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ScopesSupported_WhenCalled_ReturnsSupportedScopesFromSupportedScopesProvider()
        {
            IDictionary<string, IScope> supportedScopes = new Dictionary<string, IScope>
            {
                {_fixture.Create<string>(), _fixture.BuildScopeMock().Object},
                {_fixture.Create<string>(), _fixture.BuildScopeMock().Object},
                {_fixture.Create<string>(), _fixture.BuildScopeMock().Object}
            };
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut(supportedScopes);

            IReadOnlyDictionary<string, IScope> result = sut.ScopesSupported;

            Assert.That(result, Is.EqualTo(supportedScopes));
        }

        private IOpenIdProviderConfigurationStaticValuesProvider CreateSut(IDictionary<string, IScope> supportedScopes = null)
        {
            return CreateSut(_supportedScopesProviderMock, _fixture, supportedScopes);
        }
    }
}