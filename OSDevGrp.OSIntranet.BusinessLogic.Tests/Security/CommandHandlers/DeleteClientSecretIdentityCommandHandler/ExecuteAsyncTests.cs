using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;
using CommandHandler = OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers.DeleteClientSecretIdentityCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.CommandHandlers.DeleteClientSecretIdentityCommandHandler
{
	[TestFixture]
    public class ExecuteAsyncTests
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

            Mock<IDeleteClientSecretIdentityCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.Identifier, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertDeleteClientSecretIdentityAsyncWasCalledOnSecurityRepository()
        {
            CommandHandler sut = CreateSut();

            int identifier = _fixture.Create<int>();
            IDeleteClientSecretIdentityCommand command = CreateCommandMock(identifier).Object;
            await sut.ExecuteAsync(command);

            _securityRepositoryMock.Verify(m => m.DeleteClientSecretIdentityAsync(It.Is<int>(value => value == identifier)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _securityRepositoryMock.Setup(m => m.DeleteClientSecretIdentityAsync(It.IsAny<int>()))
                .Returns(Task.Run(() => (IClientSecretIdentity) null));

            return new CommandHandler(_validatorMock.Object, _securityRepositoryMock.Object);
        }

        private Mock<IDeleteClientSecretIdentityCommand> CreateCommandMock(int? identifier = null)
        {
            Mock<IDeleteClientSecretIdentityCommand> commandMock = new Mock<IDeleteClientSecretIdentityCommand>();
            commandMock.Setup(m => m.Identifier)
                .Returns(identifier ?? _fixture.Create<int>());
            return commandMock;
        }
    }
}