using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Extensions;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
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

        private Mock<IIdTokenContentFactory> _idTokenContentFactoryMock;
        private Mock<IIdTokenContentBuilder> _idTokenContentBuilderMock;
        private Mock<ITokenGenerator> _tokenGeneratorMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
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
            Assert.That(result.ParamName, Is.EqualTo("generateIdTokenCommand"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertClaimsIdentityWasCalledOnGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsIdentity claimsIdentity = CreateClaimsIdentity();
            Mock<IGenerateIdTokenCommand> generateIdTokenCommandMock = CreateGenerateIdTokenCommandMock(claimsIdentity: claimsIdentity);
            await sut.ExecuteAsync(generateIdTokenCommandMock.Object);

            generateIdTokenCommandMock.Verify(m => m.ClaimsIdentity, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsIdentityFromGenerateIdTokenCommandDoesNotContainNameIdentifierClaim_ThrowsIntranetBusinessException()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(hasNameIdentifierClaim: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsIdentity: claimsIdentity);
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsIdentityFromGenerateIdTokenCommandDoesNotContainNameIdentifierClaim_ThrowsIntranetBusinessExceptionWhereErrorCodeIsEqualToUnableToGenerateIdTokenForAuthenticatedUser()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(hasNameIdentifierClaim: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsIdentity: claimsIdentity);
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.UnableToGenerateIdTokenForAuthenticatedUser));
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsIdentityFromGenerateIdTokenCommandDoesNotContainNameIdentifierClaim_ThrowsIntranetBusinessExceptionWhereMessageIsNotNull()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(hasNameIdentifierClaim: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsIdentity: claimsIdentity);
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsIdentityFromGenerateIdTokenCommandDoesNotContainNameIdentifierClaim_ThrowsIntranetBusinessExceptionWhereMessageIsNotEmpty()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(hasNameIdentifierClaim: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsIdentity: claimsIdentity);
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsIdentityFromGenerateIdTokenCommandDoesNotContainNameIdentifierClaim_ThrowsIntranetBusinessExceptionWhereInnerExceptionIsNull()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(hasNameIdentifierClaim: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsIdentity: claimsIdentity);
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsIdentityFromGenerateIdTokenCommandContainsNameIdentifierClaimWithoutValue_ThrowsIntranetBusinessException()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsIdentity: claimsIdentity);
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsIdentityFromGenerateIdTokenCommandContainsNameIdentifierClaimWithoutValue_ThrowsIntranetBusinessExceptionWhereErrorCodeIsEqualToUnableToGenerateIdTokenForAuthenticatedUser()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsIdentity: claimsIdentity);
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.UnableToGenerateIdTokenForAuthenticatedUser));
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsIdentityFromGenerateIdTokenCommandContainsNameIdentifierClaimWithoutValue_ThrowsIntranetBusinessExceptionWhereMessageIsNotNull()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsIdentity: claimsIdentity);
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsIdentityFromGenerateIdTokenCommandContainsNameIdentifierClaimWithoutValue_ThrowsIntranetBusinessExceptionWhereMessageIsNotEmpty()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsIdentity: claimsIdentity);
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenClaimsIdentityFromGenerateIdTokenCommandContainsNameIdentifierClaimWithoutValue_ThrowsIntranetBusinessExceptionWhereInnerExceptionIsNull()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsIdentity: claimsIdentity);
            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(generateIdTokenCommand));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimsIdentityFromGenerateIdTokenCommandContainsNameIdentifierClaimWithValue_AssertAuthenticationTimeWasCalledOnGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: true);
            Mock<IGenerateIdTokenCommand> generateIdTokenCommandMock = CreateGenerateIdTokenCommandMock(claimsIdentity: claimsIdentity);
            await sut.ExecuteAsync(generateIdTokenCommandMock.Object);

            generateIdTokenCommandMock.Verify(m => m.AuthenticationTime, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimsIdentityFromGenerateIdTokenCommandContainsNameIdentifierClaimWithValue_AssertCreateWasCalledOnIdTokenContentFactoryWithComputedHashForValueInNameIdentifierClaim()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            string nameIdentifierClaimValue = _fixture.Create<string>();
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: true, nameIdentifierClaimValue: nameIdentifierClaimValue);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsIdentity: claimsIdentity);
            await sut.ExecuteAsync(generateIdTokenCommand);

            _idTokenContentFactoryMock.Verify(m => m.Create(
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, nameIdentifierClaimValue.ComputeSha512Hash()) == 0),
                    It.IsAny<DateTimeOffset>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClaimsIdentityFromGenerateIdTokenCommandContainsNameIdentifierClaimWithValue_AssertCreateWasCalledOnIdTokenContentFactoryWithAuthenticationTimeFromGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: true);
            DateTimeOffset authenticationTime = DateTimeOffset.UtcNow.AddSeconds(_random.Next(300) * -1);
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(claimsIdentity: claimsIdentity, authenticationTime: authenticationTime);
            await sut.ExecuteAsync(generateIdTokenCommand);

            _idTokenContentFactoryMock.Verify(m => m.Create(
                    It.IsAny<string>(),
                    It.Is<DateTimeOffset>(value => value == authenticationTime)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenIdTokenContentBuilderHasBeenCreatedByIdTokenContentFactory_AssertNonceWasCalledOnGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            Mock<IGenerateIdTokenCommand> generateIdTokenCommandMock = CreateGenerateIdTokenCommandMock();
            await sut.ExecuteAsync(generateIdTokenCommandMock.Object);

            generateIdTokenCommandMock.Verify(m => m.Nonce, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenIdTokenContentBuilderHasBeenCreatedByIdTokenContentFactoryAndNonceWasSetInGenerateIdTokenCommand_AssertWithNonceWasCalledOnIdTokenContentBuilderWithNonceFromGenerateIdTokenCommand()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            string nonce = _fixture.Create<string>();
            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(hasNonce: true, nonce: nonce);
            await sut.ExecuteAsync(generateIdTokenCommand);

            _idTokenContentBuilderMock.Verify(m => m.WithNonce(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, nonce) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenIdTokenContentBuilderHasBeenCreatedByIdTokenContentFactoryAndNonceWasNotSetInGenerateIdTokenCommand_AssertWithNonceWasNotCalledOnIdTokenContentBuilder()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand(hasNonce: false);
            await sut.ExecuteAsync(generateIdTokenCommand);

            _idTokenContentBuilderMock.Verify(m => m.WithNonce(It.IsAny<string>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenIdTokenContentBuilderHasBeenCreatedByIdTokenContentFactory_AssertBuildWasCalledOnIdTokenContentBuilder()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand();
            await sut.ExecuteAsync(generateIdTokenCommand);

            _idTokenContentBuilderMock.Verify(m => m.Build(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenIdTokenContentWasBuildByIdTokenContentBuilder_AssertGenerateWasCalledOnTokenGeneratorWithIdTokeContent()
        {
            Claim[] claims = _fixture.CreateClaims(_random);
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut(claims: claims);

            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand();
            await sut.ExecuteAsync(generateIdTokenCommand);

            _tokenGeneratorMock.Verify(m => m.Generate(It.Is<ClaimsIdentity>(value => value != null && claims.All(claim => value.HasClaim(claim.Type, claim.Value)))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenIdTokenContentWasBuildByIdTokenContentBuilder_ReturnsNotNull()
        {
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut();

            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand();
            IToken result = await sut.ExecuteAsync(generateIdTokenCommand);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenIdTokenContentWasBuildByIdTokenContentBuilder_ReturnsTokenFromTokenGenerator()
        {
            IToken token = _fixture.BuildTokenMock().Object;
            ICommandHandler<IGenerateIdTokenCommand, IToken> sut = CreateSut(token: token);

            IGenerateIdTokenCommand generateIdTokenCommand = CreateGenerateIdTokenCommand();
            IToken result = await sut.ExecuteAsync(generateIdTokenCommand);

            Assert.That(result, Is.EqualTo(token));
        }

        private ICommandHandler<IGenerateIdTokenCommand, IToken> CreateSut(IEnumerable<Claim> claims = null, IToken token = null)
        {
            _idTokenContentFactoryMock.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<DateTimeOffset>()))
                .Returns(_idTokenContentBuilderMock.Object);

            _idTokenContentBuilderMock.Setup(m => m.WithNonce(It.IsAny<string>()))
                .Returns(_idTokenContentBuilderMock.Object);
            _idTokenContentBuilderMock.Setup(m => m.Build())
                .Returns(claims ?? _fixture.CreateClaims(_random));

            _tokenGeneratorMock.Setup(m => m.Generate(It.IsAny<ClaimsIdentity>()))
                .Returns(token ?? _fixture.BuildTokenMock().Object);

            return new BusinessLogic.Security.CommandHandlers.GenerateIdTokenCommandHandler(_idTokenContentFactoryMock.Object, _tokenGeneratorMock.Object);
        }

        private IGenerateIdTokenCommand CreateGenerateIdTokenCommand(ClaimsIdentity claimsIdentity = null, DateTimeOffset? authenticationTime = null, bool hasNonce = true, string nonce = null)
        {
            return CreateGenerateIdTokenCommandMock(claimsIdentity, authenticationTime, hasNonce, nonce).Object;
        }

        private Mock<IGenerateIdTokenCommand> CreateGenerateIdTokenCommandMock(ClaimsIdentity claimsIdentity = null, DateTimeOffset? authenticationTime = null, bool hasNonce = true, string nonce = null)
        {
            Mock<IGenerateIdTokenCommand> generateIdTokenCommandMock = new Mock<IGenerateIdTokenCommand>();
            generateIdTokenCommandMock.Setup(m => m.ClaimsIdentity)
                .Returns(claimsIdentity ?? CreateClaimsIdentity());
            generateIdTokenCommandMock.Setup(m => m.AuthenticationTime)
                .Returns(authenticationTime ?? DateTimeOffset.UtcNow.AddSeconds(_random.Next(300) * -1));
            generateIdTokenCommandMock.Setup(m => m.Nonce)
                .Returns(hasNonce ? nonce ?? _fixture.Create<string>() : null);
            return generateIdTokenCommandMock;
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
    }
}