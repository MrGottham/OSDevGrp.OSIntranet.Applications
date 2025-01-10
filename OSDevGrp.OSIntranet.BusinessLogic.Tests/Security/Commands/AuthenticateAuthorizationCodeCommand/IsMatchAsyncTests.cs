using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Commands.AuthenticateAuthorizationCodeCommand
{
    [TestFixture]
    public class IsMatchAsyncTests
    {
        #region Private variables

        private Mock<ISecurityRepository> _securityRepositoryMock;
        private Mock<ITrustedDomainResolver> _trustedDomainResolverMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _securityRepositoryMock = new Mock<ISecurityRepository>();
            _trustedDomainResolverMock = new Mock<ITrustedDomainResolver>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatchAsync_WhenAuthorizationDataIsNull_ThrowsArgumentNullException()
        {
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.CreateEndpoint());

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.IsMatchAsync(null, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("authorizationData"));
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatchAsync_WhenSecurityRepositoryIsNull_ThrowsArgumentNullException()
        {
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.CreateEndpoint());

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.IsMatchAsync(CreateAuthorizationData(), null, _trustedDomainResolverMock.Object));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("securityRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatchAsync_WhenTrustedDomainResolverIsNull_ThrowsArgumentNullException()
        {
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.CreateEndpoint());

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.IsMatchAsync(CreateAuthorizationData(), _securityRepositoryMock.Object, null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("trustedDomainResolver"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataDoesNotContainKeyForClientId_AssertGetClientSecretIdentityAsyncWasNotCalledOnSecurityRepository()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(containsClientId: false, clientSecretValue: clientSecret, redirectUriValue: redirectUri.AbsoluteUri);
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _securityRepositoryMock.Verify(m => m.GetClientSecretIdentityAsync(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataDoesNotContainKeyForClientId_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(containsClientId: false, clientSecretValue: clientSecret, redirectUriValue: redirectUri.AbsoluteUri);
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeyForClientIdWithoutValue_AssertGetClientSecretIdentityAsyncWasNotCalledOnSecurityRepository()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(hasClientIdValue: false, clientSecretValue: clientSecret, redirectUriValue: redirectUri.AbsoluteUri);
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _securityRepositoryMock.Verify(m => m.GetClientSecretIdentityAsync(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeyForClientIdWithoutValue_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(hasClientIdValue: false, clientSecretValue: clientSecret, redirectUriValue: redirectUri.AbsoluteUri);
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeyForClientIdWithNoneMatchingValue_AssertGetClientSecretIdentityAsyncWasNotCalledOnSecurityRepository()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: _fixture.Create<string>(), clientSecretValue: clientSecret, redirectUriValue: redirectUri.AbsoluteUri);
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _securityRepositoryMock.Verify(m => m.GetClientSecretIdentityAsync(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeyForClientIdWithNoneMatchingValue_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: _fixture.Create<string>(), clientSecretValue: clientSecret, redirectUriValue: redirectUri.AbsoluteUri);
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataDoesNotContainKeyForClientSecret_AssertGetClientSecretIdentityAsyncWasNotCalledOnSecurityRepository()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, containsClientSecret: false, redirectUriValue: redirectUri.AbsoluteUri);
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _securityRepositoryMock.Verify(m => m.GetClientSecretIdentityAsync(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataDoesNotContainKeyForClientSecret_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, containsClientSecret: false, redirectUriValue: redirectUri.AbsoluteUri);
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeyForClientSecretWithoutValue_AssertGetClientSecretIdentityAsyncWasNotCalledOnSecurityRepository()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, hasClientSecretValue: false, redirectUriValue: redirectUri.AbsoluteUri);
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _securityRepositoryMock.Verify(m => m.GetClientSecretIdentityAsync(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeyForClientSecretWithoutValue_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, hasClientSecretValue: false, redirectUriValue: redirectUri.AbsoluteUri);
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeyForClientSecretWithNoneMatchingValue_AssertGetClientSecretIdentityAsyncWasNotCalledOnSecurityRepository()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, clientSecretValue: _fixture.Create<string>(), redirectUriValue: redirectUri.AbsoluteUri);
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _securityRepositoryMock.Verify(m => m.GetClientSecretIdentityAsync(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeyForClientSecretWithNoneMatchingValue_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, clientSecretValue: _fixture.Create<string>(), redirectUriValue: redirectUri.AbsoluteUri);
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataDoesNotContainKeyForRedirectUri_AssertGetClientSecretIdentityAsyncWasNotCalledOnSecurityRepository()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, clientSecretValue: clientSecret, containsRedirectUri: false);
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _securityRepositoryMock.Verify(m => m.GetClientSecretIdentityAsync(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataDoesNotContainKeyForRedirectUri_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, clientSecretValue: clientSecret, containsRedirectUri: false);
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeyForRedirectUriWithoutValue_AssertGetClientSecretIdentityAsyncWasNotCalledOnSecurityRepository()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, clientSecretValue: clientSecret, hasRedirectUriValue: false);
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _securityRepositoryMock.Verify(m => m.GetClientSecretIdentityAsync(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeyForRedirectUriWithoutValue_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, clientSecretValue: clientSecret, hasRedirectUriValue: false);
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeyForRedirectUriWithNonAbsoluteUriValue_AssertGetClientSecretIdentityAsyncWasNotCalledOnSecurityRepository()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, clientSecretValue: clientSecret, redirectUriValue: _fixture.Create<string>());
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _securityRepositoryMock.Verify(m => m.GetClientSecretIdentityAsync(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeyForRedirectUriWithNonAbsoluteUriValue_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, clientSecretValue: clientSecret, redirectUriValue: _fixture.Create<string>());
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeyForRedirectUriWithNoneMatchingValue_AssertGetClientSecretIdentityAsyncWasNotCalledOnSecurityRepository()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, clientSecretValue: clientSecret, redirectUriValue: _fixture.CreateEndpointString());
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _securityRepositoryMock.Verify(m => m.GetClientSecretIdentityAsync(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeyForRedirectUriWithNoneMatchingValue_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, clientSecretValue: clientSecret, redirectUriValue: _fixture.CreateEndpointString());
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeysWithMatchingValues_AssertGetClientSecretIdentityAsyncWasCalledOnSecurityRepositoryWithClientIdFromAuthenticateAuthorizationCodeCommand()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, clientSecretValue: clientSecret, redirectUriValue: redirectUri.AbsoluteUri);
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _securityRepositoryMock.Verify(m => m.GetClientSecretIdentityAsync(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, clientId) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeysWithMatchingValuesButClientSecretIdentityCouldNotBeResolveBySecurityRepository_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri, hasClientSecretIdentity: false);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, clientSecretValue: clientSecret, redirectUriValue: redirectUri.AbsoluteUri);
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeysWithMatchingValuesAndClientSecretIdentityCouldBeResolveBySecurityRepository_AssertClientSecretWasCalledOnClientSecretIdentityResolvedBySecurityRepository()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret);
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri, clientSecretIdentity: clientSecretIdentityMock.Object);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, clientSecretValue: clientSecret, redirectUriValue: redirectUri.AbsoluteUri);
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            clientSecretIdentityMock.Verify(m => m.ClientSecret, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeysWithMatchingValuesAndNoneMatchingClientSecretIdentityWasResolvedBySecurityRepository_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock(clientSecret: _fixture.Create<string>()).Object;
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri, clientSecretIdentity: clientSecretIdentity);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, clientSecretValue: clientSecret, redirectUriValue: redirectUri.AbsoluteUri);
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeysWithMatchingValuesAndMatchingClientSecretIdentityWasResolvedBySecurityRepository_AssertIsTrustedDomainWasCalledOnTrustedDomainResolverWithRedirectUriFromAuthenticateAuthorizationCodeCommand()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret).Object;
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri, clientSecretIdentity: clientSecretIdentity);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, clientSecretValue: clientSecret, redirectUriValue: redirectUri.AbsoluteUri);
            await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && value == redirectUri)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataDoesNotContainKeyForClientId_ReturnsFalse()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(containsClientId: false, clientSecretValue: clientSecret, redirectUriValue: redirectUri.AbsoluteUri);
            bool result = await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeyForClientIdWithoutValue_ReturnsFalse()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(hasClientIdValue: false, clientSecretValue: clientSecret, redirectUriValue: redirectUri.AbsoluteUri);
            bool result = await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeyForClientIdWithNoneMatchingValue_ReturnsFalse()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: _fixture.Create<string>(), clientSecretValue: clientSecret, redirectUriValue: redirectUri.AbsoluteUri);
            bool result = await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataDoesNotContainKeyForClientSecret_ReturnsFalse()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, containsClientSecret: false, redirectUriValue: redirectUri.AbsoluteUri);
            bool result = await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeyForClientSecretWithNoneMatchingValue_ReturnsFalse()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, clientSecretValue: _fixture.Create<string>(), redirectUriValue: redirectUri.AbsoluteUri);
            bool result = await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeyForClientSecretWithoutValue_ReturnsFalse()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, hasClientSecretValue: false, redirectUriValue: redirectUri.AbsoluteUri);
            bool result = await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataDoesNotContainKeyForRedirectUri_ReturnsFalse()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, clientSecretValue: clientSecret, containsRedirectUri: false);
            bool result = await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeyForRedirectUriWithoutValue_ReturnsFalse()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, clientSecretValue: clientSecret, hasRedirectUriValue: false);
            bool result = await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeyForRedirectUriWithNonAbsoluteUriValue_ReturnsFalse()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, clientSecretValue: clientSecret, redirectUriValue: _fixture.Create<string>());
            bool result = await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeyForRedirectUriWithNoneMatchingValue_ReturnsFalse()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, clientSecretValue: clientSecret, redirectUriValue: _fixture.CreateEndpointString());
            bool result = await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeysWithMatchingValuesButClientSecretIdentityCouldNotBeResolveBySecurityRepository_ReturnsFalse()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri, hasClientSecretIdentity: false);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, clientSecretValue: clientSecret, redirectUriValue: redirectUri.AbsoluteUri);
            bool result = await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeysWithMatchingValuesAndNoneMatchingClientSecretIdentityWasResolvedBySecurityRepository_ReturnsFalse()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock(clientSecret: _fixture.Create<string>()).Object;
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri, clientSecretIdentity: clientSecretIdentity);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, clientSecretValue: clientSecret, redirectUriValue: redirectUri.AbsoluteUri);
            bool result = await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task IsMatchAsync_WhenAuthorizationDataContainsKeysWithMatchingValuesAndMatchingClientSecretIdentityWasResolvedBySecurityRepository_ReturnResultFromIsTrustedDomainOnTrustedDomainResolver(bool isTrustedDomain)
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = _fixture.CreateEndpoint();
            IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret).Object;
            IAuthenticateAuthorizationCodeCommand sut = CreateSut(clientId, clientSecret, redirectUri, clientSecretIdentity: clientSecretIdentity, isTrustedDomain: isTrustedDomain);

            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationData(clientIdValue: clientId, clientSecretValue: clientSecret, redirectUriValue: redirectUri.AbsoluteUri);
            bool result = await sut.IsMatchAsync(authorizationData, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object);

            Assert.That(result, Is.EqualTo(isTrustedDomain));
        }

        private IAuthenticateAuthorizationCodeCommand CreateSut(string clientId, string clientSecret, Uri redirectUri, bool hasClientSecretIdentity = true, IClientSecretIdentity clientSecretIdentity = null, bool isTrustedDomain = true)
        {
            NullGuard.NotNullOrWhiteSpace(clientId, nameof(clientId))
                .NotNullOrWhiteSpace(clientSecret, nameof(clientSecret))
                .NotNull(redirectUri, nameof(redirectUri));

            _securityRepositoryMock.Setup(m => m.GetClientSecretIdentityAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(hasClientSecretIdentity ? clientSecretIdentity ?? _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret).Object : null));

            _trustedDomainResolverMock.Setup(m => m.IsTrustedDomain(It.IsAny<Uri>()))
                .Returns(isTrustedDomain);

            return new BusinessLogic.Security.Commands.AuthenticateAuthorizationCodeCommand(
                _fixture.Create<string>(),
                clientId ?? _fixture.Create<string>(),
                clientSecret ?? _fixture.Create<string>(),
                redirectUri ?? _fixture.CreateEndpoint(),
                _ => { },
                Array.Empty<Claim>(),
                _fixture.Create<string>(),
                new Dictionary<string, string>(),
                value => value);
        }

        private IReadOnlyDictionary<string, string> CreateAuthorizationData(bool containsClientId = true, bool hasClientIdValue = true, string clientIdValue = null, bool containsClientSecret = true, bool hasClientSecretValue = true, string clientSecretValue = null, bool containsRedirectUri = true, bool hasRedirectUriValue = true, string redirectUriValue = null)
        {
            IDictionary<string, string> authorizationData = new ConcurrentDictionary<string, string>();
            if (containsClientId)
            {
                authorizationData.Add(AuthorizationDataConverter.ClientIdKey, hasClientIdValue ? clientIdValue ?? _fixture.Create<string>() : null);
            }
            if (containsClientSecret)
            {
                authorizationData.Add(AuthorizationDataConverter.ClientSecretKey, hasClientSecretValue ? clientSecretValue ?? _fixture.Create<string>() : null);
            }
            if (containsRedirectUri)
            {
                authorizationData.Add(AuthorizationDataConverter.RedirectUriKey, hasRedirectUriValue ? redirectUriValue ?? _fixture.CreateEndpointString() : null);
            }
            return authorizationData.AsReadOnly();
        }
    }
}