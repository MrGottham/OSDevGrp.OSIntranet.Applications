using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers.CreateBudgetAccountCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.CommandHandlers.CreateBudgetAccountCommandHandler
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
        public void ExecuteAsync_WhenCommandIsNull_ThrowsArgumentNullException()
        {
            CommandHandler sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("command"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnCreateBudgetAccountCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<ICreateBudgetAccountCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(It.Is<IAccountingRepository>(value => value == _accountingRepositoryMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertCreateBudgetAccountAsyncWasCalledOnAccountingRepository()
        {
            CommandHandler sut = CreateSut();

            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock().Object;
            ICreateBudgetAccountCommand command = CreateCommand(budgetAccount);
            await sut.ExecuteAsync(command);

            _accountingRepositoryMock.Verify(m => m.CreateBudgetAccountAsync(It.Is<IBudgetAccount>(value => value == budgetAccount)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _accountingRepositoryMock.Setup(m => m.CreateBudgetAccountAsync(It.IsAny<IBudgetAccount>()))
                .Returns(Task.FromResult(_fixture.BuildBudgetAccountMock().Object));

            return new CommandHandler(_validatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);
        }

        private ICreateBudgetAccountCommand CreateCommand(IBudgetAccount budgetAccount = null)
        {
            return CreateCommandMock(budgetAccount).Object;
        }

        private Mock<ICreateBudgetAccountCommand> CreateCommandMock(IBudgetAccount budgetAccount = null)
        {
            Mock<ICreateBudgetAccountCommand> commandMock = new Mock<ICreateBudgetAccountCommand>();
            commandMock.Setup(m => m.ToDomain(It.IsAny<IAccountingRepository>()))
                .Returns(budgetAccount ?? _fixture.BuildBudgetAccountMock().Object);
            return commandMock;
        }
    }
}