using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Helpers;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers.AuthenticateClientSecretCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.CommandHandlers.AuthenticateClientSecretCommandHandler
{
    [TestFixture]
    public class ExecuteAsyncTest : BusinessLogicTestBase
    {
        #region Private variables

        private Mock<ISecurityRepository> _securityRepositoryMock;
        private Mock<ITokenHelper> _tokenHelperMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _securityRepositoryMock = new Mock<ISecurityRepository>();
            _tokenHelperMock = new Mock<ITokenHelper>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenCommandIsNull_ThrowsArgumentNullException()
        {
            CommandHandler sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("command"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertGetClientSecretIdentityAsyncWasCalledOnSecurityRepository()
        {
            CommandHandler sut = CreateSut();

            string clientId = _fixture.Create<string>();
            IAuthenticateClientSecretCommand command = CreateCommand(clientId);
            await sut.ExecuteAsync(command);

            _securityRepositoryMock.Verify(m => m.GetClientSecretIdentityAsync(It.Is<string>(value => string.Compare(value, clientId, StringComparison.Ordinal) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientIdIsUnknown_AssertClientSecretWasNotCalledOnClientSecretIdentity()
        {
            Mock<IClientSecretIdentity> clientSecretIdentityMock = CreateClientSecretIdentityMock();
            CommandHandler sut = CreateSut(false, clientSecretIdentityMock.Object);

            IAuthenticateClientSecretCommand command = CreateCommand();
            await sut.ExecuteAsync(command);

            clientSecretIdentityMock.Verify(m => m.ClientSecret, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientIdIsUnknown_AssertClearSensitiveDataWasNotCalledOnClientSecretIdentity()
        {
            Mock<IClientSecretIdentity> clientSecretIdentityMock = CreateClientSecretIdentityMock();
            CommandHandler sut = CreateSut(false, clientSecretIdentityMock.Object);

            IAuthenticateClientSecretCommand command = CreateCommand();
            await sut.ExecuteAsync(command);

            clientSecretIdentityMock.Verify(m => m.ClearSensitiveData(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientIdIsUnknown_AssertGenerateWasNotCalledOnTokenHelper()
        {
            CommandHandler sut = CreateSut(false);

            IAuthenticateClientSecretCommand command = CreateCommand();
            await sut.ExecuteAsync(command);

            _tokenHelperMock.Verify(m => m.Generate(It.IsAny<IClientSecretIdentity>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientIdIsUnknown_AssertAddTokenWasNotCalledOnClientSecretIdentity()
        {
            Mock<IClientSecretIdentity> clientSecretIdentityMock = CreateClientSecretIdentityMock();
            CommandHandler sut = CreateSut(false, clientSecretIdentityMock.Object);

            IAuthenticateClientSecretCommand command = CreateCommand();
            await sut.ExecuteAsync(command);

            clientSecretIdentityMock.Verify(m => m.AddToken(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientIdIsUnknown_ReturnsNull()
        {
            CommandHandler sut = CreateSut(false);

            IAuthenticateClientSecretCommand command = CreateCommand();
            IClientSecretIdentity result = await sut.ExecuteAsync(command);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientIdIsKnown_AssertClientSecretWasCalledOnClientSecretIdentity()
        {
            Mock<IClientSecretIdentity> clientSecretIdentityMock = CreateClientSecretIdentityMock();
            CommandHandler sut = CreateSut(clientSecretIdentity: clientSecretIdentityMock.Object);

            IAuthenticateClientSecretCommand command = CreateCommand();
            await sut.ExecuteAsync(command);

            clientSecretIdentityMock.Verify(m => m.ClientSecret, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientIdIsKnownAndClientSecretDoesNotMatch_AssertClearSensitiveDataWasNotCalledOnClientSecretIdentity()
        {
            string clientSecret = _fixture.Create<string>();
            Mock<IClientSecretIdentity> clientSecretIdentityMock = CreateClientSecretIdentityMock(clientSecret);
            CommandHandler sut = CreateSut(clientSecretIdentity: clientSecretIdentityMock.Object);

            string submittedClientSecret = _fixture.Create<string>();
            IAuthenticateClientSecretCommand command = CreateCommand(clientSecret: submittedClientSecret);
            await sut.ExecuteAsync(command);

            clientSecretIdentityMock.Verify(m => m.ClearSensitiveData(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientIdIsKnownAndClientSecretDoesNotMatch_AssertGenerateWasNotCalledOnTokenHelper()
        {
            string clientSecret = _fixture.Create<string>();
            IClientSecretIdentity clientSecretIdentity = CreateClientSecretIdentityMock(clientSecret).Object;
            CommandHandler sut = CreateSut(clientSecretIdentity: clientSecretIdentity);

            string submittedClientSecret = _fixture.Create<string>();
            IAuthenticateClientSecretCommand command = CreateCommand(clientSecret: submittedClientSecret);
            await sut.ExecuteAsync(command);

            _tokenHelperMock.Verify(m => m.Generate(It.IsAny<IClientSecretIdentity>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientIdIsKnownAndClientSecretDoesNotMatch_AssertAddTokenWasNotCalledOnClientSecretIdentity()
        {
            string clientSecret = _fixture.Create<string>();
            Mock<IClientSecretIdentity> clientSecretIdentityMock = CreateClientSecretIdentityMock(clientSecret);
            CommandHandler sut = CreateSut(clientSecretIdentity: clientSecretIdentityMock.Object);

            string submittedClientSecret = _fixture.Create<string>();
            IAuthenticateClientSecretCommand command = CreateCommand(clientSecret: submittedClientSecret);
            await sut.ExecuteAsync(command);

            clientSecretIdentityMock.Verify(m => m.AddToken(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientIdIsKnownAndClientSecretDoesNotMatch_ReturnNull()
        {
            string clientSecret = _fixture.Create<string>();
            Mock<IClientSecretIdentity> clientSecretIdentityMock = CreateClientSecretIdentityMock(clientSecret);
            CommandHandler sut = CreateSut(clientSecretIdentity: clientSecretIdentityMock.Object);

            string submittedClientSecret = _fixture.Create<string>();
            IAuthenticateClientSecretCommand command = CreateCommand(clientSecret: submittedClientSecret);
            IClientSecretIdentity result = await sut.ExecuteAsync(command);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientIdIsKnownAndClientSecretDoesMatch_AssertClearSensitiveDataWasCalledOnClientSecretIdentity()
        {
            string clientSecret = _fixture.Create<string>();
            Mock<IClientSecretIdentity> clientSecretIdentityMock = CreateClientSecretIdentityMock(clientSecret);
            CommandHandler sut = CreateSut(clientSecretIdentity: clientSecretIdentityMock.Object);

            string submittedClientSecret = clientSecret;
            IAuthenticateClientSecretCommand command = CreateCommand(clientSecret: submittedClientSecret);
            await sut.ExecuteAsync(command);

            clientSecretIdentityMock.Verify(m => m.ClearSensitiveData(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientIdIsKnownAndClientSecretDoesMatch_AssertGenerateWasCalledOnTokenHelper()
        {
            string clientSecret = _fixture.Create<string>();
            IClientSecretIdentity clientSecretIdentity = CreateClientSecretIdentityMock(clientSecret).Object;
            CommandHandler sut = CreateSut(clientSecretIdentity: clientSecretIdentity);

            string submittedClientSecret = clientSecret;
            IAuthenticateClientSecretCommand command = CreateCommand(clientSecret: submittedClientSecret);
            await sut.ExecuteAsync(command);

            _tokenHelperMock.Verify(m => m.Generate(It.Is<IClientSecretIdentity>(value => value == clientSecretIdentity)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientIdIsKnownAndClientSecretDoesMatch_AssertAddTokenWasCalledOnClientSecretIdentity()
        {
            string clientSecret = _fixture.Create<string>();
            Mock<IClientSecretIdentity> clientSecretIdentityMock = CreateClientSecretIdentityMock(clientSecret);
            string token = _fixture.Create<string>();
            CommandHandler sut = CreateSut(clientSecretIdentity: clientSecretIdentityMock.Object, token: token);

            string submittedClientSecret = clientSecret;
            IAuthenticateClientSecretCommand command = CreateCommand(clientSecret: submittedClientSecret);
            await sut.ExecuteAsync(command);

            clientSecretIdentityMock.Verify(m => m.AddToken(It.Is<string>(value => string.Compare(value, token, StringComparison.Ordinal) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenClientIdIsKnownAndClientSecretDoesMatch_ReturnsClientSecretIdentity()
        {
            string clientSecret = _fixture.Create<string>();
            IClientSecretIdentity clientSecretIdentity = CreateClientSecretIdentityMock(clientSecret).Object;
            CommandHandler sut = CreateSut(clientSecretIdentity: clientSecretIdentity);

            string submittedClientSecret = clientSecret;
            IAuthenticateClientSecretCommand command = CreateCommand(clientSecret: submittedClientSecret);
            IClientSecretIdentity result = await sut.ExecuteAsync(command);

            Assert.That(result, Is.EqualTo(clientSecretIdentity));
        }

        private CommandHandler CreateSut(bool hasClientSecretIdentityForClientId = true, IClientSecretIdentity clientSecretIdentity = null, string token = null)
        {
            _securityRepositoryMock.Setup(m => m.GetClientSecretIdentityAsync(It.IsAny<string>()))
                .Returns(Task.Run(() => hasClientSecretIdentityForClientId ? clientSecretIdentity ?? CreateClientSecretIdentityMock().Object : null));

            _tokenHelperMock.Setup(m => m.Generate(It.IsAny<IClientSecretIdentity>()))
                .Returns(token ?? _fixture.Create<string>());

            return new CommandHandler(_securityRepositoryMock.Object, _tokenHelperMock.Object);
        }

        private IAuthenticateClientSecretCommand CreateCommand(string clientId = null, string clientSecret = null)
        {
            return new AuthenticateClientSecretCommand(clientId ?? _fixture.Create<string>(), clientSecret ?? _fixture.Create<string>());
        }

        private Mock<IClientSecretIdentity> CreateClientSecretIdentityMock(string clientSecret = null)
        {
            Mock<IClientSecretIdentity> clientSecretIdentityMock = new Mock<IClientSecretIdentity>();
            clientSecretIdentityMock.Setup(m => m.ClientSecret)
                .Returns(clientSecret ?? _fixture.Create<string>());
            return clientSecretIdentityMock;
        }
    }
}
