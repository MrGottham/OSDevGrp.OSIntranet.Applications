using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers.CreatePaymentTermCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.CommandHandlers.CreatePaymentTermCommandHandler
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

            Mock<ICreatePaymentTermCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertCreatePaymentTermAsyncWasCalledOnAccountingRepository()
        {
            CommandHandler sut = CreateSut();

            IPaymentTerm paymentTerm = _fixture.BuildPaymentTermMock().Object;
            ICreatePaymentTermCommand command = CreateCommandMock(paymentTerm).Object;
            await sut.ExecuteAsync(command);

            _accountingRepositoryMock.Verify(m => m.CreatePaymentTermAsync(It.Is<IPaymentTerm>(value => value == paymentTerm)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _accountingRepositoryMock.Setup(m => m.CreatePaymentTermAsync(It.IsAny<IPaymentTerm>()))
                .Returns(Task.Run(() => _fixture.BuildPaymentTermMock().Object));

            return new CommandHandler(_validatorMock.Object, _accountingRepositoryMock.Object);
        }

        private Mock<ICreatePaymentTermCommand> CreateCommandMock(IPaymentTerm paymentTerm = null)
        {
            Mock<ICreatePaymentTermCommand> commandMock = new Mock<ICreatePaymentTermCommand>();
            commandMock.Setup(m => m.ToDomain())
                .Returns(paymentTerm ?? _fixture.BuildPaymentTermMock().Object);
            return commandMock;
        }
    }
}