using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers.UpdateClientSecretIdentityCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.CommandHandlers.UpdateClientSecretIdentityCommandHandler
{
    [TestFixture]
    public class ExecuteAsyncTests : BusinessLogicTestBase
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<ISecurityRepository> _securityRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _securityRepositoryMock = new Mock<ISecurityRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertIdentifierWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IUpdateClientSecretIdentityCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.Identifier, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertGetClientSecretIdentityAsyncWasCalledOnSecurityRepository()
        {
            CommandHandler sut = CreateSut();

            int identifier = _fixture.Create<int>();
            IUpdateClientSecretIdentityCommand command = CreateCommandMock(identifier).Object;
            await sut.ExecuteAsync(command);

            _securityRepositoryMock.Verify(m => m.GetClientSecretIdentityAsync(It.Is<int>(value => value == identifier)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenCalledAndExistingClientIdentifierDoesNotExist_ThrowsIntranetSystemException()
        {
            CommandHandler sut = CreateSut(false);

            IUpdateClientSecretIdentityCommand command = CreateCommandMock().Object;
            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.ExecuteAsync(command));

            Assert.That(result.Message.Contains("existingClientSecretIdentity"), Is.True);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ObjectIsNull));
            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndExistingClientIdentifierExists_AssertClientIdWasCalledOnExistingClientSecretIdentity()
        {
            Mock<IClientSecretIdentity> existingClientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock();
            CommandHandler sut = CreateSut(existingClientSecretIdentity: existingClientSecretIdentityMock.Object);

            IUpdateClientSecretIdentityCommand command = CreateCommandMock().Object;
            await sut.ExecuteAsync(command);

            existingClientSecretIdentityMock.Verify(m => m.ClientId, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndExistingClientIdentifierExists_AssertClientSecretWasCalledOnExistingClientSecretIdentity()
        {
            Mock<IClientSecretIdentity> existingClientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock();
            CommandHandler sut = CreateSut(existingClientSecretIdentity: existingClientSecretIdentityMock.Object);

            IUpdateClientSecretIdentityCommand command = CreateCommandMock().Object;
            await sut.ExecuteAsync(command);

            existingClientSecretIdentityMock.Verify(m => m.ClientSecret, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndExistingClientIdentifierExists_AssertToDomainWithoutClientIdAndClientSecretWasNotCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IUpdateClientSecretIdentityCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndExistingClientIdentifierExists_AssertToDomainWithClientIdAndClientSecretWasCalledOnCommand()
        {
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            IClientSecretIdentity existingClientSecretIdentity = _fixture.BuildClientSecretIdentityMock(clientId, clientSecret).Object;
            CommandHandler sut = CreateSut(existingClientSecretIdentity: existingClientSecretIdentity);

            Mock<IUpdateClientSecretIdentityCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(
                    It.Is<string>(value => string.CompareOrdinal(value, clientId) == 0), 
                    It.Is<string>(value => string.CompareOrdinal(value, clientSecret) == 0)), 
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndExistingClientIdentifierExists_AssertUpdateClientSecretIdentityAsyncWasCalledOnSecurityRepository()
        {
            CommandHandler sut = CreateSut();

            IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock().Object;
            IUpdateClientSecretIdentityCommand command = CreateCommandMock(clientSecretIdentity: clientSecretIdentity).Object;
            await sut.ExecuteAsync(command);

            _securityRepositoryMock.Verify(m => m.UpdateClientSecretIdentityAsync(It.Is<IClientSecretIdentity>(value => value == clientSecretIdentity)), Times.Once);
        }

        private CommandHandler CreateSut(bool hasExistingClientSecretIdentity = true, IClientSecretIdentity existingClientSecretIdentity = null)
        {
            _securityRepositoryMock.Setup(m => m.GetClientSecretIdentityAsync(It.IsAny<int>()))
                .Returns(Task.Run(() => hasExistingClientSecretIdentity ? existingClientSecretIdentity ?? _fixture.BuildClientSecretIdentityMock().Object : null));

            _securityRepositoryMock.Setup(m => m.UpdateClientSecretIdentityAsync(It.IsAny<IClientSecretIdentity>()))
                .Returns(Task.Run(() => _fixture.BuildClientSecretIdentityMock().Object));

            return new CommandHandler(_validatorMock.Object, _securityRepositoryMock.Object);
        }

        private Mock<IUpdateClientSecretIdentityCommand> CreateCommandMock(int? identifier = null, IClientSecretIdentity clientSecretIdentity = null)
        {
            Mock<IUpdateClientSecretIdentityCommand> commandMock = new Mock<IUpdateClientSecretIdentityCommand>();
            commandMock.Setup(m => m.Identifier)
                .Returns(identifier ?? _fixture.Create<int>());
            commandMock.Setup(m => m.ToDomain(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(clientSecretIdentity ?? _fixture.BuildClientSecretIdentityMock().Object);
            return commandMock;
        }
    }
}