using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers.UpdateAccountingCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.CommandHandlers.UpdateAccountingCommandHandler
{
    [TestFixture]
    public class ExecuteAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Mock<ICommonRepository> _commonRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _commonRepositoryMock = new Mock<ICommonRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IUpdateAccountingCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertUpdateAccountingAsyncWasCalledOnAccountingRepository()
        {
            CommandHandler sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IUpdateAccountingCommand command = CreateCommandMock(accounting).Object;
            await sut.ExecuteAsync(command);

            _accountingRepositoryMock.Verify(m => m.UpdateAccountingAsync(It.Is<IAccounting>(value => value == accounting)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _accountingRepositoryMock.Setup(m => m.UpdateAccountingAsync(It.IsAny<IAccounting>()))
                .Returns(Task.Run(() => _fixture.BuildAccountingMock().Object));

            return new CommandHandler(_validatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);
        }

        private Mock<IUpdateAccountingCommand> CreateCommandMock(IAccounting accounting = null)
        {
            Mock<IUpdateAccountingCommand> commandMock = new Mock<IUpdateAccountingCommand>();
            commandMock.Setup(m => m.ToDomain(It.IsAny<ICommonRepository>()))
                .Returns(accounting ?? _fixture.BuildAccountingMock().Object);
            return commandMock;
        }
    }
}