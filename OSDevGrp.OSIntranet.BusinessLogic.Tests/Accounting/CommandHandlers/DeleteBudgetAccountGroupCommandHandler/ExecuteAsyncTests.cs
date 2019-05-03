using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers.DeleteBudgetAccountGroupCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.CommandHandlers.DeleteBudgetAccountGroupCommandHandler
{
    [TestFixture]
    public class ExecuteAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertToNumberWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IDeleteBudgetAccountGroupCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertDeleteBudgetAccountGroupAsyncWasCalledOnAccountingRepository()
        {
            CommandHandler sut = CreateSut();

            int number = _fixture.Create<int>();
            IDeleteBudgetAccountGroupCommand command = CreateCommandMock(number).Object;
            await sut.ExecuteAsync(command);

            _accountingRepositoryMock.Verify(m => m.DeleteBudgetAccountGroupAsync(It.Is<int>(value => value == number)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _accountingRepositoryMock.Setup(m => m.DeleteBudgetAccountGroupAsync(It.IsAny<int>()))
                .Returns(Task.Run(() => (IBudgetAccountGroup) null));

            return new CommandHandler(_validatorMock.Object, _accountingRepositoryMock.Object);
        }

        private Mock<IDeleteBudgetAccountGroupCommand> CreateCommandMock(int? number = null)
        {
            Mock<IDeleteBudgetAccountGroupCommand> commandMock = new Mock<IDeleteBudgetAccountGroupCommand>();
            commandMock.Setup(m => m.Number)
                .Returns(number ?? _fixture.Create<int>());
            return commandMock;
        }
    }
}