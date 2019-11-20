using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers.DeleteAccountingCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.CommandHandlers.DeleteAccountingCommandHandler
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
        public async Task ExecuteAsync_WhenCalled_AssertAccountingNumberWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IDeleteAccountingCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.AccountingNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertDeleteAccountingAsyncWasCalledOnAccountingRepository()
        {
            CommandHandler sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            IDeleteAccountingCommand command = CreateCommandMock(accountingNumber).Object;
            await sut.ExecuteAsync(command);

            _accountingRepositoryMock.Verify(m => m.DeleteAccountingAsync(It.Is<int>(value => value == accountingNumber)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _accountingRepositoryMock.Setup(m => m.DeleteAccountingAsync(It.IsAny<int>()))
                .Returns(Task.Run(() => (IAccounting) null));

            return new CommandHandler(_validatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);
        }

        private Mock<IDeleteAccountingCommand> CreateCommandMock(int? accountingNumber = null)
        {
            Mock<IDeleteAccountingCommand> commandMock = new Mock<IDeleteAccountingCommand>();
            commandMock.Setup(m => m.AccountingNumber)
                .Returns(accountingNumber ?? _fixture.Create<int>());
            return commandMock;
        }
    }
}