using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.CommandHandlers.GenerateAuthorizationCodeCommandHandler
{
    [TestFixture]
    public class ExecuteAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IAuthorizationStateFactory> _authorizationStateFactoryMock;
        private Mock<ISecurityRepository> _securityRepositoryMock;
        private Mock<ICommonRepository> _commonRepositoryMock;
        private Mock<ITrustedDomainResolver> _trustedDomainResolverMock;
        private Mock<ISupportedScopesProvider> _supportedScopesProviderMock;
        private Mock<IClaimsSelector> _claimsSelectorMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _authorizationStateFactoryMock = new Mock<IAuthorizationStateFactory>();
            _securityRepositoryMock = new Mock<ISecurityRepository>();
            _commonRepositoryMock = new Mock<ICommonRepository>();
            _trustedDomainResolverMock = new Mock<ITrustedDomainResolver>();
            _supportedScopesProviderMock = new Mock<ISupportedScopesProvider>();
            _claimsSelectorMock = new Mock<IClaimsSelector>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenGenerateAuthorizationCodeCommandIsNull_ThrowsArgumentNullException()
        {
            ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("generateAuthorizationCodeCommand"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnGenerateAuthorizationCodeCommand()
        {
            ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> sut = CreateSut();

            Mock<IGenerateAuthorizationCodeCommand> generateAuthorizationCodeCommandMock = CreateGenerateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(generateAuthorizationCodeCommandMock.Object);

            generateAuthorizationCodeCommandMock.Verify(m => m.Validate(It.Is<IValidator>(value => value != null && value == _validatorMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnGenerateAuthorizationCodeCommand()
        {
            ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> sut = CreateSut();

            Mock<IGenerateAuthorizationCodeCommand> generateAuthorizationCodeCommandMock = CreateGenerateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(generateAuthorizationCodeCommandMock.Object);

            generateAuthorizationCodeCommandMock.Verify(m => m.ToDomain(
                    It.Is<IAuthorizationStateFactory>(value => value != null && value == _authorizationStateFactoryMock.Object),
                    It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
                    It.Is<ISecurityRepository>(value => value != null && value == _securityRepositoryMock.Object),
                    It.Is<ITrustedDomainResolver>(value => value != null && value == _trustedDomainResolverMock.Object),
                    It.Is<ISupportedScopesProvider>(value => value != null && value == _supportedScopesProviderMock.Object)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertClientIdWasCalledOnAuthorizationStateCreatedByToDomainOnGenerateAuthorizationCodeCommand()
        {
            ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> sut = CreateSut();

            Mock<IAuthorizationState> authorizationState = _fixture.BuildAuthorizationStateMock();
            IGenerateAuthorizationCodeCommand generateAuthorizationCodeCommand = CreateGenerateAuthorizationCodeCommand(toDomain: authorizationState.Object);
            await sut.ExecuteAsync(generateAuthorizationCodeCommand);

            authorizationState.Verify(m => m.ClientId, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertGetClientSecretIdentityAsyncWasCalledOnSecurityRepositoryWithClientIdFromAuthorizationStateCreatedByToDomain()
        {
            ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> sut = CreateSut();

            string clientId = _fixture.Create<string>();
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock(clientId: clientId).Object;
            IGenerateAuthorizationCodeCommand generateAuthorizationCodeCommand = CreateGenerateAuthorizationCodeCommand(toDomain: authorizationState);
            await sut.ExecuteAsync(generateAuthorizationCodeCommand);

            _securityRepositoryMock.Verify(m => m.GetClientSecretIdentityAsync(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, clientId) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientSecretCouldNotBeResolvedForClientId_AssertSupportedScopesWasNotCalledOnSupportedScopesProvider()
        {
            ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> sut = CreateSut(hasClientSecretIdentity: false);

            IGenerateAuthorizationCodeCommand generateAuthorizationCodeCommand = CreateGenerateAuthorizationCodeCommand();
            await sut.ExecuteAsync(generateAuthorizationCodeCommand);

            _supportedScopesProviderMock.Verify(m => m.SupportedScopes, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientSecretCouldNotBeResolvedForClientId_AssertScopesWasNotCalledOnAuthorizationStateCreatedByToDomainOnGenerateAuthorizationCodeCommand()
        {
            ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> sut = CreateSut(hasClientSecretIdentity: false);

            Mock<IAuthorizationState> authorizationState = _fixture.BuildAuthorizationStateMock();
            IGenerateAuthorizationCodeCommand generateAuthorizationCodeCommand = CreateGenerateAuthorizationCodeCommand(toDomain: authorizationState.Object);
            await sut.ExecuteAsync(generateAuthorizationCodeCommand);

            authorizationState.Verify(m => m.Scopes, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientSecretCouldNotBeResolvedForClientId_AssertClaimsWasNotCalledOnGenerateAuthorizationCodeCommand()
        {
            ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> sut = CreateSut(hasClientSecretIdentity: false);

            Mock<IGenerateAuthorizationCodeCommand> generateAuthorizationCodeCommandMock = CreateGenerateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(generateAuthorizationCodeCommandMock.Object);

            generateAuthorizationCodeCommandMock.Verify(m => m.Claims, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientSecretCouldNotBeResolvedForClientId_AssertSelectWasNotCalledOnClaimsSelector()
        {
            ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> sut = CreateSut(hasClientSecretIdentity: false);

            IGenerateAuthorizationCodeCommand generateAuthorizationCodeCommand = CreateGenerateAuthorizationCodeCommand();
            await sut.ExecuteAsync(generateAuthorizationCodeCommand);

            _claimsSelectorMock.Verify(m => m.Select(
                    It.IsAny<IReadOnlyDictionary<string, IScope>>(),
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<IEnumerable<Claim>>()),
                Times.Never);
        }

        // TODO: Continue testing

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientSecretCouldNotBeResolvedForClientId_AssertToBuilderWasNotCalledOnAuthorizationStateCreatedByToDomainOnGenerateAuthorizationCodeCommand()
        {
            ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> sut = CreateSut(hasClientSecretIdentity: false);

            Mock<IAuthorizationState> authorizationState = _fixture.BuildAuthorizationStateMock();
            IGenerateAuthorizationCodeCommand generateAuthorizationCodeCommand = CreateGenerateAuthorizationCodeCommand(toDomain: authorizationState.Object);
            await sut.ExecuteAsync(generateAuthorizationCodeCommand);

            authorizationState.Verify(m => m.ToBuilder(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientSecretCouldNotBeResolvedForClientId_ReturnsNull()
        {
            ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> sut = CreateSut(hasClientSecretIdentity: false);

            IGenerateAuthorizationCodeCommand generateAuthorizationCodeCommand = CreateGenerateAuthorizationCodeCommand();
            IAuthorizationState result = await sut.ExecuteAsync(generateAuthorizationCodeCommand);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientSecretCouldBeResolvedForClientId_AssertSupportedScopesWasCalledOnSupportedScopesProvider()
        {
            ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> sut = CreateSut();

            IGenerateAuthorizationCodeCommand generateAuthorizationCodeCommand = CreateGenerateAuthorizationCodeCommand();
            await sut.ExecuteAsync(generateAuthorizationCodeCommand);

            _supportedScopesProviderMock.Verify(m => m.SupportedScopes, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientSecretCouldBeResolvedForClientId_AssertScopesWasCalledOnAuthorizationStateCreatedByToDomainOnGenerateAuthorizationCodeCommand()
        {
            ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> sut = CreateSut();

            Mock<IAuthorizationState> authorizationState = _fixture.BuildAuthorizationStateMock();
            IGenerateAuthorizationCodeCommand generateAuthorizationCodeCommand = CreateGenerateAuthorizationCodeCommand(toDomain: authorizationState.Object);
            await sut.ExecuteAsync(generateAuthorizationCodeCommand);

            authorizationState.Verify(m => m.Scopes, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientSecretCouldBeResolvedForClientId_AssertClaimsWasCalledOnGenerateAuthorizationCodeCommand()
        {
            ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> sut = CreateSut();

            Mock<IGenerateAuthorizationCodeCommand> generateAuthorizationCodeCommandMock = CreateGenerateAuthorizationCodeCommandMock();
            await sut.ExecuteAsync(generateAuthorizationCodeCommandMock.Object);

            generateAuthorizationCodeCommandMock.Verify(m => m.Claims, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientSecretCouldBeResolvedForClientId_AssertSelectWasCalledOnClaimsSelectorWithSupportedScopesFromSupportedScopesProvider()
        {
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes();
            ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> sut = CreateSut(supportedScopes: supportedScopes);

            IGenerateAuthorizationCodeCommand generateAuthorizationCodeCommand = CreateGenerateAuthorizationCodeCommand();
            await sut.ExecuteAsync(generateAuthorizationCodeCommand);

            _claimsSelectorMock.Verify(m => m.Select(
                    It.Is<IReadOnlyDictionary<string, IScope>>(value => value != null && value == supportedScopes),
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<IEnumerable<Claim>>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientSecretCouldBeResolvedForClientId_AssertSelectWasCalledOnClaimsSelectorWithScopesFromAuthorizationStateCreatedByToDomainOnGenerateAuthorizationCodeCommand()
        {
            ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> sut = CreateSut();

            string[] scopes = _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray();
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock(scopes: scopes).Object;
            IGenerateAuthorizationCodeCommand generateAuthorizationCodeCommand = CreateGenerateAuthorizationCodeCommand(toDomain: authorizationState);
            await sut.ExecuteAsync(generateAuthorizationCodeCommand);

            _claimsSelectorMock.Verify(m => m.Select(
                    It.IsAny<IReadOnlyDictionary<string, IScope>>(),
                    It.Is<IEnumerable<string>>(value => value != null && scopes.All(value.Contains)),
                    It.IsAny<IEnumerable<Claim>>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientSecretCouldBeResolvedForClientId_AssertSelectWasCalledOnClaimsSelectorWithClaimsFromGenerateAuthorizationCodeCommand()
        {
            ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> sut = CreateSut();

            Claim[] claims = CreateClaims().ToArray();
            IGenerateAuthorizationCodeCommand generateAuthorizationCodeCommand = CreateGenerateAuthorizationCodeCommand(claims: claims);
            await sut.ExecuteAsync(generateAuthorizationCodeCommand);

            _claimsSelectorMock.Verify(m => m.Select(
                    It.IsAny<IReadOnlyDictionary<string, IScope>>(),
                    It.IsAny<IEnumerable<string>>(),
                    It.Is<IEnumerable<Claim>>(value => value != null && claims.All(value.Contains))),
                Times.Once);
        }

        // TODO: Continue testing

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenSelectedClaimsCouldNotBeResolved_AssertToBuilderWasNotCalledOnAuthorizationStateCreatedByToDomainOnGenerateAuthorizationCodeCommand()
        {
            ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> sut = CreateSut(hasSelectedClaims: false);

            Mock<IAuthorizationState> authorizationState = _fixture.BuildAuthorizationStateMock();
            IGenerateAuthorizationCodeCommand generateAuthorizationCodeCommand = CreateGenerateAuthorizationCodeCommand(toDomain: authorizationState.Object);
            await sut.ExecuteAsync(generateAuthorizationCodeCommand);

            authorizationState.Verify(m => m.ToBuilder(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenSelectedClaimsCouldNotBeResolved_ReturnsNull()
        {
            ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> sut = CreateSut(hasSelectedClaims: false);

            IGenerateAuthorizationCodeCommand generateAuthorizationCodeCommand = CreateGenerateAuthorizationCodeCommand();
            IAuthorizationState result = await sut.ExecuteAsync(generateAuthorizationCodeCommand);

            Assert.That(result, Is.Null);
        }

        // TODO: Continue testing

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenSelectedClaimsCouldBeResolvedAsEmptyCollection_AssertToBuilderWasNotCalledOnAuthorizationStateCreatedByToDomainOnGenerateAuthorizationCodeCommand()
        {
            ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> sut = CreateSut(selectedClaims: Array.Empty<Claim>());

            Mock<IAuthorizationState> authorizationState = _fixture.BuildAuthorizationStateMock();
            IGenerateAuthorizationCodeCommand generateAuthorizationCodeCommand = CreateGenerateAuthorizationCodeCommand(toDomain: authorizationState.Object);
            await sut.ExecuteAsync(generateAuthorizationCodeCommand);

            authorizationState.Verify(m => m.ToBuilder(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenSelectedClaimsCouldBeResolvedAsEmptyCollection_ReturnsNull()
        {
            ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> sut = CreateSut(selectedClaims: Array.Empty<Claim>());

            IGenerateAuthorizationCodeCommand generateAuthorizationCodeCommand = CreateGenerateAuthorizationCodeCommand();
            IAuthorizationState result = await sut.ExecuteAsync(generateAuthorizationCodeCommand);

            Assert.That(result, Is.Null);
        }

        // TODO: Continue testing

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenSelectedClaimsCouldBeResolvedAsNonEmptyCollection_AssertToBuilderWasCalledOnAuthorizationStateCreatedByToDomainOnGenerateAuthorizationCodeCommand()
        {
            ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> sut = CreateSut();

            Mock<IAuthorizationState> authorizationState = _fixture.BuildAuthorizationStateMock();
            IGenerateAuthorizationCodeCommand generateAuthorizationCodeCommand = CreateGenerateAuthorizationCodeCommand(toDomain: authorizationState.Object);
            await sut.ExecuteAsync(generateAuthorizationCodeCommand);

            authorizationState.Verify(m => m.ToBuilder(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenSelectedClaimsCouldBeResolvedAsNonEmptyCollection_ReturnsNotNull()
        {
            ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> sut = CreateSut();

            IGenerateAuthorizationCodeCommand generateAuthorizationCodeCommand = CreateGenerateAuthorizationCodeCommand();
            IAuthorizationState result = await sut.ExecuteAsync(generateAuthorizationCodeCommand);

            Assert.That(result, Is.Not.Null);
        }

        private ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState> CreateSut(bool hasClientSecretIdentity = true, IClientSecretIdentity clientSecretIdentity = null, IReadOnlyDictionary<string, IScope> supportedScopes = null, bool hasSelectedClaims = true, IReadOnlyCollection<Claim> selectedClaims = null)
        {
            _securityRepositoryMock.Setup(m => m.GetClientSecretIdentityAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(hasClientSecretIdentity ? clientSecretIdentity ?? _fixture.BuildClientSecretIdentityMock().Object : null));

            _supportedScopesProviderMock.Setup(m => m.SupportedScopes)
                .Returns(supportedScopes ?? CreateSupportedScopes());

            _claimsSelectorMock.Setup(m => m.Select(It.IsAny<IReadOnlyDictionary<string, IScope>>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<Claim>>()))
                .Returns(hasSelectedClaims ? selectedClaims ?? CreateClaims() : null);

            return new BusinessLogic.Security.CommandHandlers.GenerateAuthorizationCodeCommandHandler(_validatorMock.Object, _authorizationStateFactoryMock.Object, _securityRepositoryMock.Object, _commonRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object, _claimsSelectorMock.Object);
        }

        private IGenerateAuthorizationCodeCommand CreateGenerateAuthorizationCodeCommand(IReadOnlyCollection<Claim> claims = null, IAuthorizationState toDomain = null)
        {
            return CreateGenerateAuthorizationCodeCommandMock(claims, toDomain).Object;
        }

        private Mock<IGenerateAuthorizationCodeCommand> CreateGenerateAuthorizationCodeCommandMock(IReadOnlyCollection<Claim> claims = null, IAuthorizationState toDomain = null)
        {
            Mock<IGenerateAuthorizationCodeCommand> generateAuthorizationCodeCommandMock = new Mock<IGenerateAuthorizationCodeCommand>();
            generateAuthorizationCodeCommandMock.Setup(m => m.Claims)
                .Returns(claims ?? CreateClaims());
            generateAuthorizationCodeCommandMock.Setup(m => m.Validate(It.IsAny<IValidator>()))
                .Returns(_validatorMock.Object);
            generateAuthorizationCodeCommandMock.Setup(m => m.ToDomain(It.IsAny<IAuthorizationStateFactory>(), It.IsAny<IValidator>(), It.IsAny<ISecurityRepository>(), It.IsAny<ITrustedDomainResolver>(), It.IsAny<ISupportedScopesProvider>()))
                .Returns(toDomain ?? _fixture.BuildAuthorizationStateMock().Object);
            return generateAuthorizationCodeCommandMock;
        }

        private IReadOnlyDictionary<string, IScope> CreateSupportedScopes()
        {
            return _fixture.CreateMany<string>(_random.Next(5, 10))
                .Select(scope => _fixture.BuildScopeMock(name: scope).Object)
                .ToDictionary(supportedScope => supportedScope.Name, supportedScope => supportedScope)
                .AsReadOnly();
        }

        private IReadOnlyCollection<Claim> CreateClaims()
        {
            return _fixture.CreateMany<string>(_random.Next(5, 10))
                .Select(claimType => new Claim(claimType, string.Empty))
                .ToArray();
        }
    }
}