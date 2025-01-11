using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.CommandHandlers.GenerateIdTokenCommandHandler
{
    [TestFixture]
    public class ExecuteAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IAuthorizationStateFactory> _authorizationStateFactoryMock;
        private Mock<ISecurityRepository> _securityRepositoryMock;
        private Mock<ITrustedDomainResolver> _trustedDomainResolverMock;
        private Mock<ISupportedScopesProvider> _supportedScopesProviderMock;
        private Mock<IUserInfoFactory> _userInfoFactoryMock;
        private Mock<IIdTokenContentFactory> _idTokenContentFactoryMock;
        private Mock<IIdTokenContentBuilder> _idTokenContentBuilderMock;
        private Mock<ITokenGenerator> _tokenGeneratorMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _authorizationStateFactoryMock = new Mock<IAuthorizationStateFactory>();
            _securityRepositoryMock = new Mock<ISecurityRepository>();
            _trustedDomainResolverMock = new Mock<ITrustedDomainResolver>();
            _supportedScopesProviderMock = new Mock<ISupportedScopesProvider>();
            _userInfoFactoryMock = new Mock<IUserInfoFactory>();
            _idTokenContentFactoryMock = new Mock<IIdTokenContentFactory>();
            _idTokenContentBuilderMock = new Mock<IIdTokenContentBuilder>();
            _tokenGeneratorMock = new Mock<ITokenGenerator>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenGenerateIdTokenCommandIsNull_ThrowsArgumentNullException()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("command"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            Mock<IGenerateIdTokenCommand> generateIdTokenCommandMock = CreateGenerateIdTokenCommandMock();
            await sut.ExecuteAsync(generateIdTokenCommandMock.Object);

            generateIdTokenCommandMock.Verify(m => m.Validate(It.Is<IValidator>(value => value != null && value == _validatorMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            Mock<IGenerateIdTokenCommand> generateIdTokenCommandMock = CreateGenerateIdTokenCommandMock();
            await sut.ExecuteAsync(generateIdTokenCommandMock.Object);

            generateIdTokenCommandMock.Verify(m => m.ToDomain(
                    It.Is<IAuthorizationStateFactory>(value => value != null && value == _authorizationStateFactoryMock.Object),
                    It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
                    It.Is<ISecurityRepository>(value => value != null && value == _securityRepositoryMock.Object),
                    It.Is<ITrustedDomainResolver>(value => value != null && value == _trustedDomainResolverMock.Object),
                    It.Is<ISupportedScopesProvider>(value => value != null && value == _supportedScopesProviderMock.Object)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertClaimsPrincipalWasCalledOnGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            Mock<IGenerateIdTokenCommand> generateIdTokenCommandMock = CreateGenerateIdTokenCommandMock();
            await sut.ExecuteAsync(generateIdTokenCommandMock.Object);

            generateIdTokenCommandMock.Verify(m => m.ClaimsPrincipal, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandDoesNotContainNameIdentifierClaim_AssertAuthenticationTimeWasNotCalledOnGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: false);
            Mock<IGenerateIdTokenCommand> generateIdTokenCommandMock = CreateGenerateIdTokenCommandMock(claimsPrincipal: claimsPrincipal);
            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommandMock.Object));

            generateIdTokenCommandMock.Verify(m => m.AuthenticationTime, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandDoesNotContainNameIdentifierClaim_AssertScopesWasNotCalledOnAuthorizationStateResolvedByGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: false);
            Mock<IAuthorizationState> authorizationStateMock = _fixture.BuildAuthorizationStateMock();
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal, toDomain: authorizationStateMock.Object);
            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            authorizationStateMock.Verify(m => m.Scopes, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandDoesNotContainNameIdentifierClaim_AssertNonceWasNotCalledOnAuthorizationStateResolvedByGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: false);
            Mock<IAuthorizationState> authorizationStateMock = _fixture.BuildAuthorizationStateMock();
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal, toDomain: authorizationStateMock.Object);
            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            authorizationStateMock.Verify(m => m.Nonce, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandDoesNotContainNameIdentifierClaim_AssertClientIdWasNotCalledOnAuthorizationStateResolvedByGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: false);
            Mock<IAuthorizationState> authorizationStateMock = _fixture.BuildAuthorizationStateMock();
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal, toDomain: authorizationStateMock.Object);
            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            authorizationStateMock.Verify(m => m.ClientId, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandDoesNotContainNameIdentifierClaim_AssertFromPrincipalWasNotCalledOnUserInfoFactory()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal);
            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            _userInfoFactoryMock.Verify(m => m.FromPrincipal(It.IsAny<ClaimsPrincipal>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandDoesNotContainNameIdentifierClaim_AssertSupportedScopesWasNotCalledOnSupportedScopesProvider()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal);
            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            _supportedScopesProviderMock.Verify(m => m.SupportedScopes, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandDoesNotContainNameIdentifierClaim_AssertCreateWasNotCalledOnIdTokenContentFactory()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal);
            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            _idTokenContentFactoryMock.Verify(m => m.Create(
                    It.IsAny<string>(),
                    It.IsAny<IUserInfo>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<IReadOnlyDictionary<string, IScope>>(),
                    It.IsAny<IReadOnlyCollection<string>>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandDoesNotContainNameIdentifierClaim_AssertGenerateWasNotCalledOnTokenGenerator()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal);
            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            _tokenGeneratorMock.Verify(m => m.Generate(
                    It.IsAny<ClaimsIdentity>(),
                    It.IsAny<TimeSpan>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandDoesNotContainNameIdentifierClaim_ThrowsIntranetBusinessException()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal);
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandDoesNotContainNameIdentifierClaim_ThrowsIntranetBusinessExceptionWhereErrorCodeIsEqualToUnableToGenerateIdTokenForAuthenticatedUser()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal);
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.UnableToGenerateIdTokenForAuthenticatedUser));
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandDoesNotContainNameIdentifierClaim_ThrowsIntranetBusinessExceptionWhereMessageIsNotNull()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal);
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandDoesNotContainNameIdentifierClaim_ThrowsIntranetBusinessExceptionWhereMessageIsNotEmpty()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal);
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandDoesNotContainNameIdentifierClaim_ThrowsIntranetBusinessExceptionWhereInnerExceptionIsNull()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal);
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandContainsNameIdentifierClaimWithoutValue_AssertAuthenticationTimeWasNotCalledOnGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            Mock<IGenerateIdTokenCommand> generateIdTokenCommandMock = CreateGenerateIdTokenCommandMock(claimsPrincipal: claimsPrincipal);
            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommandMock.Object));

            generateIdTokenCommandMock.Verify(m => m.AuthenticationTime, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandContainsNameIdentifierClaimWithoutValue_AssertScopesWasNotCalledOnAuthorizationStateResolvedByGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            Mock<IAuthorizationState> authorizationStateMock = _fixture.BuildAuthorizationStateMock();
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal, toDomain: authorizationStateMock.Object);
            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            authorizationStateMock.Verify(m => m.Scopes, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandContainsNameIdentifierClaimWithoutValue_AssertNonceWasNotCalledOnAuthorizationStateResolvedByGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            Mock<IAuthorizationState> authorizationStateMock = _fixture.BuildAuthorizationStateMock();
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal, toDomain: authorizationStateMock.Object);
            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            authorizationStateMock.Verify(m => m.Nonce, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandContainsNameIdentifierClaimWithoutValue_AssertClientIdWasNotCalledOnAuthorizationStateResolvedByGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            Mock<IAuthorizationState> authorizationStateMock = _fixture.BuildAuthorizationStateMock();
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal, toDomain: authorizationStateMock.Object);
            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            authorizationStateMock.Verify(m => m.ClientId, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandContainsNameIdentifierClaimWithoutValue_AssertFromPrincipalWasNotCalledOnUserInfoFactory()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal);
            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            _userInfoFactoryMock.Verify(m => m.FromPrincipal(It.IsAny<ClaimsPrincipal>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandContainsNameIdentifierClaimWithoutValue_AssertSupportedScopesWasNotCalledOnSupportedScopesProvider()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal);
            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            _supportedScopesProviderMock.Verify(m => m.SupportedScopes, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandContainsNameIdentifierClaimWithoutValue_AssertCreateWasNotCalledOnIdTokenContentFactory()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal);
            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            _idTokenContentFactoryMock.Verify(m => m.Create(
                    It.IsAny<string>(),
                    It.IsAny<IUserInfo>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<IReadOnlyDictionary<string, IScope>>(),
                    It.IsAny<IReadOnlyCollection<string>>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandContainsNameIdentifierClaimWithoutValue_AssertGenerateWasNotCalledOnTokenGenerator()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal);
            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            _tokenGeneratorMock.Verify(m => m.Generate(
                    It.IsAny<ClaimsIdentity>(),
                    It.IsAny<TimeSpan>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandContainsNameIdentifierClaimWithoutValue_ThrowsIntranetBusinessException()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal);
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandContainsNameIdentifierClaimWithoutValue_ThrowsIntranetBusinessExceptionWhereErrorCodeIsEqualToUnableToGenerateIdTokenForAuthenticatedUser()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal);
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.UnableToGenerateIdTokenForAuthenticatedUser));
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandContainsNameIdentifierClaimWithoutValue_ThrowsIntranetBusinessExceptionWhereMessageIsNotNull()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal);
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandContainsNameIdentifierClaimWithoutValue_ThrowsIntranetBusinessExceptionWhereMessageIsNotEmpty()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal);
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsPrincipalFromGenerateIdTokenCommandContainsNameIdentifierClaimWithoutValue_ThrowsIntranetBusinessExceptionWhereInnerExceptionIsNull()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal);
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenNameIdentifierForClaimsPrincipalCouldBeResolved_AssertFromPrincipalWasCalledOnUserInfoFactory()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: true);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal);
            await sut.ExecuteAsync(generateIdTokenCommand);

            _userInfoFactoryMock.Verify(m => m.FromPrincipal(It.Is<ClaimsPrincipal>(value => value != null && value == claimsPrincipal)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenUserInfoCouldNotBeResolvedForClaimsPrincipal_AssertAuthenticationTimeWasNotCalledOnGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut(hasUserInfo: false);

            Mock<IGenerateIdTokenCommand> generateIdTokenCommandMock = CreateGenerateIdTokenCommandMock();
            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommandMock.Object));

            generateIdTokenCommandMock.Verify(m => m.AuthenticationTime, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenUserInfoCouldNotBeResolvedForClaimsPrincipal_AssertScopesWasNotCalledOnAuthorizationStateResolvedByGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut(hasUserInfo: false);

            Mock<IAuthorizationState> authorizationStateMock = _fixture.BuildAuthorizationStateMock();
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(toDomain: authorizationStateMock.Object);
            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            authorizationStateMock.Verify(m => m.Scopes, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenUserInfoCouldNotBeResolvedForClaimsPrincipal_AssertNonceWasNotCalledOnAuthorizationStateResolvedByGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut(hasUserInfo: false);

            Mock<IAuthorizationState> authorizationStateMock = _fixture.BuildAuthorizationStateMock();
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(toDomain: authorizationStateMock.Object);
            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            authorizationStateMock.Verify(m => m.Nonce, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenUserInfoCouldNotBeResolvedForClaimsPrincipal_AssertClientIdWasNotCalledOnAuthorizationStateResolvedByGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut(hasUserInfo: false);

            Mock<IAuthorizationState> authorizationStateMock = _fixture.BuildAuthorizationStateMock();
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(toDomain: authorizationStateMock.Object);
            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            authorizationStateMock.Verify(m => m.ClientId, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenUserInfoCouldNotBeResolvedForClaimsPrincipal_AssertSupportedScopesWasNotCalledOnSupportedScopesProvider()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut(hasUserInfo: false);

            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand();
            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            _supportedScopesProviderMock.Verify(m => m.SupportedScopes, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenUserInfoCouldNotBeResolvedForClaimsPrincipal_AssertCreateWasNotCalledOnIdTokenContentFactory()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut(hasUserInfo: false);

            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand();
            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            _idTokenContentFactoryMock.Verify(m => m.Create(
                    It.IsAny<string>(),
                    It.IsAny<IUserInfo>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<IReadOnlyDictionary<string, IScope>>(),
                    It.IsAny<IReadOnlyCollection<string>>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenUserInfoCouldNotBeResolvedForClaimsPrincipal_AssertGenerateWasNotCalledOnTokenGenerator()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut(hasUserInfo: false);

            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand();
            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            _tokenGeneratorMock.Verify(m => m.Generate(
                    It.IsAny<ClaimsIdentity>(),
                    It.IsAny<TimeSpan>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenUserInfoCouldNotBeResolvedForClaimsPrincipal_ThrowsIntranetBusinessException()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut(hasUserInfo: false);

            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand();
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenUserInfoCouldNotBeResolvedForClaimsPrincipal_ThrowsIntranetBusinessExceptionWhereErrorCodeIsEqualToUnableToGenerateIdTokenForAuthenticatedUser()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut(hasUserInfo: false);

            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand();
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.UnableToGenerateIdTokenForAuthenticatedUser));
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenUserInfoCouldNotBeResolvedForClaimsPrincipal_ThrowsIntranetBusinessExceptionWhereMessageIsNotNull()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut(hasUserInfo: false);

            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand();
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenUserInfoCouldNotBeResolvedForClaimsPrincipal_ThrowsIntranetBusinessExceptionWhereMessageIsNotEmpty()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut(hasUserInfo: false);

            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand();
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenUserInfoCouldNotBeResolvedForClaimsPrincipal_ThrowsIntranetBusinessExceptionWhereInnerExceptionIsNull()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut(hasUserInfo: false);

            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand();
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenNameIdentifierAndUserInfoForClaimsPrincipalCouldBeResolved_AssertAuthenticationTimeWasCalledOnGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            Mock<IGenerateIdTokenCommand> generateIdTokenCommandMock = CreateGenerateIdTokenCommandMock();
            await sut.ExecuteAsync(generateIdTokenCommandMock.Object);

            generateIdTokenCommandMock.Verify(m => m.AuthenticationTime, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenNameIdentifierAndUserInfoForClaimsPrincipalCouldBeResolved_AssertScopesWasCalledOnAuthorizationStateResolvedByGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            Mock<IAuthorizationState> authorizationStateMock = _fixture.BuildAuthorizationStateMock();
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(toDomain: authorizationStateMock.Object);
            await sut.ExecuteAsync(generateIdTokenCommand);

            authorizationStateMock.Verify(m => m.Scopes, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenNameIdentifierAndUserInfoForClaimsPrincipalCouldBeResolved_AssertSupportedScopesCalledOnSupportedScopesProvider()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand();
            await sut.ExecuteAsync(generateIdTokenCommand);

            _supportedScopesProviderMock.Verify(m => m.SupportedScopes, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenNameIdentifierAndUserInfoForClaimsPrincipalCouldBeResolved_AssertCreateWasCalledOnIdTokenContentFactoryWithResolvedNameIdentifier()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            string nameIdentifierClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: true, nameIdentifierClaimValue: nameIdentifierClaimValue);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal);
            await sut.ExecuteAsync(generateIdTokenCommand);

            _idTokenContentFactoryMock.Verify(m => m.Create(
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, nameIdentifierClaimValue) == 0),
                    It.IsAny<IUserInfo>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<IReadOnlyDictionary<string, IScope>>(),
                    It.IsAny<IReadOnlyCollection<string>>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenNameIdentifierAndUserInfoForClaimsPrincipalCouldBeResolved_AssertCreateWasCalledOnIdTokenContentFactoryWithResolvedUserInfo()
        {
            IUserInfo userInfo = _fixture.BuildUserInfoMock().Object;
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut(userInfo: userInfo);

            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand();
            await sut.ExecuteAsync(generateIdTokenCommand);

            _idTokenContentFactoryMock.Verify(m => m.Create(
                    It.IsAny<string>(),
                    It.Is<IUserInfo>(value => value != null && value == userInfo),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<IReadOnlyDictionary<string, IScope>>(),
                    It.IsAny<IReadOnlyCollection<string>>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenNameIdentifierAndUserInfoForClaimsPrincipalCouldBeResolved_AssertCreateWasCalledOnIdTokenContentFactoryWithAuthenticationTimeFromGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            DateTimeOffset authenticationTime = DateTimeOffset.UtcNow.AddSeconds(_random.Next(300) * -1);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(authenticationTime: authenticationTime);
            await sut.ExecuteAsync(generateIdTokenCommand);

            _idTokenContentFactoryMock.Verify(m => m.Create(
                    It.IsAny<string>(),
                    It.IsAny<IUserInfo>(),
                    It.Is<DateTimeOffset>(value => value == authenticationTime),
                    It.IsAny<IReadOnlyDictionary<string, IScope>>(),
                    It.IsAny<IReadOnlyCollection<string>>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenNameIdentifierAndUserInfoForClaimsPrincipalCouldBeResolved_AssertCreateWasCalledOnIdTokenContentFactoryWithSupportedScopesResolvedBySupportedScopesProvider()
        {
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(_fixture.BuildScopeMock(name: ScopeHelper.WebApiScope).Object, _fixture.BuildScopeMock().Object, _fixture.BuildScopeMock().Object);
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut(supportedScopes: supportedScopes);

            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand();
            await sut.ExecuteAsync(generateIdTokenCommand);

            _idTokenContentFactoryMock.Verify(m => m.Create(
                    It.IsAny<string>(),
                    It.IsAny<IUserInfo>(),
                    It.IsAny<DateTimeOffset>(),
                    It.Is<IReadOnlyDictionary<string, IScope>>(value => value != null && value == supportedScopes),
                    It.IsAny<IReadOnlyCollection<string>>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenNameIdentifierAndUserInfoForClaimsPrincipalCouldBeResolved_AssertCreateWasCalledOnIdTokenContentFactoryWithScopesFromAuthorizationStateResolvedByGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            string[] scopes = _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray();
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock(scopes: scopes).Object;
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(toDomain: authorizationState);
            await sut.ExecuteAsync(generateIdTokenCommand);

            _idTokenContentFactoryMock.Verify(m => m.Create(
                    It.IsAny<string>(),
                    It.IsAny<IUserInfo>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<IReadOnlyDictionary<string, IScope>>(),
                    It.Is<IReadOnlyCollection<string>>(value => value != null && scopes.All(value.Contains))),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenIdTokenContentBuilderWasCreatedByIdTokenContentFactory_AssertNonceWasCalledOnAuthorizationStateResolvedByGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            Mock<IAuthorizationState> authorizationStateMock = _fixture.BuildAuthorizationStateMock();
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(toDomain: authorizationStateMock.Object);
            await sut.ExecuteAsync(generateIdTokenCommand);

            authorizationStateMock.Verify(m => m.Nonce, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenNonceOnAuthorizationStateResolvedByGenerateIdTokenCommandHasValue_AssertWithNonceWasCalledIdTokenContentBuilderWithValueForNonce()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            string nonce = _fixture.Create<string>();
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock(hasNonce: true, nonce: nonce).Object;
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(toDomain: authorizationState);
            await sut.ExecuteAsync(generateIdTokenCommand);

            _idTokenContentBuilderMock.Verify(m => m.WithNonce(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, nonce) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenNonceOnAuthorizationStateResolvedByGenerateIdTokenCommandHasNoValue_AssertWithNonceWasNotCalledIdTokenContentBuilder()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock(hasNonce: false).Object;
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(toDomain: authorizationState);
            await sut.ExecuteAsync(generateIdTokenCommand);

            _idTokenContentBuilderMock.Verify(m => m.WithNonce(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenIdTokenContentBuilderWasCreatedByIdTokenContentFactory_AssertWithCustomClaimsFilteredByScopeWasCalledOnIdTokenContentBuilderWithScopeForWebApiScope()
        {
            IScope webApiScope = _fixture.BuildScopeMock(name: ScopeHelper.WebApiScope).Object;
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(webApiScope, _fixture.BuildScopeMock().Object, _fixture.BuildScopeMock().Object);
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut(supportedScopes: supportedScopes);

            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand();
            await sut.ExecuteAsync(generateIdTokenCommand);

            _idTokenContentBuilderMock.Verify(m => m.WithCustomClaimsFilteredByScope(
                    It.Is<IScope>(value => value != null && value == webApiScope),
                    It.IsAny<IEnumerable<Claim>>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenIdTokenContentBuilderWasCreatedByIdTokenContentFactory_AssertWithCustomClaimsFilteredByScopeWasCalledOnIdTokenContentBuilderWithClaimsOnClaimsPrincipalResolvedByGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal);
            await sut.ExecuteAsync(generateIdTokenCommand);

            _idTokenContentBuilderMock.Verify(m => m.WithCustomClaimsFilteredByScope(
                    It.IsAny<IScope>(),
                    It.Is<IEnumerable<Claim>>(value => value != null && claimsPrincipal.Claims.All(claim => value.SingleOrDefault(c => c != null && string.IsNullOrWhiteSpace(c.Type) == false && string.CompareOrdinal(c.Type, claim.Type) == 0 && string.IsNullOrWhiteSpace(c.Value) == false && string.CompareOrdinal(c.Value, claim.Value) == 0) != null))),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenIdTokenContentBuilderWasCreatedByIdTokenContentFactory_AssertWithCustomClaimsFilteredByClaimTypeWasCalledOnIdTokenContentBuilderWithClaimTypeForMicrosoftTokenAndClaimsOnClaimsPrincipalResolvedByGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal);
            await sut.ExecuteAsync(generateIdTokenCommand);

            _idTokenContentBuilderMock.Verify(m => m.WithCustomClaimsFilteredByClaimType(
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, ClaimHelper.MicrosoftTokenClaimType) == 0),
                    It.Is<IEnumerable<Claim>>(value => value != null && claimsPrincipal.Claims.All(claim => value.SingleOrDefault(c => c != null && string.IsNullOrWhiteSpace(c.Type) == false && string.CompareOrdinal(c.Type, claim.Type) == 0 && string.IsNullOrWhiteSpace(c.Value) == false && string.CompareOrdinal(c.Value, claim.Value) == 0) != null))),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenIdTokenContentBuilderWasCreatedByIdTokenContentFactory_AssertWithCustomClaimsFilteredByClaimTypeWasCalledOnIdTokenContentBuilderWithClaimTypeForGoogleTokenAndClaimsOnClaimsPrincipalResolvedByGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsPrincipal: claimsPrincipal);
            await sut.ExecuteAsync(generateIdTokenCommand);

            _idTokenContentBuilderMock.Verify(m => m.WithCustomClaimsFilteredByClaimType(
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, ClaimHelper.GoogleTokenClaimType) == 0),
                    It.Is<IEnumerable<Claim>>(value => value != null && claimsPrincipal.Claims.All(claim => value.SingleOrDefault(c => c != null && string.IsNullOrWhiteSpace(c.Type) == false && string.CompareOrdinal(c.Type, claim.Type) == 0 && string.IsNullOrWhiteSpace(c.Value) == false && string.CompareOrdinal(c.Value, claim.Value) == 0) != null))),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenIdTokenContentBuilderWasCreatedByIdTokenContentFactory_AssertBuildWasCalledOnIdTokenContentBuilder()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand();
            await sut.ExecuteAsync(generateIdTokenCommand);

            _idTokenContentBuilderMock.Verify(m => m.Build(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimsWasBuildByIdTokenContentBuilder_AssertClientIdWasCalledOnAuthorizationStateResolvedByGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            Mock<IAuthorizationState> authorizationStateMock = _fixture.BuildAuthorizationStateMock();
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(toDomain: authorizationStateMock.Object);
            await sut.ExecuteAsync(generateIdTokenCommand);

            authorizationStateMock.Verify(m => m.ClientId, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimsWasBuildByIdTokenContentBuilder_AssertGenerateWasCalledOnTokenGeneratorWithClaimsIdentityContainingClaimsBuildByIdTokenContentBuilder()
        {
            Claim[] claimsBuildByIdTokenContentBuilder = _fixture.CreateClaims(_random);
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut(claimsBuildByIdTokenContentBuilder: claimsBuildByIdTokenContentBuilder);

            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand();
            await sut.ExecuteAsync(generateIdTokenCommand);

            _tokenGeneratorMock.Verify(m => m.Generate(It.Is<ClaimsIdentity>(value => value != null && claimsBuildByIdTokenContentBuilder.All(claim => value.HasClaim(claim.Type, claim.Value))),
                    It.IsAny<TimeSpan>(),
                    It.IsAny<string>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimsWasBuildByIdTokenContentBuilder_AssertGenerateWasCalledOnTokenGeneratorWithExpiresInEqualToFiveMinutes()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand();
            await sut.ExecuteAsync(generateIdTokenCommand);

            _tokenGeneratorMock.Verify(m => m.Generate(
                    It.IsAny<ClaimsIdentity>(),
                    It.Is<TimeSpan>(value => (int) value.TotalSeconds == 300),
                    It.IsAny<string>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimsWasBuildByIdTokenContentBuilder_AssertGenerateWasCalledOnTokenGeneratorWithClientIdFromAuthorizationStateResolvedByGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            string clientId = _fixture.Create<string>();
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock(clientId: clientId).Object;
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(toDomain: authorizationState);
            await sut.ExecuteAsync(generateIdTokenCommand);

            _tokenGeneratorMock.Verify(m => m.Generate(
                    It.IsAny<ClaimsIdentity>(),
                    It.IsAny<TimeSpan>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, clientId) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimsWasBuildByIdTokenContentBuilder_ReturnsNotNull()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand();
            IToken result = await sut.ExecuteAsync(generateIdTokenCommand);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimsWasBuildByIdTokenContentBuilder_ReturnsTokenFromTokenGenerator()
        {
            IToken token = _fixture.BuildTokenMock().Object;
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut(token: token);

            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand();
            IToken result = await sut.ExecuteAsync(generateIdTokenCommand);

            Assert.That(result, Is.EqualTo(token));
        }

        private ICommandHandler<IGenerateIdTokenCommand, IToken> CreateSut(bool hasUserInfo = true, IUserInfo userInfo = null, IReadOnlyDictionary<string, IScope> supportedScopes = null, IEnumerable<Claim> claimsBuildByIdTokenContentBuilder = null, IToken token = null)
        {
            _userInfoFactoryMock.Setup(m => m.FromPrincipal(It.IsAny<ClaimsPrincipal>()))
                .Returns(hasUserInfo ? userInfo ?? _fixture.BuildUserInfoMock().Object : null);

            _supportedScopesProviderMock.Setup(m => m.SupportedScopes)
                .Returns(supportedScopes ?? CreateSupportedScopes(_fixture.BuildScopeMock(name: ScopeHelper.WebApiScope).Object, _fixture.BuildScopeMock().Object, _fixture.BuildScopeMock().Object));

            _idTokenContentFactoryMock.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<IUserInfo>(), It.IsAny<DateTimeOffset>(), It.IsAny<IReadOnlyDictionary<string, IScope>>(), It.IsAny<IReadOnlyCollection<string>>()))
                .Returns(_idTokenContentBuilderMock.Object);

            _idTokenContentBuilderMock.Setup(m => m.WithNonce(It.IsAny<string>()))
                .Returns(_idTokenContentBuilderMock.Object);
            _idTokenContentBuilderMock.Setup(m => m.WithCustomClaimsFilteredByScope(It.IsAny<IScope>(), It.IsAny<IEnumerable<Claim>>()))
                .Returns(_idTokenContentBuilderMock.Object);
            _idTokenContentBuilderMock.Setup(m => m.WithCustomClaimsFilteredByClaimType(It.IsAny<string>(), It.IsAny<IEnumerable<Claim>>()))
                .Returns(_idTokenContentBuilderMock.Object);
            _idTokenContentBuilderMock.Setup(m => m.Build())
                .Returns(claimsBuildByIdTokenContentBuilder ?? _fixture.CreateClaims(_random));

            _tokenGeneratorMock.Setup(m => m.Generate(It.IsAny<ClaimsIdentity>(), It.IsAny<TimeSpan>(), It.IsAny<string>()))
                .Returns(token ?? _fixture.BuildTokenMock().Object);

            return new BusinessLogic.Security.CommandHandlers.GenerateIdTokenCommandHandler(_validatorMock.Object, _authorizationStateFactoryMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object, _userInfoFactoryMock.Object, _idTokenContentFactoryMock.Object, _tokenGeneratorMock.Object);
        }

        private IGenerateIdTokenCommand CreateGenerateIdTokenCommand(ClaimsPrincipal claimsPrincipal = null, DateTimeOffset? authenticationTime = null, IAuthorizationState toDomain = null)
        {
            return CreateGenerateIdTokenCommandMock(claimsPrincipal, authenticationTime, toDomain).Object;
        }

        private Mock<IGenerateIdTokenCommand> CreateGenerateIdTokenCommandMock(ClaimsPrincipal claimsPrincipal = null, DateTimeOffset? authenticationTime = null, IAuthorizationState toDomain = null)
        {
            Mock<IGenerateIdTokenCommand> generateIdTokenCommandMock = new Mock<IGenerateIdTokenCommand>();
            generateIdTokenCommandMock.Setup(m => m.ClaimsPrincipal)
                .Returns(claimsPrincipal ?? CreateClaimsPrincipal());
            generateIdTokenCommandMock.Setup(m => m.AuthenticationTime)
                .Returns(authenticationTime ?? DateTimeOffset.UtcNow.AddSeconds(_random.Next(300) * -1));
            generateIdTokenCommandMock.Setup(m => m.ToDomain(It.IsAny<IAuthorizationStateFactory>(), It.IsAny<IValidator>(), It.IsAny<ISecurityRepository>(), It.IsAny<ITrustedDomainResolver>(), It.IsAny<ISupportedScopesProvider>()))
                .Returns(toDomain ?? _fixture.BuildAuthorizationStateMock().Object);
            return generateIdTokenCommandMock;
        }

        private ClaimsPrincipal CreateClaimsPrincipal(bool hasNameIdentifierClaim = true, bool hasNameIdentifierClaimValue = true, string nameIdentifierClaimValue = null)
        {
            return new ClaimsPrincipal(CreateClaimsIdentity(hasNameIdentifierClaim, hasNameIdentifierClaimValue, nameIdentifierClaimValue));
        }

        private ClaimsIdentity CreateClaimsIdentity(bool hasNameIdentifierClaim = true, bool hasNameIdentifierClaimValue = true, string nameIdentifierClaimValue = null)
        {
            List<Claim> claims = new List<Claim>();
            if (hasNameIdentifierClaim)
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, hasNameIdentifierClaimValue ? nameIdentifierClaimValue ?? _fixture.Create<string>() : string.Empty));
            }

            claims.AddRange(_fixture.CreateClaims(_random));

            return new ClaimsIdentity(claims);
        }

        private static IReadOnlyDictionary<string, IScope> CreateSupportedScopes(params IScope[] scopes)
        {
            NullGuard.NotNull(scopes, nameof(scopes));

            return scopes.ToDictionary(scope => scope.Name, scope => scope).AsReadOnly();
        }
    }
}