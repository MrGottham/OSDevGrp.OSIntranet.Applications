﻿using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Extensions;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.CommandHandlers.AuthenticateUserCommandHandler
{
    [TestFixture]
    public class ExecuteAsyncTests
    {
	    #region Private variables

	    private Mock<ISecurityRepository> _securityRepositoryMock;
	    private Mock<IExternalTokenClaimCreator> _externalTokenClaimCreatorMock;
	    private Fixture _fixture;
	    private Random _random;

	    #endregion

	    [SetUp]
	    public void SetUp()
	    {
		    _securityRepositoryMock = new Mock<ISecurityRepository>();
		    _externalTokenClaimCreatorMock = new Mock<IExternalTokenClaimCreator>();
		    _fixture = new Fixture();
		    _random = new Random(_fixture.Create<int>());
	    }

	    [Test]
	    [Category("UnitTest")]
	    public void ExecuteAsync_WhenAuthenticateUserCommandIsNull_ThrowsArgumentNullException()
	    {
		    ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut();

		    ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

		    Assert.That(result, Is.Not.Null);
		    Assert.That(result.ParamName, Is.EqualTo("authenticateCommand"));
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenCalled_AssertExternalUserIdentifierWasCalledOnAuthenticateUserCommand()
	    {
		    ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut();

		    Mock<IAuthenticateUserCommand> authenticateUserCommandMock = CreateAuthenticateUserCommandMock();
		    await sut.ExecuteAsync(authenticateUserCommandMock.Object);

		    authenticateUserCommandMock.Verify(m => m.ExternalUserIdentifier, Times.Once);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenCalled_AssertGetUserIdentityAsyncWasCalledOnSecurityRepository()
	    {
		    ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut();

		    string externalUserIdentifier = _fixture.Create<string>();
		    IAuthenticateUserCommand authenticateUserCommand = CreateAuthenticateUserCommand(externalUserIdentifier);
		    await sut.ExecuteAsync(authenticateUserCommand);

		    _securityRepositoryMock.Verify(m => m.GetUserIdentityAsync(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, externalUserIdentifier) == 0)), Times.Once);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenNoUserIdentityWasReturnedForExternalUserIdentifier_AssertClaimsWasNotCalledOnAuthenticateUserCommand()
	    {
		    ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut(hasUserIdentityForExternalUserIdentifier: false);

		    Mock<IAuthenticateUserCommand> authenticateUserCommandMock = CreateAuthenticateUserCommandMock();
		    await sut.ExecuteAsync(authenticateUserCommandMock.Object);

		    authenticateUserCommandMock.Verify(m => m.Claims, Times.Never);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenNoUserIdentityWasReturnedForExternalUserIdentifier_AssertAuthenticationSessionItemsWasNotCalledOnAuthenticateUserCommand()
	    {
		    ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut(hasUserIdentityForExternalUserIdentifier: false);

		    Mock<IAuthenticateUserCommand> authenticateUserCommandMock = CreateAuthenticateUserCommandMock();
		    await sut.ExecuteAsync(authenticateUserCommandMock.Object);

		    authenticateUserCommandMock.Verify(m => m.AuthenticationSessionItems, Times.Never);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenNoUserIdentityWasReturnedForExternalUserIdentifier_AssertProtectorWasNotCalledOnAuthenticateUserCommand()
	    {
		    ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut(hasUserIdentityForExternalUserIdentifier: false);

		    Mock<IAuthenticateUserCommand> authenticateUserCommandMock = CreateAuthenticateUserCommandMock();
		    await sut.ExecuteAsync(authenticateUserCommandMock.Object);

		    authenticateUserCommandMock.Verify(m => m.Protector, Times.Never);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenNoUserIdentityWasReturnedForExternalUserIdentifier_AssertCanBuildWasNotCalledOnExternalTokenClaimCreator()
	    {
		    ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut(hasUserIdentityForExternalUserIdentifier: false);

		    IAuthenticateUserCommand authenticateUserCommand = CreateAuthenticateUserCommand();
		    await sut.ExecuteAsync(authenticateUserCommand);

		    _externalTokenClaimCreatorMock.Verify(m => m.CanBuild(It.IsAny<IReadOnlyDictionary<string, string>>()), Times.Never);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenNoUserIdentityWasReturnedForExternalUserIdentifier_AssertBuildWasNotCalledOnExternalTokenClaimCreator()
	    {
		    ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut(hasUserIdentityForExternalUserIdentifier: false);

		    IAuthenticateUserCommand authenticateUserCommand = CreateAuthenticateUserCommand();
		    await sut.ExecuteAsync(authenticateUserCommand);

		    _externalTokenClaimCreatorMock.Verify(m => m.Build(
				    It.IsAny<IReadOnlyDictionary<string, string>>(),
				    It.IsAny<Func<string, string>>()),
			    Times.Never);
	    }

		[Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenNoUserIdentityWasReturnedForExternalUserIdentifier_AssertAuthenticationTypeWasNotCalledOnAuthenticateUserCommand()
	    {
		    ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut(hasUserIdentityForExternalUserIdentifier: false);

		    Mock<IAuthenticateUserCommand> authenticateUserCommandMock = CreateAuthenticateUserCommandMock();
		    await sut.ExecuteAsync(authenticateUserCommandMock.Object);

		    authenticateUserCommandMock.Verify(m => m.AuthenticationType, Times.Never);
	    }

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenUserIdentityWasReturnedForExternalUserIdentifier_AssertClaimsWasCalledOnAuthenticateUserCommand()
		{
			ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut();

			Mock<IAuthenticateUserCommand> authenticateUserCommandMock = CreateAuthenticateUserCommandMock();
			await sut.ExecuteAsync(authenticateUserCommandMock.Object);

			authenticateUserCommandMock.Verify(m => m.Claims, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenUserIdentityWasReturnedForExternalUserIdentifier_AssertAuthenticationSessionItemsWasCalledOnAuthenticateUserCommand()
		{
			ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut();

			Mock<IAuthenticateUserCommand> authenticateUserCommandMock = CreateAuthenticateUserCommandMock();
			await sut.ExecuteAsync(authenticateUserCommandMock.Object);

			authenticateUserCommandMock.Verify(m => m.AuthenticationSessionItems, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenUserIdentityWasReturnedForExternalUserIdentifier_AssertProtectorWasCalledOnAuthenticateUserCommand()
		{
			ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut();

			Mock<IAuthenticateUserCommand> authenticateUserCommandMock = CreateAuthenticateUserCommandMock();
			await sut.ExecuteAsync(authenticateUserCommandMock.Object);

			authenticateUserCommandMock.Verify(m => m.Protector, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenUserIdentityWasReturnedForExternalUserIdentifier_AssertCanBuildWasCalledOnExternalTokenClaimCreator()
		{
			ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut();

			IReadOnlyDictionary<string, string> authenticationSessionItems = new ConcurrentDictionary<string, string>();
			IAuthenticateUserCommand authenticateUserCommand = CreateAuthenticateUserCommand(authenticationSessionItems: authenticationSessionItems);
			await sut.ExecuteAsync(authenticateUserCommand);

			_externalTokenClaimCreatorMock.Verify(m => m.CanBuild(It.Is<IReadOnlyDictionary<string, string>>(value => value != null && value == authenticationSessionItems)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenUserIdentityWasReturnedForExternalUserIdentifierAndExternalTokenClaimCouldNotBeBuild_AssertBuildWasNotCalledOnExternalTokenClaimCreator()
		{
			ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut(canBuildExternalTokenClaim: false);

			IAuthenticateUserCommand authenticateUserCommand = CreateAuthenticateUserCommand();
			await sut.ExecuteAsync(authenticateUserCommand);

			_externalTokenClaimCreatorMock.Verify(m => m.Build(
					It.IsAny<IReadOnlyDictionary<string, string>>(),
					It.IsAny<Func<string, string>>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenUserIdentityWasReturnedForExternalUserIdentifierAndExternalTokenClaimCouldBeBuild_AssertBuildWasCalledOnExternalTokenClaimCreator()
		{
			ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut();

			IReadOnlyDictionary<string, string> authenticationSessionItems = new ConcurrentDictionary<string, string>();
			Func<string, string> protector = value => value;
			IAuthenticateUserCommand authenticateUserCommand = CreateAuthenticateUserCommand(authenticationSessionItems: authenticationSessionItems, protector: protector);
			await sut.ExecuteAsync(authenticateUserCommand);

			_externalTokenClaimCreatorMock.Verify(m => m.Build(
					It.Is<IReadOnlyDictionary<string, string>>(value => value != null && value == authenticationSessionItems),
					It.Is<Func<string, string>>(value => value != null && value == protector)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenUserIdentityWasReturnedForExternalUserIdentifier_AssertAuthenticationTypeWasCalledOnAuthenticateUserCommand()
		{
			ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut();

			Mock<IAuthenticateUserCommand> authenticateUserCommandMock = CreateAuthenticateUserCommandMock();
			await sut.ExecuteAsync(authenticateUserCommandMock.Object);

			authenticateUserCommandMock.Verify(m => m.AuthenticationType, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task ExecuteAsync_WhenUserIdentityWasReturnedForExternalUserIdentifier_AssertAddClaimsWasCalledOnUserIdentityForExternalUserIdentifierWithAllClaimsFromAuthenticateUserCommand(bool canBuildExternalTokenClaim)
		{
			Mock<IUserIdentity> userIdentityMock = _fixture.BuildUserIdentityMock();
			ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut(userIdentity: userIdentityMock.Object, canBuildExternalTokenClaim: canBuildExternalTokenClaim);

			IReadOnlyCollection<Claim> claims = BuildClaimCollection();
			IAuthenticateUserCommand authenticateUserCommand = CreateAuthenticateUserCommand(claims: claims);
			await sut.ExecuteAsync(authenticateUserCommand);

			userIdentityMock.Verify(m => m.AddClaims(It.Is<IEnumerable<Claim>>(value => value != null && claims.All(claim => value.Any(v => IsMatch(v, claim.Type, CalculateExpectedClaimValue(claim)))))), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenUserIdentityWasReturnedForExternalUserIdentifierAndExternalTokenClaimCouldNotBeBuild_AssertAddClaimsWasCalledOnUserIdentityForExternalUserIdentifierWithoutExternalTokenClaim()
		{
			Mock<IUserIdentity> userIdentityMock = _fixture.BuildUserIdentityMock();
			ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut(userIdentity: userIdentityMock.Object, canBuildExternalTokenClaim: false);

			IAuthenticateUserCommand authenticateUserCommand = CreateAuthenticateUserCommand(claims: BuildEmptyClaimCollection());
			await sut.ExecuteAsync(authenticateUserCommand);

			userIdentityMock.Verify(m => m.AddClaims(It.Is<IEnumerable<Claim>>(value => value != null && value.Any() == false)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenUserIdentityWasReturnedForExternalUserIdentifierAndExternalTokenClaimHasNotBeenBuild_AssertAddClaimsWasCalledOnUserIdentityForExternalUserIdentifierWithoutExternalTokenClaim()
		{
			Mock<IUserIdentity> userIdentityMock = _fixture.BuildUserIdentityMock();
			ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut(userIdentity: userIdentityMock.Object, hasExternalTokenClaim: false);

			IAuthenticateUserCommand authenticateUserCommand = CreateAuthenticateUserCommand(claims: BuildEmptyClaimCollection());
			await sut.ExecuteAsync(authenticateUserCommand);

			userIdentityMock.Verify(m => m.AddClaims(It.Is<IEnumerable<Claim>>(value => value != null && value.Any() == false)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenUserIdentityWasReturnedForExternalUserIdentifierAndExternalTokenClaimHasBeenBuild_AssertAddClaimsWasCalledOnUserIdentityForExternalUserIdentifierWithExternalTokenClaim()
		{
			Mock<IUserIdentity> userIdentityMock = _fixture.BuildUserIdentityMock();
			Claim externalTokenClaim = BuildExternalTokenClaim();
			ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut(userIdentity: userIdentityMock.Object, externalTokenClaim: externalTokenClaim);

			IAuthenticateUserCommand authenticateUserCommand = CreateAuthenticateUserCommand(claims: BuildClaimCollection());
			await sut.ExecuteAsync(authenticateUserCommand);

			userIdentityMock.Verify(m => m.AddClaims(It.Is<IEnumerable<Claim>>(value => value != null && value.Contains(externalTokenClaim))), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenUserIdentityWasReturnedForExternalUserIdentifier_AssertClearSensitiveDataWasCalledOnUserIdentityForExternalUserIdentifier()
		{
			Mock<IUserIdentity> userIdentityMock = _fixture.BuildUserIdentityMock();
			ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut(userIdentity: userIdentityMock.Object);

			IAuthenticateUserCommand authenticateUserCommand = CreateAuthenticateUserCommand();
			await sut.ExecuteAsync(authenticateUserCommand);

			userIdentityMock.Verify(m => m.ClearSensitiveData(), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenUserIdentityWasReturnedForExternalUserIdentifier_AssertToClaimsIdentityWasCalledOnUserIdentityForExternalUserIdentifier()
		{
			Mock<IUserIdentity> userIdentityMock = _fixture.BuildUserIdentityMock();
			ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut(userIdentity: userIdentityMock.Object);

			IAuthenticateUserCommand authenticateUserCommand = CreateAuthenticateUserCommand();
			await sut.ExecuteAsync(authenticateUserCommand);

			userIdentityMock.Verify(m => m.ToClaimsIdentity(), Times.Once);
		}

		[Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenNoUserIdentityWasReturnedForExternalUserIdentifier_ReturnsNull()
	    {
		    ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut(hasUserIdentityForExternalUserIdentifier: false);

		    IAuthenticateUserCommand authenticateUserCommand = CreateAuthenticateUserCommand();
		    ClaimsPrincipal result = await sut.ExecuteAsync(authenticateUserCommand);

		    Assert.That(result, Is.Null);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenUserIdentityWasReturnedForExternalUserIdentifier_ReturnsNotNull()
	    {
		    ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut();

		    IAuthenticateUserCommand authenticateUserCommand = CreateAuthenticateUserCommand();
		    ClaimsPrincipal result = await sut.ExecuteAsync(authenticateUserCommand);

		    Assert.That(result, Is.Not.Null);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenUserIdentityWasReturnedForExternalUserIdentifier_ReturnsClaimsPrincipalWhereIdentityIsNotNull()
	    {
		    ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut();

		    IAuthenticateUserCommand authenticateUserCommand = CreateAuthenticateUserCommand();
		    ClaimsPrincipal result = await sut.ExecuteAsync(authenticateUserCommand);

		    Assert.That(result.Identity, Is.Not.Null);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenUserIdentityWasReturnedForExternalUserIdentifier_ReturnsClaimsPrincipalWhereIdentityIsAuthenticated()
	    {
		    ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut();

		    IAuthenticateUserCommand authenticateUserCommand = CreateAuthenticateUserCommand();
		    ClaimsPrincipal result = await sut.ExecuteAsync(authenticateUserCommand);

		    Assert.That(result.Identity!.IsAuthenticated, Is.True);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenUserIdentityWasReturnedForExternalUserIdentifier_ReturnsClaimsPrincipalWhereIdentityIsAuthenticatedWithAuthenticationTypeFromAuthenticateUserCommand()
	    {
		    ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> sut = CreateSut();

		    string authenticationType = _fixture.Create<string>();
		    IAuthenticateUserCommand authenticateUserCommand = CreateAuthenticateUserCommand(authenticationType: authenticationType);
		    ClaimsPrincipal result = await sut.ExecuteAsync(authenticateUserCommand);

		    Assert.That(result.Identity!.AuthenticationType, Is.EqualTo(authenticationType));
	    }

		private ICommandHandler<IAuthenticateUserCommand, ClaimsPrincipal> CreateSut(bool hasUserIdentityForExternalUserIdentifier = true, IUserIdentity userIdentity = null, bool canBuildExternalTokenClaim = true, bool hasExternalTokenClaim = true, Claim externalTokenClaim = null)
        {
            _securityRepositoryMock.Setup(m => m.GetUserIdentityAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(hasUserIdentityForExternalUserIdentifier ? userIdentity ?? _fixture.BuildUserIdentityMock().Object : null));

            _externalTokenClaimCreatorMock.Setup(m => m.CanBuild(It.IsAny<IReadOnlyDictionary<string, string>>()))
	            .Returns(canBuildExternalTokenClaim);
            _externalTokenClaimCreatorMock.Setup(m => m.Build(It.IsAny<IReadOnlyDictionary<string, string>>(), It.IsAny<Func<string, string>>()))
	            .Returns(hasExternalTokenClaim ? externalTokenClaim ?? BuildExternalTokenClaim() : null);

			return new BusinessLogic.Security.CommandHandlers.AuthenticateUserCommandHandler(_securityRepositoryMock.Object, _externalTokenClaimCreatorMock.Object);
        }

        private IAuthenticateUserCommand CreateAuthenticateUserCommand(string externalUserIdentifier = null, IReadOnlyCollection<Claim> claims = null, string authenticationType = null, IReadOnlyDictionary<string, string> authenticationSessionItems = null, Func<string, string> protector = null)
        {
	        return CreateAuthenticateUserCommandMock(externalUserIdentifier, claims, authenticationType, authenticationSessionItems, protector).Object;
        }

        private Mock<IAuthenticateUserCommand> CreateAuthenticateUserCommandMock(string externalUserIdentifier = null, IReadOnlyCollection<Claim> claims = null, string authenticationType = null, IReadOnlyDictionary<string, string> authenticationSessionItems = null, Func<string, string> protector = null)
        {
	        Mock<IAuthenticateUserCommand> authenticateUserCommandMock = new Mock<IAuthenticateUserCommand>();
	        authenticateUserCommandMock.Setup(m => m.ExternalUserIdentifier)
		        .Returns(externalUserIdentifier ?? string.Empty);
	        authenticateUserCommandMock.Setup(m => m.Claims)
		        .Returns(claims ?? BuildClaimCollection());
	        authenticateUserCommandMock.Setup(m => m.AuthenticationType)
		        .Returns(authenticationType ?? _fixture.Create<string>());
	        authenticateUserCommandMock.Setup(m => m.AuthenticationSessionItems)
		        .Returns(authenticationSessionItems ?? new ConcurrentDictionary<string, string>());
	        authenticateUserCommandMock.Setup(m => m.Protector)
		        .Returns(protector ?? (value => value));
	        return authenticateUserCommandMock;
        }

        private Claim BuildExternalTokenClaim()
        {
	        return ClaimHelper.CreateClaim(ClaimHelper.MicrosoftTokenClaimType, Convert.ToBase64String(_fixture.CreateMany<byte>(_random.Next(256, 512)).ToArray()), typeof(IRefreshableToken).FullName);
        }

        private IReadOnlyCollection<Claim> BuildClaimCollection()
        {
	        return
            [
                ClaimHelper.CreateNameIdentifierClaim(_fixture.Create<string>()),
		        ClaimHelper.CreateNameClaim(_fixture.Create<string>()),
		        ClaimHelper.CreateEmailClaim($"{_fixture.Create<string>()}@{_fixture.Create<string>()}.{_fixture.Create<string>()}")
            ];
        }

        private IReadOnlyCollection<Claim> BuildEmptyClaimCollection()
        {
	        return Array.Empty<Claim>();
        }

        private static bool IsMatch(Claim claim, string claimType, string claimValue)
        {
            NullGuard.NotNull(claim, nameof(claim))
                .NotNullOrWhiteSpace(claimType, nameof(claimType))
                .NotNullOrWhiteSpace(claimValue, nameof(claimValue));

            return string.IsNullOrWhiteSpace(claim.Type) == false && string.CompareOrdinal(claim.Type, claimType) == 0 &&
                   string.IsNullOrWhiteSpace(claim.Value) == false && string.CompareOrdinal(claim.Value, claimValue) == 0;
        }

        private static string CalculateExpectedClaimValue(Claim claim)
        {
            NullGuard.NotNull(claim, nameof(claim));

            if (string.IsNullOrWhiteSpace(claim.Type) == false && string.CompareOrdinal(claim.Type, ClaimTypes.NameIdentifier) == 0)
            {
                return string.IsNullOrWhiteSpace(claim.Value) ? string.Empty : claim.Value.ComputeSha512Hash();
            }

            return string.IsNullOrWhiteSpace(claim.Value) ? string.Empty : claim.Value;
        }
    }
}