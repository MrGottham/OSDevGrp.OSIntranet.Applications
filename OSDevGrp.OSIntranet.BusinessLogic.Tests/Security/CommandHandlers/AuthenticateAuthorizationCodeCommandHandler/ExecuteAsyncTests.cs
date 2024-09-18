using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.CommandHandlers.AuthenticateAuthorizationCodeCommandHandler
{
    [TestFixture]
    public class ExecuteAsyncTests
    {
        #region Private variables

        private Mock<ISecurityRepository> _securityRepositoryMock;
        private Mock<ICommonRepository> _commonRepositoryMock;
        private Mock<IAuthorizationDataConverter> _authorizationDataConverterMock;
        private Mock<ITrustedDomainResolver> _trustedDomainResolverMock;
        private Mock<IExternalTokenClaimCreator> _externalTokenClaimCreatorMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _securityRepositoryMock = new Mock<ISecurityRepository>();
            _commonRepositoryMock = new Mock<ICommonRepository>();
            _authorizationDataConverterMock = new Mock<IAuthorizationDataConverter>();
            _trustedDomainResolverMock = new Mock<ITrustedDomainResolver>();
            _externalTokenClaimCreatorMock = new Mock<IExternalTokenClaimCreator>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenAuthenticateAuthorizationCodeCommandIsNull_ThrowsArgumentNullException()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("authenticateCommand"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertAuthorizationCodeWasCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.AuthorizationCode, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertPullKeyValueEntryAsyncWasCalledOnCommonRepositoryWithAuthorizationCodeFromAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            string authorizationCode = _fixture.Create<string>();
            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand(authorizationCode: authorizationCode);
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _commonRepositoryMock.Verify(m => m.PullKeyValueEntryAsync(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, authorizationCode) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenKeyValueEntryCouldNotBeResolvedForAuthorizationCode_AssertDeleteKeyValueEntryAsyncWasNotCalledOnCommonRepository()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasKeyValueEntryForAuthorizationCode: false);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _commonRepositoryMock.Verify(m => m.DeleteKeyValueEntryAsync(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenKeyValueEntryCouldNotBeResolvedForAuthorizationCode_AssertToAuthorizationCodeAsyncWasNotCalledOnAuthorizationDataConverter()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasKeyValueEntryForAuthorizationCode: false);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _authorizationDataConverterMock.Verify(m => m.ToAuthorizationCodeAsync(
                    It.IsAny<IKeyValueEntry>(),
                    out It.Ref<IReadOnlyCollection<Claim>>.IsAny,
                    out It.Ref<IReadOnlyDictionary<string, string>>.IsAny),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenKeyValueEntryCouldNotBeResolvedForAuthorizationCode_AssertClientIdWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasKeyValueEntryForAuthorizationCode: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.ClientId, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenKeyValueEntryCouldNotBeResolvedForAuthorizationCode_AssertClientSecretWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasKeyValueEntryForAuthorizationCode: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.ClientSecret, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenKeyValueEntryCouldNotBeResolvedForAuthorizationCode_AssertRedirectUriWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasKeyValueEntryForAuthorizationCode: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.RedirectUri, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenKeyValueEntryCouldNotBeResolvedForAuthorizationCode_AssertIsMatchAsyncWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasKeyValueEntryForAuthorizationCode: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.IsMatchAsync(
                It.IsAny<IReadOnlyDictionary<string, string>>(), 
                It.IsAny<ISecurityRepository>(),
                It.IsAny<ITrustedDomainResolver>()), 
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenKeyValueEntryCouldNotBeResolvedForAuthorizationCode_AssertClaimsWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasKeyValueEntryForAuthorizationCode: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.Claims, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenKeyValueEntryCouldNotBeResolvedForAuthorizationCode_AssertAuthenticationSessionItemsWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasKeyValueEntryForAuthorizationCode: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.AuthenticationSessionItems, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenKeyValueEntryCouldNotBeResolvedForAuthorizationCode_AssertProtectorWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasKeyValueEntryForAuthorizationCode: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.Protector, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenKeyValueEntryCouldNotBeResolvedForAuthorizationCode_AssertCanBuildWasNotCalledOnExternalTokenClaimCreator()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasKeyValueEntryForAuthorizationCode: false);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _externalTokenClaimCreatorMock.Verify(m => m.CanBuild(It.IsAny<IReadOnlyDictionary<string, string>>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenKeyValueEntryCouldNotBeResolvedForAuthorizationCode_AssertBuildWasNotCalledOnExternalTokenClaimCreator()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasKeyValueEntryForAuthorizationCode: false);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _externalTokenClaimCreatorMock.Verify(m => m.Build(
                    It.IsAny<IReadOnlyDictionary<string, string>>(),
                    It.IsAny<Func<string, string>>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenKeyValueEntryCouldNotBeResolvedForAuthorizationCode_AssertAuthenticationTypeWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasKeyValueEntryForAuthorizationCode: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.AuthenticationType, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenKeyValueEntryCouldNotBeResolvedForAuthorizationCode_ReturnsNull()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasKeyValueEntryForAuthorizationCode: false);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            ClaimsPrincipal result = await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenKeyValueEntryCouldBeResolvedForAuthorizationCode_AssertDeleteKeyValueEntryAsyncWasCalledOnCommonRepositoryWithAuthorizationCodeFromAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            string authorizationCode = _fixture.Create<string>();
            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand(authorizationCode: authorizationCode);
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _commonRepositoryMock.Verify(m => m.DeleteKeyValueEntryAsync(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, authorizationCode) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenKeyValueEntryCouldBeResolvedForAuthorizationCode_AssertToAuthorizationCodeAsyncWasCalledOnAuthorizationDataConverterWithKeyValueEntryResolvedByCommonRepository()
        {
            IKeyValueEntry keyValueEntryForAuthorizationCode = _fixture.BuildKeyValueEntryMock<object>().Object;
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(keyValueEntryForAuthorizationCode: keyValueEntryForAuthorizationCode);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _authorizationDataConverterMock.Verify(m => m.ToAuthorizationCodeAsync(
                    It.Is<IKeyValueEntry>(value => value != null && value == keyValueEntryForAuthorizationCode), 
                    out It.Ref<IReadOnlyCollection<Claim>>.IsAny,
                    out It.Ref<IReadOnlyDictionary<string, string>>.IsAny),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationCodeCouldNotBeResolvedByAuthorizationDataConverter_AssertClientIdWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasAuthorizationCode: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.ClientId, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationCodeCouldNotBeResolvedByAuthorizationDataConverter_AssertClientSecretWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasAuthorizationCode: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.ClientSecret, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationCodeCouldNotBeResolvedByAuthorizationDataConverter_AssertRedirectUriWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasAuthorizationCode: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.RedirectUri, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationCodeCouldNotBeResolvedByAuthorizationDataConverter_AssertIsMatchAsyncWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasAuthorizationCode: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.IsMatchAsync(
                    It.IsAny<IReadOnlyDictionary<string, string>>(),
                    It.IsAny<ISecurityRepository>(),
                    It.IsAny<ITrustedDomainResolver>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationCodeCouldNotBeResolvedByAuthorizationDataConverter_AssertClaimsWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasAuthorizationCode: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.Claims, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationCodeCouldNotBeResolvedByAuthorizationDataConverter_AssertAuthenticationSessionItemsWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasAuthorizationCode: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.AuthenticationSessionItems, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationCodeCouldNotBeResolvedByAuthorizationDataConverter_AssertProtectorWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasAuthorizationCode: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.Protector, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationCodeCouldNotBeResolvedByAuthorizationDataConverter_AssertCanBuildWasNotCalledOnExternalTokenClaimCreator()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasAuthorizationCode: false);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _externalTokenClaimCreatorMock.Verify(m => m.CanBuild(It.IsAny<IReadOnlyDictionary<string, string>>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationCodeCouldNotBeResolvedByAuthorizationDataConverter_AssertBuildWasNotCalledOnExternalTokenClaimCreator()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasAuthorizationCode: false);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _externalTokenClaimCreatorMock.Verify(m => m.Build(
                    It.IsAny<IReadOnlyDictionary<string, string>>(),
                    It.IsAny<Func<string, string>>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationCodeCouldNotBeResolvedByAuthorizationDataConverter_AssertAuthenticationTypeWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasAuthorizationCode: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.AuthenticationType, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationCodeCouldNotBeResolvedByAuthorizationDataConverter_ReturnsNull()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasAuthorizationCode: false);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            ClaimsPrincipal result = await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationCodeCouldBeResolvedByAuthorizationDataConverter_AssertExpiredWasCalledOnAuthorizationCodeCouldResolvedByAuthorizationDataConverter()
        {
            Mock<IAuthorizationCode> authorizationCodeMock = _fixture.BuildAuthorizationCodeMock();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationCode: authorizationCodeMock.Object);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            authorizationCodeMock.Verify(m => m.Expired, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationCodeResolvedByAuthorizationDataConverterHasExpired_AssertClientIdWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(expired: true).Object;
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationCode: authorizationCode);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.ClientId, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationCodeResolvedByAuthorizationDataConverterHasExpired_AssertClientSecretWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(expired: true).Object;
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationCode: authorizationCode);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.ClientSecret, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationCodeResolvedByAuthorizationDataConverterHasExpired_AssertRedirectUriWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(expired: true).Object;
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationCode: authorizationCode);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.RedirectUri, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationCodeResolvedByAuthorizationDataConverterHasExpired_AssertIsMatchAsyncWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(expired: true).Object;
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationCode: authorizationCode);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.IsMatchAsync(
                    It.IsAny<IReadOnlyDictionary<string, string>>(),
                    It.IsAny<ISecurityRepository>(),
                    It.IsAny<ITrustedDomainResolver>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationCodeResolvedByAuthorizationDataConverterHasExpired_AssertClaimsWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(expired: true).Object;
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationCode: authorizationCode);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.Claims, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationCodeResolvedByAuthorizationDataConverterHasExpired_AssertAuthenticationSessionItemsWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(expired: true).Object;
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationCode: authorizationCode);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.AuthenticationSessionItems, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationCodeResolvedByAuthorizationDataConverterHasExpired_AssertProtectorWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(expired: true).Object;
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationCode: authorizationCode);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.Protector, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationCodeResolvedByAuthorizationDataConverterHasExpired_AssertCanBuildWasNotCalledOnExternalTokenClaimCreator()
        {
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(expired: true).Object;
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationCode: authorizationCode);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _externalTokenClaimCreatorMock.Verify(m => m.CanBuild(It.IsAny<IReadOnlyDictionary<string, string>>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationCodeResolvedByAuthorizationDataConverterHasExpired_AssertBuildWasNotCalledOnExternalTokenClaimCreator()
        {
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(expired: true).Object;
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationCode: authorizationCode);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _externalTokenClaimCreatorMock.Verify(m => m.Build(
                    It.IsAny<IReadOnlyDictionary<string, string>>(),
                    It.IsAny<Func<string, string>>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationCodeResolvedByAuthorizationDataConverterHasExpired_AssertAuthenticationTypeWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(expired: true).Object;
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationCode: authorizationCode);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.AuthenticationType, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationCodeResolvedByAuthorizationDataConverterHasExpired_ReturnsNull()
        {
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(expired: true).Object;
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationCode: authorizationCode);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            ClaimsPrincipal result = await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimCollectionCouldNotBeResolvedByAuthorizationDataConverter_AssertClientIdWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasClaims: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.ClientId, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimCollectionCouldNotBeResolvedByAuthorizationDataConverter_AssertClientSecretWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasClaims: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.ClientSecret, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimCollectionCouldNotBeResolvedByAuthorizationDataConverter_AssertRedirectUriWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasClaims: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.RedirectUri, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimCollectionCouldNotBeResolvedByAuthorizationDataConverter_AssertIsMatchAsyncWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasClaims: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.IsMatchAsync(
                    It.IsAny<IReadOnlyDictionary<string, string>>(),
                    It.IsAny<ISecurityRepository>(),
                    It.IsAny<ITrustedDomainResolver>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimCollectionCouldNotBeResolvedByAuthorizationDataConverter_AssertClaimsWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasClaims: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.Claims, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimCollectionCouldNotBeResolvedByAuthorizationDataConverter_AssertAuthenticationSessionItemsWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasClaims: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.AuthenticationSessionItems, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimCollectionCouldNotBeResolvedByAuthorizationDataConverter_AssertProtectorWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasClaims: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.Protector, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimCollectionCouldNotBeResolvedByAuthorizationDataConverter_AssertCanBuildWasNotCalledOnExternalTokenClaimCreator()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasClaims: false);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _externalTokenClaimCreatorMock.Verify(m => m.CanBuild(It.IsAny<IReadOnlyDictionary<string, string>>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimCollectionCouldNotBeResolvedByAuthorizationDataConverter_AssertBuildWasNotCalledOnExternalTokenClaimCreator()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasClaims: false);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _externalTokenClaimCreatorMock.Verify(m => m.Build(
                    It.IsAny<IReadOnlyDictionary<string, string>>(),
                    It.IsAny<Func<string, string>>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimCollectionCouldNotBeResolvedByAuthorizationDataConverter_AssertAuthenticationTypeWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasClaims: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.AuthenticationType, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimCollectionCouldNotBeResolvedByAuthorizationDataConverter_ReturnsNull()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasClaims: false);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            ClaimsPrincipal result = await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimCollectionResolvedByAuthorizationDataConverterIsEmpty_AssertClientIdWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IReadOnlyCollection<Claim> claims = CreateEmptyClaimCollection();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(claims: claims);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.ClientId, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimCollectionResolvedByAuthorizationDataConverterIsEmpty_AssertClientSecretWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IReadOnlyCollection<Claim> claims = CreateEmptyClaimCollection();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(claims: claims);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.ClientSecret, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimCollectionResolvedByAuthorizationDataConverterIsEmpty_AssertRedirectUriWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IReadOnlyCollection<Claim> claims = CreateEmptyClaimCollection();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(claims: claims);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.RedirectUri, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimCollectionResolvedByAuthorizationDataConverterIsEmpty_AssertIsMatchAsyncWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IReadOnlyCollection<Claim> claims = CreateEmptyClaimCollection();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(claims: claims);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.IsMatchAsync(
                    It.IsAny<IReadOnlyDictionary<string, string>>(),
                    It.IsAny<ISecurityRepository>(),
                    It.IsAny<ITrustedDomainResolver>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimCollectionResolvedByAuthorizationDataConverterIsEmpty_AssertClaimsWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IReadOnlyCollection<Claim> claims = CreateEmptyClaimCollection();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(claims: claims);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.Claims, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimCollectionResolvedByAuthorizationDataConverterIsEmpty_AssertAuthenticationSessionItemsWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IReadOnlyCollection<Claim> claims = CreateEmptyClaimCollection();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(claims: claims);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.AuthenticationSessionItems, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimCollectionResolvedByAuthorizationDataConverterIsEmpty_AssertProtectorWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IReadOnlyCollection<Claim> claims = CreateEmptyClaimCollection();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(claims: claims);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.Protector, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimCollectionResolvedByAuthorizationDataConverterIsEmpty_AssertCanBuildWasNotCalledOnExternalTokenClaimCreator()
        {
            IReadOnlyCollection<Claim> claims = CreateEmptyClaimCollection();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(claims: claims);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _externalTokenClaimCreatorMock.Verify(m => m.CanBuild(It.IsAny<IReadOnlyDictionary<string, string>>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimCollectionResolvedByAuthorizationDataConverterIsEmpty_AssertBuildWasNotCalledOnExternalTokenClaimCreator()
        {
            IReadOnlyCollection<Claim> claims = CreateEmptyClaimCollection();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(claims: claims);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _externalTokenClaimCreatorMock.Verify(m => m.Build(
                    It.IsAny<IReadOnlyDictionary<string, string>>(),
                    It.IsAny<Func<string, string>>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimCollectionResolvedByAuthorizationDataConverterIsEmpty_AssertAuthenticationTypeWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IReadOnlyCollection<Claim> claims = CreateEmptyClaimCollection();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(claims: claims);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.AuthenticationType, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimCollectionResolvedByAuthorizationDataConverterIsEmpty_ReturnsNull()
        {
            IReadOnlyCollection<Claim> claims = CreateEmptyClaimCollection();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(claims: claims);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            ClaimsPrincipal result = await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationDataDictionaryCouldNotBeResolvedByAuthorizationDataConverter_AssertClientIdWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasAuthorizationData: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.ClientId, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationDataDictionaryCouldNotBeResolvedByAuthorizationDataConverter_AssertClientSecretWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasAuthorizationData: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.ClientSecret, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationDataDictionaryCouldNotBeResolvedByAuthorizationDataConverter_AssertRedirectUriWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasAuthorizationData: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.RedirectUri, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationDataDictionaryCouldNotBeResolvedByAuthorizationDataConverter_AssertIsMatchAsyncWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasAuthorizationData: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.IsMatchAsync(
                    It.IsAny<IReadOnlyDictionary<string, string>>(),
                    It.IsAny<ISecurityRepository>(),
                    It.IsAny<ITrustedDomainResolver>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationDataDictionaryCouldNotBeResolvedByAuthorizationDataConverter_AssertClaimsWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasAuthorizationData: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.Claims, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationDataDictionaryCouldNotBeResolvedByAuthorizationDataConverter_AssertAuthenticationSessionItemsWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasAuthorizationData: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.AuthenticationSessionItems, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationDataDictionaryCouldNotBeResolvedByAuthorizationDataConverter_AssertProtectorWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasAuthorizationData: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.Protector, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationDataDictionaryCouldNotBeResolvedByAuthorizationDataConverter_AssertCanBuildWasNotCalledOnExternalTokenClaimCreator()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasAuthorizationData: false);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _externalTokenClaimCreatorMock.Verify(m => m.CanBuild(It.IsAny<IReadOnlyDictionary<string, string>>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationDataDictionaryCouldNotBeResolvedByAuthorizationDataConverter_AssertBuildWasNotCalledOnExternalTokenClaimCreator()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasAuthorizationData: false);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _externalTokenClaimCreatorMock.Verify(m => m.Build(
                    It.IsAny<IReadOnlyDictionary<string, string>>(),
                    It.IsAny<Func<string, string>>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationDataDictionaryCouldNotBeResolvedByAuthorizationDataConverter_AssertAuthenticationTypeWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasAuthorizationData: false);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.AuthenticationType, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationDataDictionaryCouldNotBeResolvedByAuthorizationDataConverter_ReturnsNull()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(hasAuthorizationData: false);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            ClaimsPrincipal result = await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationDataDictionaryResolvedByAuthorizationDataConverterIsEmpty_AssertClientIdWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IReadOnlyDictionary<string, string> authorizationData = CreateEmptyAuthorizationDataDictionary();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationData: authorizationData);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.ClientId, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationDataDictionaryResolvedByAuthorizationDataConverterIsEmpty_AssertClientSecretWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IReadOnlyDictionary<string, string> authorizationData = CreateEmptyAuthorizationDataDictionary();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationData: authorizationData);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.ClientSecret, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationDataDictionaryResolvedByAuthorizationDataConverterIsEmpty_AssertRedirectUriWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IReadOnlyDictionary<string, string> authorizationData = CreateEmptyAuthorizationDataDictionary();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationData: authorizationData);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.RedirectUri, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationDataDictionaryResolvedByAuthorizationDataConverterIsEmpty_AssertIsMatchAsyncWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IReadOnlyDictionary<string, string> authorizationData = CreateEmptyAuthorizationDataDictionary();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationData: authorizationData);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.IsMatchAsync(
                    It.IsAny<IReadOnlyDictionary<string, string>>(),
                    It.IsAny<ISecurityRepository>(),
                    It.IsAny<ITrustedDomainResolver>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationDataDictionaryResolvedByAuthorizationDataConverterIsEmpty_AssertClaimsWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IReadOnlyDictionary<string, string> authorizationData = CreateEmptyAuthorizationDataDictionary();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationData: authorizationData);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.Claims, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationDataDictionaryResolvedByAuthorizationDataConverterIsEmpty_AssertAuthenticationSessionItemsWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IReadOnlyDictionary<string, string> authorizationData = CreateEmptyAuthorizationDataDictionary();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationData: authorizationData);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.AuthenticationSessionItems, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationDataDictionaryResolvedByAuthorizationDataConverterIsEmpty_AssertProtectorWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IReadOnlyDictionary<string, string> authorizationData = CreateEmptyAuthorizationDataDictionary();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationData: authorizationData);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.Protector, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationDataDictionaryResolvedByAuthorizationDataConverterIsEmpty_AssertCanBuildWasNotCalledOnExternalTokenClaimCreator()
        {
            IReadOnlyDictionary<string, string> authorizationData = CreateEmptyAuthorizationDataDictionary();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationData: authorizationData);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _externalTokenClaimCreatorMock.Verify(m => m.CanBuild(It.IsAny<IReadOnlyDictionary<string, string>>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationDataDictionaryResolvedByAuthorizationDataConverterIsEmpty_AssertBuildWasNotCalledOnExternalTokenClaimCreator()
        {
            IReadOnlyDictionary<string, string> authorizationData = CreateEmptyAuthorizationDataDictionary();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationData: authorizationData);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _externalTokenClaimCreatorMock.Verify(m => m.Build(
                    It.IsAny<IReadOnlyDictionary<string, string>>(),
                    It.IsAny<Func<string, string>>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationDataDictionaryResolvedByAuthorizationDataConverterIsEmpty_AssertAuthenticationTypeWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            IReadOnlyDictionary<string, string> authorizationData = CreateEmptyAuthorizationDataDictionary();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationData: authorizationData);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.AuthenticationType, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenAuthorizationDataDictionaryResolvedByAuthorizationDataConverterIsEmpty_ReturnsNull()
        {
            IReadOnlyDictionary<string, string> authorizationData = CreateEmptyAuthorizationDataDictionary();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationData: authorizationData);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            ClaimsPrincipal result = await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenIdentityForAuthorizationCodeCouldBeCreated_AssertIsMatchAsyncWasCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.IsMatchAsync(
                    It.IsNotNull<IReadOnlyDictionary<string, string>>(),
                    It.IsNotNull<ISecurityRepository>(),
                    It.IsNotNull<ITrustedDomainResolver>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenIdentityForAuthorizationCodeCouldBeCreated_AssertIsMatchAsyncWasCalledOnAuthenticateAuthorizationCodeCommandWithAuthorizationDataDictionaryResolvedByAuthorizationDataConverter()
        {
            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationDataDictionary();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationData: authorizationData);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.IsMatchAsync(
                    It.Is<IReadOnlyDictionary<string, string>>(value => value != null && value == authorizationData),
                    It.IsNotNull<ISecurityRepository>(),
                    It.IsNotNull<ITrustedDomainResolver>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenIdentityForAuthorizationCodeCouldBeCreated_AssertIsMatchAsyncWasCalledOnAuthenticateAuthorizationCodeCommandWithSecurityRepository()
        {
            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationDataDictionary();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationData: authorizationData);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.IsMatchAsync(
                    It.IsNotNull<IReadOnlyDictionary<string, string>>(),
                    It.Is<ISecurityRepository>(value => value != null && value == _securityRepositoryMock.Object),
                    It.IsNotNull<ITrustedDomainResolver>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenIdentityForAuthorizationCodeCouldBeCreated_AssertIsMatchAsyncWasCalledOnAuthenticateAuthorizationCodeCommandWithTrustedDomainResolver()
        {
            IReadOnlyDictionary<string, string> authorizationData = CreateAuthorizationDataDictionary();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(authorizationData: authorizationData);

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.IsMatchAsync(
                    It.IsNotNull<IReadOnlyDictionary<string, string>>(),
                    It.IsNotNull<ISecurityRepository>(),
                    It.Is<ITrustedDomainResolver>(value => value != null && value == _trustedDomainResolverMock.Object)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsNoneMatching_AssertClientIdWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock(isMatch: false);
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.ClientId, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsNoneMatching_AssertClientSecretWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock(isMatch: false);
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.ClientSecret, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsNoneMatching_AssertRedirectUriWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock(isMatch: false);
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.RedirectUri, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsNoneMatching_AssertClaimsWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock(isMatch: false);
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.Claims, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsNoneMatching_AssertAuthenticationSessionItemsWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock(isMatch: false);
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.AuthenticationSessionItems, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsNoneMatching_AssertProtectorWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock(isMatch: false);
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.Protector, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsNoneMatching_AssertCanBuildWasNotCalledOnExternalTokenClaimCreator()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand(isMatch: false);
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _externalTokenClaimCreatorMock.Verify(m => m.CanBuild(It.IsAny<IReadOnlyDictionary<string, string>>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsNoneMatching_AssertBuildWasNotCalledOnExternalTokenClaimCreator()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand(isMatch: false);
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _externalTokenClaimCreatorMock.Verify(m => m.Build(
                    It.IsAny<IReadOnlyDictionary<string, string>>(),
                    It.IsAny<Func<string, string>>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsNoneMatching_AssertAuthenticationTypeWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock(isMatch: false);
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.AuthenticationType, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsNoneMatching_ReturnsNull()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand(isMatch: false);
            ClaimsPrincipal result = await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatching_AssertClientIdWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.ClientId, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatching_AssertClientSecretWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.ClientSecret, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatching_AssertRedirectUriWasNotCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.RedirectUri, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatching_AssertClaimsWasCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.Claims, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatching_AssertAuthenticationSessionItemsWasCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.AuthenticationSessionItems, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatching_AssertProtectorWasCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.Protector, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatching_AssertCanBuildWasCalledOnExternalTokenClaimCreator()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _externalTokenClaimCreatorMock.Verify(m => m.CanBuild(It.IsNotNull<IReadOnlyDictionary<string, string>>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatching_AssertCanBuildWasCalledOnExternalTokenClaimCreatorWithAuthenticationSessionItemsFromAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            IReadOnlyDictionary<string, string> authenticationSessionItems = new ConcurrentDictionary<string, string>();
            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand(authenticationSessionItems: authenticationSessionItems);
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _externalTokenClaimCreatorMock.Verify(m => m.CanBuild(It.Is<IReadOnlyDictionary<string, string>>(value => value != null && value == authenticationSessionItems)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatchingAndExternalTokenClaimCouldBeBuild_AssertBuildWasCalledOnExternalTokenClaimCreator()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(canBuildExternalTokenClaim: true);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _externalTokenClaimCreatorMock.Verify(m => m.Build(
                    It.IsNotNull<IReadOnlyDictionary<string, string>>(),
                    It.IsNotNull<Func<string, string>>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatchingAndExternalTokenClaimCouldBeBuild_AssertBuildWasCalledOnExternalTokenClaimCreatorWithAuthenticationSessionItemsFromAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(canBuildExternalTokenClaim: true);

            IReadOnlyDictionary<string, string> authenticationSessionItems = new ConcurrentDictionary<string, string>();
            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand(authenticationSessionItems: authenticationSessionItems);
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _externalTokenClaimCreatorMock.Verify(m => m.Build(
                    It.Is<IReadOnlyDictionary<string, string>>(value => value != null && value == authenticationSessionItems),
                    It.IsNotNull<Func<string, string>>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatchingAndExternalTokenClaimCouldBeBuild_AssertBuildWasCalledOnExternalTokenClaimCreatorWithProtectorFromAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(canBuildExternalTokenClaim: true);

            Func<string, string> protector = value => value;
            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand(protector: protector);
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _externalTokenClaimCreatorMock.Verify(m => m.Build(
                    It.IsNotNull<IReadOnlyDictionary<string, string>>(),
                    It.Is<Func<string, string>>(value => value != null && value == protector)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatchingAndExternalTokenClaimCouldNotBeBuild_AssertBuildWasNotCalledOnExternalTokenClaimCreator()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(canBuildExternalTokenClaim: false);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            _externalTokenClaimCreatorMock.Verify(m => m.Build(
                    It.IsAny<IReadOnlyDictionary<string, string>>(),
                    It.IsAny<Func<string, string>>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatching_AssertAuthenticationTypeWasCalledOnAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = CreateAuthenticateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(authenticateAuthorizationCodeCommandMock.Object);

            authenticateAuthorizationCodeCommandMock.Verify(m => m.AuthenticationType, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatching_ReturnsNotNull()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            ClaimsPrincipal result = await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatching_ReturnsClaimsPrincipalWhereIdentityIsNotNull()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            ClaimsPrincipal result = await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            Assert.That(result.Identity, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatching_ReturnsClaimsPrincipalWhereIdentityIsAuthenticated()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            ClaimsPrincipal result = await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            Assert.That(result.Identity, Is.Not.Null);
            Assert.That(result.Identity.IsAuthenticated, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatching_ReturnsClaimsPrincipalWithIdentityWhereAuthenticationTypeIsNotNull()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            ClaimsPrincipal result = await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            Assert.That(result.Identity, Is.Not.Null);
            Assert.That(result.Identity.AuthenticationType, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatching_ReturnsClaimsPrincipalWithIdentityWhereAuthenticationTypeIsNotEmpty()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            ClaimsPrincipal result = await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            Assert.That(result.Identity, Is.Not.Null);
            Assert.That(result.Identity.AuthenticationType, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatching_ReturnsClaimsPrincipalWithIdentityWhereAuthenticationTypeIsEqualToAuthenticationTypeFromAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            string authenticationType = _fixture.Create<string>();
            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand(authenticationType: authenticationType);
            ClaimsPrincipal result = await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            Assert.That(result.Identity, Is.Not.Null);
            Assert.That(result.Identity.AuthenticationType, Is.EqualTo(authenticationType));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatching_ReturnsClaimsPrincipalWhereClaimsIsNotNull()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            ClaimsPrincipal result = await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            Assert.That(result.Claims, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatching_ReturnsClaimsPrincipalWhereClaimsIsNotEmpty()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            ClaimsPrincipal result = await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            Assert.That(result.Claims, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatching_ReturnsClaimsPrincipalWhereClaimsContainsClaimsForAuthorizationCode()
        {
            Claim[] claims = _fixture.CreateClaims(_random);
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(claims: claims);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            ClaimsPrincipal result = await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            Assert.That(claims.All(claim => result.HasClaim(claim.Type, claim.Value)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatching_ReturnsClaimsPrincipalWhereClaimsContainsClaimsFromAuthenticateAuthorizationCodeCommand()
        {
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut();

            Claim[] claims = _fixture.CreateClaims(_random);
            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand(claims: claims);
            ClaimsPrincipal result = await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            Assert.That(claims.All(claim => result.HasClaim(claim.Type, claim.Value)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCreatedIdentityForAuthorizationCodeIsMatchingAndExternalTokenClaimCouldBeBuild_ReturnsClaimsPrincipalWhereClaimsContainsExternalTokenClaim()
        {
            Claim externalTokenClaim = _fixture.CreateClaim();
            ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> sut = CreateSut(canBuildExternalTokenClaim: true, externalTokenClaim: externalTokenClaim);

            IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand = CreateAuthenticateAuthorizationCodeCommand();
            ClaimsPrincipal result = await sut.ExecuteAsync(authenticateAuthorizationCodeCommand);

            Assert.That(result.HasClaim(externalTokenClaim.Type, externalTokenClaim.Value), Is.True);
        }

        private ICommandHandler<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal> CreateSut(bool hasKeyValueEntryForAuthorizationCode = true, IKeyValueEntry keyValueEntryForAuthorizationCode = null, bool hasAuthorizationCode = true, IAuthorizationCode authorizationCode = null, bool hasClaims = true, IReadOnlyCollection<Claim> claims = null, bool hasAuthorizationData = true, IReadOnlyDictionary<string, string> authorizationData = null, bool canBuildExternalTokenClaim = true, bool hasExternalTokenClaim = true, Claim externalTokenClaim = null)
        {
            claims = hasClaims ? claims ?? CreateClaimCollection() : null;
            authorizationData = hasAuthorizationData ? authorizationData ?? CreateAuthorizationDataDictionary() : null;

            _commonRepositoryMock.Setup(m => m.PullKeyValueEntryAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(hasKeyValueEntryForAuthorizationCode ? keyValueEntryForAuthorizationCode ?? _fixture.BuildKeyValueEntryMock<object>().Object : null));
            _commonRepositoryMock.Setup(m => m.DeleteKeyValueEntryAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(_fixture.BuildKeyValueEntryMock<object>().Object));

            _authorizationDataConverterMock.Setup(m => m.ToAuthorizationCodeAsync(It.IsAny<IKeyValueEntry>(), out claims, out authorizationData))
                .Returns(Task.FromResult(hasAuthorizationCode ? authorizationCode ?? _fixture.BuildAuthorizationCodeMock().Object : null));

            _externalTokenClaimCreatorMock.Setup(m => m.CanBuild(It.IsAny<IReadOnlyDictionary<string, string>>()))
                .Returns(canBuildExternalTokenClaim);
            _externalTokenClaimCreatorMock.Setup(m => m.Build(It.IsAny<IReadOnlyDictionary<string, string>>(), It.IsAny<Func<string, string>>()))
                .Returns(hasExternalTokenClaim ? externalTokenClaim ?? CreateExternalTokenClaim() : null);

            return new BusinessLogic.Security.CommandHandlers.AuthenticateAuthorizationCodeCommandHandler(_securityRepositoryMock.Object, _commonRepositoryMock.Object, _authorizationDataConverterMock.Object, _trustedDomainResolverMock.Object, _externalTokenClaimCreatorMock.Object);
        }

        private IAuthenticateAuthorizationCodeCommand CreateAuthenticateAuthorizationCodeCommand(string authorizationCode = null, string clientId = null, string clientSecret = null, Uri redirectUri = null, IReadOnlyCollection<Claim> claims = null, string authenticationType = null, IReadOnlyDictionary<string, string> authenticationSessionItems = null, Func<string, string> protector = null, bool isMatch = true)
        {
            return CreateAuthenticateAuthorizationCodeCommandMock(authorizationCode, clientId, clientSecret, redirectUri, claims, authenticationType, authenticationSessionItems, protector, isMatch).Object;
        }

        private Mock<IAuthenticateAuthorizationCodeCommand> CreateAuthenticateAuthorizationCodeCommandMock(string authorizationCode = null, string clientId = null, string clientSecret = null, Uri redirectUri = null, IReadOnlyCollection<Claim> claims = null, string authenticationType = null, IReadOnlyDictionary<string, string> authenticationSessionItems = null, Func<string, string> protector = null, bool isMatch = true)
        {
            Mock<IAuthenticateAuthorizationCodeCommand> authenticateAuthorizationCodeCommandMock = new Mock<IAuthenticateAuthorizationCodeCommand>();
            authenticateAuthorizationCodeCommandMock.Setup(m => m.AuthorizationCode)
                .Returns(authorizationCode ?? _fixture.Create<string>());
            authenticateAuthorizationCodeCommandMock.Setup(m => m.ClientId)
                .Returns(clientId ?? _fixture.Create<string>());
            authenticateAuthorizationCodeCommandMock.Setup(m => m.ClientSecret)
                .Returns(clientSecret ?? _fixture.Create<string>());
            authenticateAuthorizationCodeCommandMock.Setup(m => m.RedirectUri)
                .Returns(redirectUri ?? _fixture.CreateEndpoint());
            authenticateAuthorizationCodeCommandMock.Setup(m => m.Claims)
                .Returns(claims ?? CreateEmptyClaimCollection());
            authenticateAuthorizationCodeCommandMock.Setup(m => m.AuthenticationType)
                .Returns(authenticationType ?? _fixture.Create<string>());
            authenticateAuthorizationCodeCommandMock.Setup(m => m.AuthenticationSessionItems)
                .Returns(authenticationSessionItems ?? new ConcurrentDictionary<string, string>());
            authenticateAuthorizationCodeCommandMock.Setup(m => m.Protector)
                .Returns(protector ?? (value => value));
            authenticateAuthorizationCodeCommandMock.Setup(m => m.IsMatchAsync(It.IsAny<IReadOnlyDictionary<string, string>>(), It.IsAny<ISecurityRepository>(), It.IsAny<ITrustedDomainResolver>()))
                .Returns(Task.FromResult(isMatch));
            return authenticateAuthorizationCodeCommandMock;
        }

        private Claim CreateExternalTokenClaim()
        {
            return ClaimHelper.CreateClaim(ClaimHelper.MicrosoftTokenClaimType, Convert.ToBase64String(_fixture.CreateMany<byte>(_random.Next(256, 512)).ToArray()), typeof(IRefreshableToken).FullName);
        }

        private IReadOnlyCollection<Claim> CreateClaimCollection()
        {
            return new[]
            {
                ClaimHelper.CreateNameIdentifierClaim(_fixture.Create<string>()),
                ClaimHelper.CreateNameClaim(_fixture.Create<string>()),
                ClaimHelper.CreateEmailClaim($"{_fixture.Create<string>()}@{_fixture.Create<string>()}.{_fixture.Create<string>()}")
            };
        }

        private IReadOnlyCollection<Claim> CreateEmptyClaimCollection()
        {
            return Array.Empty<Claim>();
        }

        private IReadOnlyDictionary<string, string> CreateAuthorizationDataDictionary()
        {
            return _fixture.CreateMany<string>(_random.Next(5, 10))
                .ToDictionary(value => value, _ => _fixture.Create<string>())
                .AsReadOnly();
        }

        private IReadOnlyDictionary<string, string> CreateEmptyAuthorizationDataDictionary()
        {
            return new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
        }
    }
}