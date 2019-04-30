using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers.CreateBudgetAccountGroupCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.CommandHandlers.CreateBudgetAccountGroupCommandHandler
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

            Mock<ICreateBudgetAccountGroupCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertCreateBudgetAccountGroupAsyncWasCalledOnAccountingRepository()
        {
            CommandHandler sut = CreateSut();

            IBudgetAccountGroup budgetAccountGroup = _fixture.BuildBudgetAccountGroupMock().Object;
            ICreateBudgetAccountGroupCommand command = CreateCommandMock(budgetAccountGroup).Object;
            await sut.ExecuteAsync(command);

            _accountingRepositoryMock.Verify(m => m.CreateBudgetAccountGroupAsync(It.Is<IBudgetAccountGroup>(value => value == budgetAccountGroup)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _accountingRepositoryMock.Setup(m => m.CreateBudgetAccountGroupAsync(It.IsAny<IBudgetAccountGroup>()))
                .Returns(Task.Run(() => _fixture.BuildBudgetAccountGroupMock().Object));

            return new CommandHandler(_validatorMock.Object, _accountingRepositoryMock.Object);
        }

        private Mock<ICreateBudgetAccountGroupCommand> CreateCommandMock(IBudgetAccountGroup budgetAccountGroup = null)
        {
            Mock<ICreateBudgetAccountGroupCommand> commandMock = new Mock<ICreateBudgetAccountGroupCommand>();
            commandMock.Setup(m => m.ToDomain())
                .Returns(budgetAccountGroup ?? _fixture.BuildBudgetAccountGroupMock().Object);
            return commandMock;
        }
    }
}