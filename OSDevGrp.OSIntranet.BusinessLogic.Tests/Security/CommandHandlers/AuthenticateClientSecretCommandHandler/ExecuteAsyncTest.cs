using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.CommandHandlers.AuthenticateClientSecretCommandHandler
{
	[TestFixture]
    public class ExecuteAsyncTest : BusinessLogicTestBase
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
	    public void ExecuteAsync_WhenAuthenticateClientSecretCommandIsNull_ThrowsArgumentNullException()
	    {
		    ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut();

		    ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

		    Assert.That(result, Is.Not.Null);
		    Assert.That(result.ParamName, Is.EqualTo("authenticateCommand"));
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenCalled_AssertClientIdWasCalledOnAuthenticateClientSecretCommand()
	    {
		    ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut();

		    Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock();
		    await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

		    authenticateClientSecretCommandMock.Verify(m => m.ClientId, Times.Once);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenCalled_AssertGetClientSecretIdentityAsyncWasCalledOnSecurityRepository()
	    {
		    ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut();

		    string clientId = _fixture.Create<string>();
		    IAuthenticateClientSecretCommand authenticateClientSecretCommand = CreateAuthenticateClientSecretCommand(clientId);
		    await sut.ExecuteAsync(authenticateClientSecretCommand);

		    _securityRepositoryMock.Verify(m => m.GetClientSecretIdentityAsync(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, clientId) == 0)), Times.Once);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenNoClientSecretIdentityWasReturnedForClientId_AssertClientSecretWasNotCalledOnAuthenticateClientSecretCommand()
	    {
		    ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(false);

		    Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock();
		    await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

		    authenticateClientSecretCommandMock.Verify(m => m.ClientSecret, Times.Never);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenNoClientSecretIdentityWasReturnedForClientId_AssertClaimsWasNotCalledOnAuthenticateClientSecretCommand()
	    {
		    ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(false);

		    Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock();
		    await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

		    authenticateClientSecretCommandMock.Verify(m => m.Claims, Times.Never);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenNoClientSecretIdentityWasReturnedForClientId_AssertAuthenticationSessionItemsWasNotCalledOnAuthenticateClientSecretCommand()
	    {
		    ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(false);

		    Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock();
		    await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

		    authenticateClientSecretCommandMock.Verify(m => m.AuthenticationSessionItems, Times.Never);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenNoClientSecretIdentityWasReturnedForClientId_AssertProtectorWasNotCalledOnAuthenticateClientSecretCommand()
	    {
		    ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(false);

		    Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock();
		    await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

		    authenticateClientSecretCommandMock.Verify(m => m.Protector, Times.Never);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenNoClientSecretIdentityWasReturnedForClientId_AssertCanBuildWasNotCalledOnExternalTokenClaimCreator()
	    {
		    ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(false);

		    IAuthenticateClientSecretCommand authenticateClientSecretCommand = CreateAuthenticateClientSecretCommand();
		    await sut.ExecuteAsync(authenticateClientSecretCommand);

		    _externalTokenClaimCreatorMock.Verify(m => m.CanBuild(It.IsAny<IDictionary<string, string>>()), Times.Never);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenNoClientSecretIdentityWasReturnedForClientId_AssertBuildWasNotCalledOnExternalTokenClaimCreator()
	    {
		    ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(false);

		    IAuthenticateClientSecretCommand authenticateClientSecretCommand = CreateAuthenticateClientSecretCommand();
		    await sut.ExecuteAsync(authenticateClientSecretCommand);

		    _externalTokenClaimCreatorMock.Verify(m => m.Build(
				    It.IsAny<IDictionary<string, string>>(),
				    It.IsAny<Func<string, string>>()),
			    Times.Never);
	    }

		[Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenNoClientSecretIdentityWasReturnedForClientId_AssertAuthenticationTypeWasNotCalledOnAuthenticateClientSecretCommand()
	    {
		    ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(false);

		    Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock();
		    await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

		    authenticateClientSecretCommandMock.Verify(m => m.AuthenticationType, Times.Never);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenClientSecretIdentityWasReturnedForClientId_AssertClientSecretWasCalledOnAuthenticateClientSecretCommand()
	    {
		    ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut();

		    Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock();
		    await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

		    authenticateClientSecretCommandMock.Verify(m => m.ClientSecret, Times.Once);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenClientSecretIdentityWasReturnedForClientId_AssertClientSecretWasCalledOnClientSecretIdentityForClientId()
	    {
		    Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock();
			ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentityMock.Object);

		    IAuthenticateClientSecretCommand authenticateClientSecretCommand = CreateAuthenticateClientSecretCommand();
		    await sut.ExecuteAsync(authenticateClientSecretCommand);

		    clientSecretIdentityMock.Verify(m => m.ClientSecret, Times.Once);
	    }

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNonMatchingClientSecretIdentityWasReturnedForClientId_AssertClaimsWasNotCalledOnAuthenticateClientSecretCommand()
		{
			string clientSecret = _fixture.Create<string>();
			IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret).Object;
			ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentity);

			string submittedClientSecret = _fixture.Create<string>();
			Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock(clientSecret: submittedClientSecret);
			await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

			authenticateClientSecretCommandMock.Verify(m => m.Claims, Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNonMatchingClientSecretIdentityWasReturnedForClientId_AssertAuthenticationSessionItemsWasNotCalledOnAuthenticateClientSecretCommand()
		{
			string clientSecret = _fixture.Create<string>();
			IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret).Object;
			ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentity);

			string submittedClientSecret = _fixture.Create<string>();
			Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock(clientSecret: submittedClientSecret);
			await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

			authenticateClientSecretCommandMock.Verify(m => m.AuthenticationSessionItems, Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNonMatchingClientSecretIdentityWasReturnedForClientId_AssertProtectorWasNotCalledOnAuthenticateClientSecretCommand()
		{
			string clientSecret = _fixture.Create<string>();
			IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret).Object;
			ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentity);

			string submittedClientSecret = _fixture.Create<string>();
			Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock(clientSecret: submittedClientSecret);
			await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

			authenticateClientSecretCommandMock.Verify(m => m.Protector, Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNonMatchingClientSecretIdentityWasReturnedForClientId_AssertCanBuildWasNotCalledOnExternalTokenClaimCreator()
		{
			string clientSecret = _fixture.Create<string>();
			IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret).Object;
			ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentity);

			string submittedClientSecret = _fixture.Create<string>();
			IAuthenticateClientSecretCommand authenticateClientSecretCommand = CreateAuthenticateClientSecretCommand(clientSecret: submittedClientSecret);
			await sut.ExecuteAsync(authenticateClientSecretCommand);

			_externalTokenClaimCreatorMock.Verify(m => m.CanBuild(It.IsAny<IDictionary<string, string>>()), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNonMatchingClientSecretIdentityWasReturnedForClientId_AssertBuildWasNotCalledOnExternalTokenClaimCreator()
		{
			string clientSecret = _fixture.Create<string>();
			IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret).Object;
			ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentity);

			string submittedClientSecret = _fixture.Create<string>();
			IAuthenticateClientSecretCommand authenticateClientSecretCommand = CreateAuthenticateClientSecretCommand(clientSecret: submittedClientSecret);
			await sut.ExecuteAsync(authenticateClientSecretCommand);

			_externalTokenClaimCreatorMock.Verify(m => m.Build(
					It.IsAny<IDictionary<string, string>>(),
					It.IsAny<Func<string, string>>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNonMatchingClientSecretIdentityWasReturnedForClientId_AssertAuthenticationTypeWasNotCalledOnAuthenticateClientSecretCommand()
		{
			string clientSecret = _fixture.Create<string>();
			IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret).Object;
			ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentity);

			string submittedClientSecret = _fixture.Create<string>();
			Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock(clientSecret: submittedClientSecret);
			await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

			authenticateClientSecretCommandMock.Verify(m => m.AuthenticationType, Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNonMatchingClientSecretIdentityWasReturnedForClientId_AssertAddClaimsWasNotCalledOnClientSecretIdentityForClientId()
		{
			string clientSecret = _fixture.Create<string>();
			Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret);
			ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentityMock.Object);

			string submittedClientSecret = _fixture.Create<string>();
			Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock(clientSecret: submittedClientSecret);
			await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

			clientSecretIdentityMock.Verify(m => m.AddClaims(It.IsAny<IEnumerable<Claim>>()), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNonMatchingClientSecretIdentityWasReturnedForClientId_AssertClearSensitiveDataWasNotCalledOnClientSecretIdentityForClientId()
		{
			string clientSecret = _fixture.Create<string>();
			Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret);
			ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentityMock.Object);

			string submittedClientSecret = _fixture.Create<string>();
			Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock(clientSecret: submittedClientSecret);
			await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

			clientSecretIdentityMock.Verify(m => m.ClearSensitiveData(), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNonMatchingClientSecretIdentityWasReturnedForClientId_AssertToClaimsIdentityWasNotCalledOnClientSecretIdentityForClientId()
		{
			string clientSecret = _fixture.Create<string>();
			Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret);
			ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentityMock.Object);

			string submittedClientSecret = _fixture.Create<string>();
			Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock(clientSecret: submittedClientSecret);
			await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

			clientSecretIdentityMock.Verify(m => m.ToClaimsIdentity(), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingClientSecretIdentityWasReturnedForClientId_AssertClaimsWasCalledOnAuthenticateClientSecretCommand()
		{
			string clientSecret = _fixture.Create<string>();
			IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret).Object;
			ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentity);

			Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock(clientSecret: clientSecret);
			await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

			authenticateClientSecretCommandMock.Verify(m => m.Claims, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingClientSecretIdentityWasReturnedForClientId_AssertAuthenticationSessionItemsWasCalledOnAuthenticateClientSecretCommand()
		{
			string clientSecret = _fixture.Create<string>();
			IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret).Object;
			ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentity);

			Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock(clientSecret: clientSecret);
			await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

			authenticateClientSecretCommandMock.Verify(m => m.AuthenticationSessionItems, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingClientSecretIdentityWasReturnedForClientId_AssertProtectorWasCalledOnAuthenticateClientSecretCommand()
		{
			string clientSecret = _fixture.Create<string>();
			IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret).Object;
			ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentity);

			Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock(clientSecret: clientSecret);
			await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

			authenticateClientSecretCommandMock.Verify(m => m.Protector, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingClientSecretIdentityWasReturnedForClientId_AssertCanBuildWasCalledOnExternalTokenClaimCreator()
		{
			string clientSecret = _fixture.Create<string>();
			IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret).Object;
			ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentity);

			IDictionary<string, string> authenticationSessionItems = new ConcurrentDictionary<string, string>();
			authenticationSessionItems.Add(_fixture.Create<string>(), _fixture.Create<string>());
			authenticationSessionItems.Add(_fixture.Create<string>(), _fixture.Create<string>());
			authenticationSessionItems.Add(_fixture.Create<string>(), _fixture.Create<string>());
			IAuthenticateClientSecretCommand authenticateClientSecretCommand = CreateAuthenticateClientSecretCommand(clientSecret: clientSecret, authenticationSessionItems: authenticationSessionItems.AsReadOnly());
			await sut.ExecuteAsync(authenticateClientSecretCommand);

			_externalTokenClaimCreatorMock.Verify(m => m.CanBuild(It.Is<IDictionary<string, string>>(value => value != null && authenticationSessionItems.All(n => value.ContainsKey(n.Key) && value[n.Key] == n.Value))), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingClientSecretIdentityWasReturnedForClientIdAndExternalTokenClaimCouldNotBeBuild_AssertBuildWasNotCalledOnExternalTokenClaimCreator()
		{
			string clientSecret = _fixture.Create<string>();
			IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret).Object;
			ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentity, canBuildExternalTokenClaim: false);

			IAuthenticateClientSecretCommand authenticateClientSecretCommand = CreateAuthenticateClientSecretCommand(clientSecret: clientSecret);
			await sut.ExecuteAsync(authenticateClientSecretCommand);

			_externalTokenClaimCreatorMock.Verify(m => m.Build(
					It.IsAny<IDictionary<string, string>>(),
					It.IsAny<Func<string, string>>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingClientSecretIdentityWasReturnedForClientIdAndExternalTokenClaimCouldBeBuild_AssertBuildWasCalledOnExternalTokenClaimCreator()
		{
			string clientSecret = _fixture.Create<string>();
			IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret).Object;
			ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentity);

			IDictionary<string, string> authenticationSessionItems = new ConcurrentDictionary<string, string>();
			authenticationSessionItems.Add(_fixture.Create<string>(), _fixture.Create<string>());
			authenticationSessionItems.Add(_fixture.Create<string>(), _fixture.Create<string>());
			authenticationSessionItems.Add(_fixture.Create<string>(), _fixture.Create<string>());
			Func<string, string> protector = value => value;
			IAuthenticateClientSecretCommand authenticateClientSecretCommand = CreateAuthenticateClientSecretCommand(clientSecret: clientSecret, authenticationSessionItems: authenticationSessionItems.AsReadOnly(), protector: protector);
			await sut.ExecuteAsync(authenticateClientSecretCommand);

			_externalTokenClaimCreatorMock.Verify(m => m.Build(
					It.Is<IDictionary<string, string>>(value => value != null && authenticationSessionItems.All(n => value.ContainsKey(n.Key) && value[n.Key] == n.Value)),
					It.Is<Func<string, string>>(value => value != null && value == protector)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingClientSecretIdentityWasReturnedForClientId_AssertAuthenticationTypeWasCalledOnAuthenticateClientSecretCommand()
		{
			string clientSecret = _fixture.Create<string>();
			IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret).Object;
			ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentity);

			Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock(clientSecret: clientSecret);
			await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

			authenticateClientSecretCommandMock.Verify(m => m.AuthenticationType, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task ExecuteAsync_WhenMatchingClientSecretIdentityWasReturnedForClientId_AssertAddClaimsWasCalledOnClientSecretIdentityForClientIdWithAllClaimsFromAuthenticateClientSecretCommand(bool canBuildExternalTokenClaim)
		{
			string clientSecret = _fixture.Create<string>();
			Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret);
			ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentityMock.Object, canBuildExternalTokenClaim: canBuildExternalTokenClaim);

			Claim[] claims =
			{
				new(_fixture.Create<string>(), _fixture.Create<string>()),
				new(_fixture.Create<string>(), _fixture.Create<string>()),
				new(_fixture.Create<string>(), _fixture.Create<string>())
			};
			Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock(clientSecret: clientSecret, claims: claims);
			await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

			clientSecretIdentityMock.Verify(m => m.AddClaims(It.Is<IEnumerable<Claim>>(value => value != null && claims.All(claim => value.Any(v => v.Type == claim.Type && v.Value == claim.Value)))), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingClientSecretIdentityWasReturnedForClientIdAndExternalTokenClaimCouldNotBeBuild_AssertAddClaimsWasCalledOnClientSecretIdentityForClientIdWithoutExternalTokenClaim()
		{
			string clientSecret = _fixture.Create<string>();
			Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret);
			ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentityMock.Object, canBuildExternalTokenClaim: false);

			Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock(clientSecret: clientSecret, claims: Array.Empty<Claim>());
			await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

			clientSecretIdentityMock.Verify(m => m.AddClaims(It.Is<IEnumerable<Claim>>(value => value != null && value.Any() == false)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingClientSecretIdentityWasReturnedForClientIdAndExternalTokenClaimHasNotBeenBuild_AssertAddClaimsWasCalledOnClientSecretIdentityForClientIdWithoutExternalTokenClaim()
		{
			string clientSecret = _fixture.Create<string>();
			Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret);
			ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentityMock.Object, hasExternalTokenClaim: false);

			Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock(clientSecret: clientSecret, claims: Array.Empty<Claim>());
			await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

			clientSecretIdentityMock.Verify(m => m.AddClaims(It.Is<IEnumerable<Claim>>(value => value != null && value.Any() == false)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingClientSecretIdentityWasReturnedForClientIdAndExternalTokenClaimHasBeenBuild_AssertAddClaimsWasCalledOnClientSecretIdentityForClientIdWithExternalTokenClaim()
		{
			string clientSecret = _fixture.Create<string>();
			Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret);
			Claim externalTokenClaim = new Claim(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>());
			ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentityMock.Object, externalTokenClaim: externalTokenClaim);

			Claim[] claims =
			{
				new(_fixture.Create<string>(), _fixture.Create<string>()),
				new(_fixture.Create<string>(), _fixture.Create<string>()),
				new(_fixture.Create<string>(), _fixture.Create<string>())
			};
			Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock(clientSecret: clientSecret, claims: claims);
			await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

			clientSecretIdentityMock.Verify(m => m.AddClaims(It.Is<IEnumerable<Claim>>(value => value != null && value.Contains(externalTokenClaim))), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingClientSecretIdentityWasReturnedForClientId_AssertClearSensitiveDataWasCalledOnClientSecretIdentityForClientId()
		{
			string clientSecret = _fixture.Create<string>();
			Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret);
			ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentityMock.Object);

			Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock(clientSecret: clientSecret);
			await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

			clientSecretIdentityMock.Verify(m => m.ClearSensitiveData(), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenMatchingClientSecretIdentityWasReturnedForClientId_AssertToClaimsIdentityWasCalledOnClientSecretIdentityForClientId()
		{
			string clientSecret = _fixture.Create<string>();
			Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret);
			ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentityMock.Object);

			Mock<IAuthenticateClientSecretCommand> authenticateClientSecretCommandMock = CreateAuthenticateClientSecretCommandMock(clientSecret: clientSecret);
			await sut.ExecuteAsync(authenticateClientSecretCommandMock.Object);

			clientSecretIdentityMock.Verify(m => m.ToClaimsIdentity(), Times.Once);
		}

		[Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenNoClientSecretIdentityWasReturnedForClientId_ReturnsNull()
	    {
		    ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(hasClientSecretIdentityForClientId: false);

		    IAuthenticateClientSecretCommand authenticateClientSecretCommand = CreateAuthenticateClientSecretCommand();
		    ClaimsPrincipal result = await sut.ExecuteAsync(authenticateClientSecretCommand);

		    Assert.That(result, Is.Null);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenNonMatchingClientSecretIdentityWasReturnedForClientId_ReturnsNull()
	    {
		    string clientSecret = _fixture.Create<string>();
		    IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret).Object;
		    ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentity);

		    string submittedClientSecret = _fixture.Create<string>();
		    IAuthenticateClientSecretCommand authenticateClientSecretCommand = CreateAuthenticateClientSecretCommand(clientSecret: submittedClientSecret);
		    ClaimsPrincipal result = await sut.ExecuteAsync(authenticateClientSecretCommand);

		    Assert.That(result, Is.Null);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenMatchingClientSecretIdentityWasReturnedForClientId_ReturnsNotNull()
	    {
		    string clientSecret = _fixture.Create<string>();
		    IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret).Object;
		    ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentity);

		    IAuthenticateClientSecretCommand authenticateClientSecretCommand = CreateAuthenticateClientSecretCommand(clientSecret: clientSecret);
		    ClaimsPrincipal result = await sut.ExecuteAsync(authenticateClientSecretCommand);

		    Assert.That(result, Is.Not.Null);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenMatchingClientSecretIdentityWasReturnedForClientId_ReturnsClaimsPrincipalWhereIdentityIsNotNull()
	    {
		    string clientSecret = _fixture.Create<string>();
		    IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret).Object;
		    ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentity);

		    IAuthenticateClientSecretCommand authenticateClientSecretCommand = CreateAuthenticateClientSecretCommand(clientSecret: clientSecret);
		    ClaimsPrincipal result = await sut.ExecuteAsync(authenticateClientSecretCommand);

		    Assert.That(result.Identity, Is.Not.Null);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenMatchingClientSecretIdentityWasReturnedForClientId_ReturnsClaimsPrincipalWhereIdentityIsAuthenticated()
	    {
		    string clientSecret = _fixture.Create<string>();
		    IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret).Object;
		    ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentity);

		    IAuthenticateClientSecretCommand authenticateClientSecretCommand = CreateAuthenticateClientSecretCommand(clientSecret: clientSecret);
		    ClaimsPrincipal result = await sut.ExecuteAsync(authenticateClientSecretCommand);

		    Assert.That(result.Identity!.IsAuthenticated, Is.True);
	    }

	    [Test]
	    [Category("UnitTest")]
	    public async Task ExecuteAsync_WhenMatchingClientSecretIdentityWasReturnedForClientId_ReturnsClaimsPrincipalWhereIdentityIsAuthenticatedWithAuthenticationTypeFromAuthenticateClientSecretCommand()
	    {
		    string clientSecret = _fixture.Create<string>();
		    IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock(clientSecret: clientSecret).Object;
		    ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> sut = CreateSut(clientSecretIdentity: clientSecretIdentity);

		    string authenticationType = _fixture.Create<string>();
		    IAuthenticateClientSecretCommand authenticateClientSecretCommand = CreateAuthenticateClientSecretCommand(clientSecret: clientSecret, authenticationType: authenticationType);
		    ClaimsPrincipal result = await sut.ExecuteAsync(authenticateClientSecretCommand);

		    Assert.That(result.Identity!.AuthenticationType, Is.EqualTo(authenticationType));
	    }

		private ICommandHandler<IAuthenticateClientSecretCommand, ClaimsPrincipal> CreateSut(bool hasClientSecretIdentityForClientId = true, IClientSecretIdentity clientSecretIdentity = null, bool canBuildExternalTokenClaim = true, bool hasExternalTokenClaim = true, Claim externalTokenClaim = null)
        {
            _securityRepositoryMock.Setup(m => m.GetClientSecretIdentityAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(hasClientSecretIdentityForClientId ? clientSecretIdentity ?? _fixture.BuildClientSecretIdentityMock().Object : null));

            _externalTokenClaimCreatorMock.Setup(m => m.CanBuild(It.IsAny<IDictionary<string, string>>()))
	            .Returns(canBuildExternalTokenClaim);
            _externalTokenClaimCreatorMock.Setup(m => m.Build(It.IsAny<IDictionary<string, string>>(), It.IsAny<Func<string, string>>()))
	            .Returns(hasExternalTokenClaim ? externalTokenClaim ?? new Claim(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>()) : null);

            return new BusinessLogic.Security.CommandHandlers.AuthenticateClientSecretCommandHandler(_securityRepositoryMock.Object, _externalTokenClaimCreatorMock.Object);
        }

        private IAuthenticateClientSecretCommand CreateAuthenticateClientSecretCommand(string clientId = null, string clientSecret = null, IReadOnlyCollection<Claim> claims = null, string authenticationType = null, IReadOnlyDictionary<string, string> authenticationSessionItems = null, Func<string, string> protector = null)
        {
	        return CreateAuthenticateClientSecretCommandMock(clientId, clientSecret, claims, authenticationType, authenticationSessionItems, protector).Object;
        }

        private Mock<IAuthenticateClientSecretCommand> CreateAuthenticateClientSecretCommandMock(string clientId = null, string clientSecret = null, IReadOnlyCollection<Claim> claims = null, string authenticationType = null, IReadOnlyDictionary<string, string> authenticationSessionItems = null, Func<string, string> protector = null)
        {
	        Mock<IAuthenticateClientSecretCommand> authenticateUserCommandMock = new Mock<IAuthenticateClientSecretCommand>();
	        authenticateUserCommandMock.Setup(m => m.ClientId)
		        .Returns(clientId ?? string.Empty);
	        authenticateUserCommandMock.Setup(m => m.ClientSecret)
		        .Returns(clientSecret ?? string.Empty);
	        authenticateUserCommandMock.Setup(m => m.Claims)
		        .Returns(claims ?? Array.Empty<Claim>());
	        authenticateUserCommandMock.Setup(m => m.AuthenticationType)
		        .Returns(authenticationType ?? _fixture.Create<string>());
	        authenticateUserCommandMock.Setup(m => m.AuthenticationSessionItems)
		        .Returns(authenticationSessionItems ?? new ConcurrentDictionary<string, string>().AsReadOnly());
	        authenticateUserCommandMock.Setup(m => m.Protector)
		        .Returns(protector ?? (value => value));
	        return authenticateUserCommandMock;
        }
    }
}