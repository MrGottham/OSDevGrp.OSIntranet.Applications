using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;
using CommandHandler = OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers.UpdateAccountGroupCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.CommandHandlers.UpdateAccountGroupCommandHandler
{
    [TestFixture]
    public class ExecuteAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IUpdateAccountGroupCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertUpdateAccountGroupAsyncWasCalledOnAccountingRepository()
        {
            CommandHandler sut = CreateSut();

            IAccountGroup accountGroup = _fixture.BuildAccountGroupMock().Object;
            IUpdateAccountGroupCommand command = CreateCommandMock(accountGroup).Object;
            await sut.ExecuteAsync(command);

            _accountingRepositoryMock.Verify(m => m.UpdateAccountGroupAsync(It.Is<IAccountGroup>(value => value == accountGroup)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _accountingRepositoryMock.Setup(m => m.UpdateAccountGroupAsync(It.IsAny<IAccountGroup>()))
                .Returns(Task.FromResult(_fixture.BuildAccountGroupMock().Object));

            return new CommandHandler(_validatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object);
        }

        private Mock<IUpdateAccountGroupCommand> CreateCommandMock(IAccountGroup accountGroup = null)
        {
            Mock<IUpdateAccountGroupCommand> commandMock = new Mock<IUpdateAccountGroupCommand>();
            commandMock.Setup(m => m.ToDomain())
                .Returns(accountGroup ?? _fixture.BuildAccountGroupMock().Object);
            return commandMock;
        }
    }
}