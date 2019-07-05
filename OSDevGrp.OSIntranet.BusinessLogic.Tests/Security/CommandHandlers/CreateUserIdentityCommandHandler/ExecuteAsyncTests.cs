using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers.CreateUserIdentityCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.CommandHandlers.CreateUserIdentityCommandHandler
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
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<ICreateUserIdentityCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertCreateUserIdentityAsyncWasCalledOnSecurityRepository()
        {
            CommandHandler sut = CreateSut();

            IUserIdentity userIdentity = _fixture.BuildUserIdentityMock().Object;
            ICreateUserIdentityCommand command = CreateCommandMock(userIdentity).Object;
            await sut.ExecuteAsync(command);

            _securityRepositoryMock.Verify(m => m.CreateUserIdentityAsync(It.Is<IUserIdentity>(value => value == userIdentity)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _securityRepositoryMock.Setup(m => m.CreateUserIdentityAsync(It.IsAny<IUserIdentity>()))
                .Returns(Task.Run(() => _fixture.BuildUserIdentityMock().Object));

            return new CommandHandler(_validatorMock.Object, _securityRepositoryMock.Object);
        }

        private Mock<ICreateUserIdentityCommand> CreateCommandMock(IUserIdentity userIdentity = null)
        {
            Mock<ICreateUserIdentityCommand> commandMock = new Mock<ICreateUserIdentityCommand>();
            commandMock.Setup(m => m.ToDomain())
                .Returns(userIdentity ?? _fixture.BuildUserIdentityMock().Object);
            return commandMock;
        }
    }
}