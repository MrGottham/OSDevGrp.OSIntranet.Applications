using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers.UpdateBudgetAccountGroupCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.CommandHandlers.UpdateBudgetAccountGroupCommandHandler
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
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IUpdateBudgetAccountGroupCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertUpdateBudgetAccountGroupAsyncWasCalledOnAccountingRepository()
        {
            CommandHandler sut = CreateSut();

            IBudgetAccountGroup budgetAccountGroup = _fixture.BuildBudgetAccountGroupMock().Object;
            IUpdateBudgetAccountGroupCommand command = CreateCommandMock(budgetAccountGroup).Object;
            await sut.ExecuteAsync(command);

            _accountingRepositoryMock.Verify(m => m.UpdateBudgetAccountGroupAsync(It.Is<IBudgetAccountGroup>(value => value == budgetAccountGroup)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _accountingRepositoryMock.Setup(m => m.UpdateBudgetAccountGroupAsync(It.IsAny<IBudgetAccountGroup>()))
                .Returns(Task.Run(() => _fixture.BuildBudgetAccountGroupMock().Object));

            return new CommandHandler(_validatorMock.Object, _accountingRepositoryMock.Object);
        }

        private Mock<IUpdateBudgetAccountGroupCommand> CreateCommandMock(IBudgetAccountGroup budgetAccountGroup = null)
        {
            Mock<IUpdateBudgetAccountGroupCommand> commandMock = new Mock<IUpdateBudgetAccountGroupCommand>();
            commandMock.Setup(m => m.ToDomain())
                .Returns(budgetAccountGroup ?? _fixture.BuildBudgetAccountGroupMock().Object);
            return commandMock;
        }
    }
}