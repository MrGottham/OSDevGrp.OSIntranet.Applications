using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.CommandHandlers.AuthenticateCommandHandlerBase
{
	[TestFixture]
	public class ExecuteAsyncTests : BusinessLogicTestBase
	{
		#region Private variables

		private Mock<ISecurityRepository> _securityRepositoryMock;
		private Mock<IExternalTokenClaimCreator> _externalTokenClaimCreatorMock;
		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_securityRepositoryMock = new Mock<ISecurityRepository>();
			_externalTokenClaimCreatorMock = new Mock<IExternalTokenClaimCreator>();
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void ExecuteAsync_WhenAuthenticateCommandIsNull_ThrowsArgumentNullException()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("authenticateCommand"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertGetIdentityAsyncWasCalledOnAuthenticateCommandHandlerBase()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut();

			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand();
			await sut.ExecuteAsync(authenticateCommand);

			Assert.That(((MyAuthenticateCommandHandler) sut).GetIdentityAsyncWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertGetIdentityAsyncWasCalledOnAuthenticateCommandHandlerBaseWithAuthenticateCommand()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut();

			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand();
			await sut.ExecuteAsync(authenticateCommand);

			Assert.That(((MyAuthenticateCommandHandler) sut).GetIdentityAsyncWasCalledWithAuthenticateCommand, Is.EqualTo(authenticateCommand));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNoIdentityWasReturnedFromGetIdentityAsync_AssertIsMatchWasNotCalledOnAuthenticateCommandHandlerBase()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(hasIdentity: false);

			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand();
			await sut.ExecuteAsync(authenticateCommand);

			Assert.That(((MyAuthenticateCommandHandler) sut).IsMatchWasCalled, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNoIdentityWasReturnedFromGetIdentityAsync_AssertClaimsWasNotCalledOnAuthenticateCommand()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(hasIdentity: false);

			Mock<IAuthenticateCommand> authenticateCommandMock = CreateAuthenticateCommandMock();
			await sut.ExecuteAsync(authenticateCommandMock.Object);

			authenticateCommandMock.Verify(m => m.Claims, Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNoIdentityWasReturnedFromGetIdentityAsync_AssertAuthenticationSessionItemsWasNotCalledOnAuthenticateCommand()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(hasIdentity: false);

			Mock<IAuthenticateCommand> authenticateCommandMock = CreateAuthenticateCommandMock();
			await sut.ExecuteAsync(authenticateCommandMock.Object);

			authenticateCommandMock.Verify(m => m.AuthenticationSessionItems, Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNoIdentityWasReturnedFromGetIdentityAsync_AssertProtectorWasNotCalledOnAuthenticateCommand()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(hasIdentity: false);

			Mock<IAuthenticateCommand> authenticateCommandMock = CreateAuthenticateCommandMock();
			await sut.ExecuteAsync(authenticateCommandMock.Object);

			authenticateCommandMock.Verify(m => m.Protector, Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNoIdentityWasReturnedFromGetIdentityAsync_AssertCanBuildWasNotCalledOnExternalTokenClaimCreator()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(hasIdentity: false);

			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand();
			await sut.ExecuteAsync(authenticateCommand);

			_externalTokenClaimCreatorMock.Verify(m => m.CanBuild(It.IsAny<IDictionary<string, string>>()), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNoIdentityWasReturnedFromGetIdentityAsync_AssertBuildWasNotCalledOnExternalTokenClaimCreator()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(hasIdentity: false);

			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand();
			await sut.ExecuteAsync(authenticateCommand);

			_externalTokenClaimCreatorMock.Verify(m => m.Build(
					It.IsAny<IDictionary<string, string>>(),
					It.IsAny<Func<string, string>>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNoIdentityWasReturnedFromGetIdentityAsync_AssertAuthenticationTypeWasNotCalledOnAuthenticateCommand()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(hasIdentity: false);

			Mock<IAuthenticateCommand> authenticateCommandMock = CreateAuthenticateCommandMock();
			await sut.ExecuteAsync(authenticateCommandMock.Object);

			authenticateCommandMock.Verify(m => m.AuthenticationType, Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNoIdentityWasReturnedFromGetIdentityAsync_AssertCreateAuthenticatedClaimsIdentityWasNotCalledOnAuthenticateCommandHandlerBase()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(hasIdentity: false);

			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand();
			await sut.ExecuteAsync(authenticateCommand);

			Assert.That(((MyAuthenticateCommandHandler) sut).CreateAuthenticatedClaimsIdentityWasCalled, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenIdentityWasReturnedFromGetIdentityAsync_AssertIsMatchWasCalledOnAuthenticateCommandHandlerBase()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut();

			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand();
			await sut.ExecuteAsync(authenticateCommand);

			Assert.That(((MyAuthenticateCommandHandler) sut).IsMatchWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenIdentityWasReturnedFromGetIdentityAsync_AssertIsMatchWasCalledOnAuthenticateCommandHandlerBaseWithAuthenticateCommand()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut();

			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand();
			await sut.ExecuteAsync(authenticateCommand);

			Assert.That(((MyAuthenticateCommandHandler) sut).IsMatchWasCalledWithAuthenticateCommand, Is.EqualTo(authenticateCommand));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenIdentityWasReturnedFromGetIdentityAsync_AssertIsMatchWasCalledOnAuthenticateCommandHandlerBaseWithIdentityFromGetIdentityAsync()
		{
			IIdentity identity = new ClaimsIdentity();
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(identity: identity);

			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand();
			await sut.ExecuteAsync(authenticateCommand);

			Assert.That(((MyAuthenticateCommandHandler) sut).IsMatchWasCalledWithIdentity, Is.EqualTo(identity));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNonMatchingIdentityWasReturnedFromGetIdentityAsync_AssertClaimsWasNotCalledOnAuthenticateCommand()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(isMatch: false);

			Mock<IAuthenticateCommand> authenticateCommandMock = CreateAuthenticateCommandMock();
			await sut.ExecuteAsync(authenticateCommandMock.Object);

			authenticateCommandMock.Verify(m => m.Claims, Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNonMatchingIdentityWasReturnedFromGetIdentityAsync_AssertAuthenticationSessionItemsWasNotCalledOnAuthenticateCommand()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(isMatch: false);

			Mock<IAuthenticateCommand> authenticateCommandMock = CreateAuthenticateCommandMock();
			await sut.ExecuteAsync(authenticateCommandMock.Object);

			authenticateCommandMock.Verify(m => m.AuthenticationSessionItems, Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNonMatchingIdentityWasReturnedFromGetIdentityAsync_AssertProtectorWasNotCalledOnAuthenticateCommand()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(isMatch: false);

			Mock<IAuthenticateCommand> authenticateCommandMock = CreateAuthenticateCommandMock();
			await sut.ExecuteAsync(authenticateCommandMock.Object);

			authenticateCommandMock.Verify(m => m.Protector, Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNonMatchingIdentityWasReturnedFromGetIdentityAsync_AssertCanBuildWasNotCalledOnExternalTokenClaimCreator()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(isMatch: false);

			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand();
			await sut.ExecuteAsync(authenticateCommand);

			_externalTokenClaimCreatorMock.Verify(m => m.CanBuild(It.IsAny<IDictionary<string, string>>()), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNonMatchingIdentityWasReturnedFromGetIdentityAsync_AssertBuildWasNotCalledOnExternalTokenClaimCreator()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(isMatch: false);

			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand();
			await sut.ExecuteAsync(authenticateCommand);

			_externalTokenClaimCreatorMock.Verify(m => m.Build(
					It.IsAny<IDictionary<string, string>>(),
					It.IsAny<Func<string, string>>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNonMatchingIdentityWasReturnedFromGetIdentityAsync_AssertAuthenticationTypeWasNotCalledOnAuthenticateCommand()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(isMatch: false);

			Mock<IAuthenticateCommand> authenticateCommandMock = CreateAuthenticateCommandMock();
			await sut.ExecuteAsync(authenticateCommandMock.Object);

			authenticateCommandMock.Verify(m => m.AuthenticationType, Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNonMatchingIdentityWasReturnedFromGetIdentityAsync_AssertCreateAuthenticatedClaimsIdentityWasNotCalledOnAuthenticateCommandHandlerBase()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(isMatch: false);

			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand();
			await sut.ExecuteAsync(authenticateCommand);

			Assert.That(((MyAuthenticateCommandHandler) sut).CreateAuthenticatedClaimsIdentityWasCalled, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingIdentityWasReturnedFromGetIdentityAsync_AssertClaimsWasCalledOnAuthenticateCommand()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut();

			Mock<IAuthenticateCommand> authenticateCommandMock = CreateAuthenticateCommandMock();
			await sut.ExecuteAsync(authenticateCommandMock.Object);

			authenticateCommandMock.Verify(m => m.Claims, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingIdentityWasReturnedFromGetIdentityAsync_AssertAuthenticationSessionItemsWasCalledOnAuthenticateCommand()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut();

			Mock<IAuthenticateCommand> authenticateCommandMock = CreateAuthenticateCommandMock();
			await sut.ExecuteAsync(authenticateCommandMock.Object);

			authenticateCommandMock.Verify(m => m.AuthenticationSessionItems, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingIdentityWasReturnedFromGetIdentityAsync_AssertProtectorWasCalledOnAuthenticateCommand()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut();

			Mock<IAuthenticateCommand> authenticateCommandMock = CreateAuthenticateCommandMock();
			await sut.ExecuteAsync(authenticateCommandMock.Object);

			authenticateCommandMock.Verify(m => m.Protector, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingIdentityWasReturnedFromGetIdentityAsync_AssertCanBuildWasCalledOnExternalTokenClaimCreator()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = new ConcurrentDictionary<string, string>();
			authenticationSessionItems.Add(_fixture.Create<string>(), _fixture.Create<string>());
			authenticationSessionItems.Add(_fixture.Create<string>(), _fixture.Create<string>());
			authenticationSessionItems.Add(_fixture.Create<string>(), _fixture.Create<string>());
			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand(authenticationSessionItems: authenticationSessionItems.AsReadOnly());
			await sut.ExecuteAsync(authenticateCommand);

			_externalTokenClaimCreatorMock.Verify(m => m.CanBuild(It.Is<IDictionary<string, string>>(value => value != null && authenticationSessionItems.All(n => value.ContainsKey(n.Key) && value[n.Key] == n.Value))), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingIdentityWasReturnedFromGetIdentityAsyncAndExternalTokenClaimCouldNotBeBuild_AssertBuildWasNotCalledOnExternalTokenClaimCreator()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(canBuildExternalTokenClaim: false);

			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand();
			await sut.ExecuteAsync(authenticateCommand);

			_externalTokenClaimCreatorMock.Verify(m => m.Build(
					It.IsAny<IDictionary<string, string>>(),
					It.IsAny<Func<string, string>>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingIdentityWasReturnedFromGetIdentityAsyncAndExternalTokenClaimCouldBeBuild_AssertBuildWasCalledOnExternalTokenClaimCreator()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = new ConcurrentDictionary<string, string>();
			authenticationSessionItems.Add(_fixture.Create<string>(), _fixture.Create<string>());
			authenticationSessionItems.Add(_fixture.Create<string>(), _fixture.Create<string>());
			authenticationSessionItems.Add(_fixture.Create<string>(), _fixture.Create<string>());
			Func<string, string> protector = value => value;
			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand(authenticationSessionItems: authenticationSessionItems.AsReadOnly(), protector: protector);
			await sut.ExecuteAsync(authenticateCommand);

			_externalTokenClaimCreatorMock.Verify(m => m.Build(
					It.Is<IDictionary<string, string>>(value => value != null && authenticationSessionItems.All(n => value.ContainsKey(n.Key) && value[n.Key] == n.Value)),
					It.Is<Func<string, string>>(value => value != null && value == protector)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingIdentityWasReturnedFromGetIdentityAsync_AssertAuthenticationTypeWasCalledOnAuthenticateCommand()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut();

			Mock<IAuthenticateCommand> authenticateCommandMock = CreateAuthenticateCommandMock();
			await sut.ExecuteAsync(authenticateCommandMock.Object);

			authenticateCommandMock.Verify(m => m.AuthenticationType, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingIdentityWasReturnedFromGetIdentityAsync_AssertCreateAuthenticatedClaimsIdentityWasCalledOnAuthenticateCommandHandlerBase()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut();

			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand();
			await sut.ExecuteAsync(authenticateCommand);

			Assert.That(((MyAuthenticateCommandHandler) sut).CreateAuthenticatedClaimsIdentityWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingIdentityWasReturnedFromGetIdentityAsync_AssertCreateAuthenticatedClaimsIdentityWasCalledOnAuthenticateCommandHandlerBaseWithIdentityFromGetIdentityAsync() 
		{
			IIdentity identity = new ClaimsIdentity();
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(identity: identity);

			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand();
			await sut.ExecuteAsync(authenticateCommand);

			Assert.That(((MyAuthenticateCommandHandler) sut).CreateAuthenticatedClaimsIdentityWasCalledWithIdentity, Is.EqualTo(identity));
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task ExecuteAsync_WhenMatchingIdentityWasReturnedFromGetIdentityAsync_AssertCreateAuthenticatedClaimsIdentityWasCalledOnAuthenticateCommandHandlerBaseWithAllClaimsFromAuthenticateCommand(bool canBuildExternalTokenClaim)
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(canBuildExternalTokenClaim: canBuildExternalTokenClaim);

			IReadOnlyCollection<Claim> claims = new[]
			{
				new Claim(_fixture.Create<string>(), _fixture.Create<string>()),
				new Claim(_fixture.Create<string>(), _fixture.Create<string>()),
				new Claim(_fixture.Create<string>(), _fixture.Create<string>())
			};
			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand(claims: claims);
			await sut.ExecuteAsync(authenticateCommand);

			foreach (Claim claim in claims)
			{
				Assert.That(((MyAuthenticateCommandHandler) sut).CreateAuthenticatedClaimsIdentityWasCalledWithClaims.SingleOrDefault(m => m != null && string.CompareOrdinal(claim.Type, m.Type) == 0 && string.CompareOrdinal(claim.Value, m.Value) == 0), Is.Not.Null);
			}
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingIdentityWasReturnedFromGetIdentityAsyncAndExternalTokenClaimCouldNotBeBuild_AssertCreateAuthenticatedClaimsIdentityWasCalledOnAuthenticateCommandHandlerBaseWithoutExternalTokenClaim()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(canBuildExternalTokenClaim: false);

			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand(claims: Array.Empty<Claim>());
			await sut.ExecuteAsync(authenticateCommand);

			Assert.That(((MyAuthenticateCommandHandler) sut).CreateAuthenticatedClaimsIdentityWasCalledWithClaims, Is.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingIdentityWasReturnedFromGetIdentityAsyncAndExternalTokenClaimHasNotBeenBuild_AssertCreateAuthenticatedClaimsIdentityWasCalledOnAuthenticateCommandHandlerBaseWithoutExternalTokenClaim()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(hasExternalTokenClaim: false);

			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand(claims: Array.Empty<Claim>());
			await sut.ExecuteAsync(authenticateCommand);

			Assert.That(((MyAuthenticateCommandHandler)sut).CreateAuthenticatedClaimsIdentityWasCalledWithClaims, Is.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingIdentityWasReturnedFromGetIdentityAsyncAndExternalTokenClaimHasBeenBuild_AssertCreateAuthenticatedClaimsIdentityWasCalledOnAuthenticateCommandHandlerBaseWithExternalTokenClaim()
		{
			Claim externalTokenClaim = new Claim(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>());
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(externalTokenClaim: externalTokenClaim);

			IReadOnlyCollection<Claim> claims = new[]
			{
				new Claim(_fixture.Create<string>(), _fixture.Create<string>()),
				new Claim(_fixture.Create<string>(), _fixture.Create<string>()),
				new Claim(_fixture.Create<string>(), _fixture.Create<string>())
			};
			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand(claims: claims);
			await sut.ExecuteAsync(authenticateCommand);

			Assert.That(((MyAuthenticateCommandHandler) sut).CreateAuthenticatedClaimsIdentityWasCalledWithClaims.Contains(externalTokenClaim), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingIdentityWasReturnedFromGetIdentityAsync_AssertCreateAuthenticatedClaimsIdentityWasCalledOnAuthenticateCommandHandlerBaseWithAuthenticationTypeFromAuthenticateCommand()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut();

			string authenticationType = _fixture.Create<string>();
			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand(authenticationType: authenticationType);
			await sut.ExecuteAsync(authenticateCommand);

			Assert.That(((MyAuthenticateCommandHandler) sut).CreateAuthenticatedClaimsIdentityWasCalledWithAuthenticationType, Is.EqualTo(authenticationType));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNoIdentityWasReturnedFromGetIdentityAsync_ReturnsNull()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(hasIdentity: false);

			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand();
			ClaimsPrincipal result = await sut.ExecuteAsync(authenticateCommand);

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNonMatchingIdentityWasReturnedFromGetIdentityAsync_ReturnsNull()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(isMatch: false);

			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand();
			ClaimsPrincipal result = await sut.ExecuteAsync(authenticateCommand);

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingIdentityWasReturnedFromGetIdentityAsync_ReturnsNotNull()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut();

			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand();
			ClaimsPrincipal result = await sut.ExecuteAsync(authenticateCommand);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingIdentityWasReturnedFromGetIdentityAsync_ReturnsClaimsPrincipalWhereIdentityIsNotNull()
		{
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut();

			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand();
			ClaimsPrincipal result = await sut.ExecuteAsync(authenticateCommand);

			Assert.That(result.Identity, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingIdentityWasReturnedFromGetIdentityAsync_ReturnsClaimsPrincipalWhereIdentityIsEqualToClaimsIdentityFromCreateAuthenticatedClaimsIdentityOnAuthenticateCommandHandlerBase()
		{
			ClaimsIdentity claimsIdentity = new ClaimsIdentity();
			ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> sut = CreateSut(claimsIdentity: claimsIdentity);

			IAuthenticateCommand authenticateCommand = CreateAuthenticateCommand();
			ClaimsPrincipal result = await sut.ExecuteAsync(authenticateCommand);

			Assert.That(result.Identity, Is.EqualTo(claimsIdentity));
		}

		private ICommandHandler<IAuthenticateCommand, ClaimsPrincipal> CreateSut(bool hasIdentity = true, IIdentity identity = null, ClaimsIdentity claimsIdentity = null, bool isMatch = true, bool canBuildExternalTokenClaim = true, bool hasExternalTokenClaim = true, Claim externalTokenClaim = null)
		{
			_externalTokenClaimCreatorMock.Setup(m => m.CanBuild(It.IsAny<IDictionary<string, string>>()))
				.Returns(canBuildExternalTokenClaim);
			_externalTokenClaimCreatorMock.Setup(m => m.Build(It.IsAny<IDictionary<string, string>>(), It.IsAny<Func<string, string>>()))
				.Returns(hasExternalTokenClaim ? externalTokenClaim ?? new Claim(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>()) : null);

			return new MyAuthenticateCommandHandler(hasIdentity, identity ?? new ClaimsIdentity(), claimsIdentity ?? new ClaimsIdentity(), isMatch, _securityRepositoryMock.Object, _externalTokenClaimCreatorMock.Object);
		}

		private IAuthenticateCommand CreateAuthenticateCommand(IReadOnlyCollection<Claim> claims = null, string authenticationType = null, IReadOnlyDictionary<string, string> authenticationSessionItems = null, Func<string, string> protector = null)
		{
			return CreateAuthenticateCommandMock(claims, authenticationType, authenticationSessionItems, protector).Object;
		}

		private Mock<IAuthenticateCommand> CreateAuthenticateCommandMock(IReadOnlyCollection<Claim> claims = null, string authenticationType = null, IReadOnlyDictionary<string, string> authenticationSessionItems = null, Func<string, string> protector = null)
		{
			Mock<IAuthenticateCommand> authenticateCommandMock = new Mock<IAuthenticateCommand>();
			authenticateCommandMock.Setup(m => m.Claims)
				.Returns(claims ?? Array.Empty<Claim>());
			authenticateCommandMock.Setup(m => m.AuthenticationType)
				.Returns(authenticationType ?? _fixture.Create<string>());
			authenticateCommandMock.Setup(m => m.AuthenticationSessionItems)
				.Returns(authenticationSessionItems ?? new ConcurrentDictionary<string, string>().AsReadOnly());
			authenticateCommandMock.Setup(m => m.Protector)
				.Returns(protector ?? (value => value));
			return authenticateCommandMock;
		}

		private sealed class MyAuthenticateCommandHandler : BusinessLogic.Security.CommandHandlers.AuthenticateCommandHandlerBase<IAuthenticateCommand, IIdentity>
		{
			#region Private variables

			private readonly bool _hasIdentity;
			private readonly IIdentity _identity;
			private readonly ClaimsIdentity _claimsIdentity;
			private readonly bool _isMatch;

			#endregion

			#region Constructor

			public MyAuthenticateCommandHandler(bool hasIdentity, IIdentity identity, ClaimsIdentity claimsIdentity, bool isMatch, ISecurityRepository securityRepository, IExternalTokenClaimCreator externalTokenClaimCreator) 
				: base(securityRepository, externalTokenClaimCreator)
			{
				NullGuard.NotNull(identity, nameof(identity))
					.NotNull(claimsIdentity, nameof(claimsIdentity));

				_hasIdentity = hasIdentity;
				_identity = identity;
				_claimsIdentity = claimsIdentity;
				_isMatch = isMatch;
			}

			#endregion

			#region Properties

			public bool GetIdentityAsyncWasCalled { get; private set; }

			public IAuthenticateCommand GetIdentityAsyncWasCalledWithAuthenticateCommand { get; private set; }

			public bool CreateAuthenticatedClaimsIdentityWasCalled { get; private set; }

			public IIdentity CreateAuthenticatedClaimsIdentityWasCalledWithIdentity { get; private set; }

			public IReadOnlyCollection<Claim> CreateAuthenticatedClaimsIdentityWasCalledWithClaims { get; private set; }

			public string CreateAuthenticatedClaimsIdentityWasCalledWithAuthenticationType { get; private set; }

			public bool IsMatchWasCalled { get; private set; }

			public IAuthenticateCommand IsMatchWasCalledWithAuthenticateCommand { get; private set; }

			public IIdentity IsMatchWasCalledWithIdentity { get; private set; }

			#endregion

			#region Methods

			protected override Task<IIdentity> GetIdentityAsync(IAuthenticateCommand authenticateCommand)
			{
				NullGuard.NotNull(authenticateCommand, nameof(authenticateCommand));

				GetIdentityAsyncWasCalled = true;
				GetIdentityAsyncWasCalledWithAuthenticateCommand = authenticateCommand;

				return Task.FromResult(_hasIdentity ? _identity : null);
			}

			protected override ClaimsIdentity CreateAuthenticatedClaimsIdentity(IIdentity identity, IReadOnlyCollection<Claim> claims, string authenticationType)
			{
				NullGuard.NotNull(identity, nameof(identity))
					.NotNull(claims, nameof(claims))
					.NotNullOrWhiteSpace(authenticationType, nameof(authenticationType));

				CreateAuthenticatedClaimsIdentityWasCalled = true;
				CreateAuthenticatedClaimsIdentityWasCalledWithIdentity = identity;
				CreateAuthenticatedClaimsIdentityWasCalledWithClaims = claims;
				CreateAuthenticatedClaimsIdentityWasCalledWithAuthenticationType = authenticationType;

				return _claimsIdentity;
			}

			protected override bool IsMatch(IAuthenticateCommand authenticateCommand, IIdentity identity)
			{
				NullGuard.NotNull(authenticateCommand, nameof(authenticateCommand))
					.NotNull(identity, nameof(identity));

				IsMatchWasCalled = true;
				IsMatchWasCalledWithAuthenticateCommand = authenticateCommand;
				IsMatchWasCalledWithIdentity = identity;

				return _isMatch;
			}

			#endregion
		}
	}
}