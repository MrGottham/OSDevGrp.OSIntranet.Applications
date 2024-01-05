using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;
using CommandHandler = OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers.DeleteUserIdentityCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.CommandHandlers.DeleteUserIdentityCommandHandler
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

            Mock<IDeleteUserIdentityCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.Identifier, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertDeleteUserIdentityAsyncWasCalledOnSecurityRepository()
        {
            CommandHandler sut = CreateSut();

            int identifier = _fixture.Create<int>();
            IDeleteUserIdentityCommand command = CreateCommandMock(identifier).Object;
            await sut.ExecuteAsync(command);

            _securityRepositoryMock.Verify(m => m.DeleteUserIdentityAsync(It.Is<int>(value => value == identifier)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _securityRepositoryMock.Setup(m => m.DeleteUserIdentityAsync(It.IsAny<int>()))
                .Returns(Task.Run(() => (IUserIdentity) null));

            return new CommandHandler(_validatorMock.Object, _securityRepositoryMock.Object);
        }

        private Mock<IDeleteUserIdentityCommand> CreateCommandMock(int? identifier = null)
        {
            Mock<IDeleteUserIdentityCommand> commandMock = new Mock<IDeleteUserIdentityCommand>();
            commandMock.Setup(m => m.Identifier)
                .Returns(identifier ?? _fixture.Create<int>());
            return commandMock;
        }
    }
}