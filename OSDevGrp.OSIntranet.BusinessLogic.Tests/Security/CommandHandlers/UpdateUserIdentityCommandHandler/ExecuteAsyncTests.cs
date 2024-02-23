using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;
using CommandHandler = OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers.UpdateUserIdentityCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.CommandHandlers.UpdateUserIdentityCommandHandler
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
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IUpdateUserIdentityCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertUpdateUserIdentityAsyncWasCalledOnSecurityRepository()
        {
            CommandHandler sut = CreateSut();

            IUserIdentity userIdentity = _fixture.BuildUserIdentityMock().Object;
            IUpdateUserIdentityCommand command = CreateCommandMock(userIdentity).Object;
            await sut.ExecuteAsync(command);

            _securityRepositoryMock.Verify(m => m.UpdateUserIdentityAsync(It.Is<IUserIdentity>(value => value == userIdentity)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _securityRepositoryMock.Setup(m => m.UpdateUserIdentityAsync(It.IsAny<IUserIdentity>()))
                .Returns(Task.Run(() => _fixture.BuildUserIdentityMock().Object));

            return new CommandHandler(_validatorMock.Object, _securityRepositoryMock.Object);
        }

        private Mock<IUpdateUserIdentityCommand> CreateCommandMock(IUserIdentity userIdentity = null)
        {
            Mock<IUpdateUserIdentityCommand> commandMock = new Mock<IUpdateUserIdentityCommand>();
            commandMock.Setup(m => m.ToDomain())
                .Returns(userIdentity ?? _fixture.BuildUserIdentityMock().Object);
            return commandMock;
        }
    }
}