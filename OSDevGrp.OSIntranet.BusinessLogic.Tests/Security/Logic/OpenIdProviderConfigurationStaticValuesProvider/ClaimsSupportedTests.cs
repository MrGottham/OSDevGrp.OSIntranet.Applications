using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.OpenIdProviderConfigurationStaticValuesProvider
{
    [TestFixture]
    public class ClaimsSupportedTests : OpenIdProviderConfigurationStaticValuesProviderTestBase
    {
        #region Private variables

        private Mock<ISupportedScopesProvider> _supportedScopesProviderMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _supportedScopesProviderMock = new Mock<ISupportedScopesProvider>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ClaimsSupported_WhenCalled_AssertSupportedScopesWasCalledOnSupportedScopesProvider()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> _ = sut.ClaimsSupported;

            _supportedScopesProviderMock.Verify(m => m.SupportedScopes, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ClaimsSupported_WhenCalled_AssertRelatedClaimsWasCalledOnEachSupportedScopesGivenBySupportedScopes()
        {
            Mock<IScope>[] scopeMockCollection = 
            [
                _fixture.BuildScopeMock(),
                _fixture.BuildScopeMock(),
                _fixture.BuildScopeMock()
            ];
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut(scopeMockCollection.ToDictionary(scopeMock => scopeMock.Object.Name, scopeMock => scopeMock.Object));

            IReadOnlyCollection<string> _ = sut.ClaimsSupported;

            foreach (Mock<IScope> scopeMock in scopeMockCollection)
            {
                scopeMock.Verify(m => m.RelatedClaims, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void ClaimsSupported_WhenCalledCalledMultipleTimes_AssertSupportedScopesWasCalledOnlyOnceOnSupportedScopesProvider()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            for (int i = 0; i < 5; i++)
            {
                IReadOnlyCollection<string> _ = sut.ClaimsSupported;
            }

            _supportedScopesProviderMock.Verify(m => m.SupportedScopes, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ClaimsSupported_WhenCalled_AssertRelatedClaimsWasCalledOnlyOnceOnEachSupportedScopesGivenBySupportedScopes()
        {
            Mock<IScope>[] scopeMockCollection =
            [
                _fixture.BuildScopeMock(),
                _fixture.BuildScopeMock(),
                _fixture.BuildScopeMock()
            ];
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut(scopeMockCollection.ToDictionary(scopeMock => scopeMock.Object.Name, scopeMock => scopeMock.Object));

            for (int i = 0; i < 5; i++)
            {
                IReadOnlyCollection<string> _ = sut.ClaimsSupported;
            }

            foreach (Mock<IScope> scopeMock in scopeMockCollection)
            {
                scopeMock.Verify(m => m.RelatedClaims, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void ClaimsSupported_WhenCalled_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.ClaimsSupported;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ClaimsSupported_WhenCalled_ReturnsNotEmpty()
        {
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut();

            IReadOnlyCollection<string> result = sut.ClaimsSupported;

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ClaimsSupported_WhenCalled_ReturnsRelatedClaimsFromEachSupportedScopesGivenBySupportedScopesProvider()
        {
            string[] relatedClaimsSet1 = _fixture.CreateMany<string>(_random.Next(1, 10)).ToArray();
            string[] relatedClaimsSet2 = _fixture.CreateMany<string>(_random.Next(1, 10)).ToArray();
            string[] relatedClaimsSet3 = _fixture.CreateMany<string>(_random.Next(1, 10)).ToArray();
            IDictionary<string, IScope> supportedScopes = new Dictionary<string, IScope>
            {
                {_fixture.Create<string>(), _fixture.BuildScopeMock(relatedClaims: relatedClaimsSet1).Object},
                {_fixture.Create<string>(), _fixture.BuildScopeMock(relatedClaims: relatedClaimsSet2).Object},
                {_fixture.Create<string>(), _fixture.BuildScopeMock(relatedClaims: relatedClaimsSet3).Object}
            };
            IOpenIdProviderConfigurationStaticValuesProvider sut = CreateSut(supportedScopes);

            IReadOnlyCollection<string> result = sut.ClaimsSupported;

            Assert.That(result, Is.EqualTo(relatedClaimsSet1.Concat(relatedClaimsSet2).Concat(relatedClaimsSet3)));
        }

        private IOpenIdProviderConfigurationStaticValuesProvider CreateSut(IDictionary<string, IScope> supportedScopes = null)
        {
            return CreateSut(_supportedScopesProviderMock, _fixture, supportedScopes);
        }
    }
}