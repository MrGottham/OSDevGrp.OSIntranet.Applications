using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers.DeletePaymentTermCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.CommandHandlers.DeletePaymentTermCommandHandler
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
        public async Task ExecuteAsync_WhenCalled_AssertNumberWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IDeletePaymentTermCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertDeletePaymentTermAsyncWasCalledOnAccountingRepository()
        {
            CommandHandler sut = CreateSut();

            int number = _fixture.Create<int>();
            IDeletePaymentTermCommand command = CreateCommandMock(number).Object;
            await sut.ExecuteAsync(command);

            _accountingRepositoryMock.Verify(m => m.DeletePaymentTermAsync(It.Is<int>(value => value == number)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _accountingRepositoryMock.Setup(m => m.DeletePaymentTermAsync(It.IsAny<int>()))
                .Returns(Task.Run(() => (IPaymentTerm) null));

            return new CommandHandler(_validatorMock.Object, _accountingRepositoryMock.Object);
        }

        private Mock<IDeletePaymentTermCommand> CreateCommandMock(int? number = null)
        {
            Mock<IDeletePaymentTermCommand> commandMock = new Mock<IDeletePaymentTermCommand>();
            commandMock.Setup(m => m.Number)
                .Returns(number ?? _fixture.Create<int>());
            return commandMock;
        }
    }
}