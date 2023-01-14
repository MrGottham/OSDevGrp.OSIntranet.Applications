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
using CommandHandler = OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers.UpdatePaymentTermCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.CommandHandlers.UpdatePaymentTermCommandHandler
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

            Mock<IUpdatePaymentTermCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertUpdatePaymentTermAsyncWasCalledOnAccountingRepository()
        {
            CommandHandler sut = CreateSut();

            IPaymentTerm paymentTerm = _fixture.BuildPaymentTermMock().Object;
            IUpdatePaymentTermCommand command = CreateCommandMock(paymentTerm).Object;
            await sut.ExecuteAsync(command);

            _accountingRepositoryMock.Verify(m => m.UpdatePaymentTermAsync(It.Is<IPaymentTerm>(value => value == paymentTerm)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _accountingRepositoryMock.Setup(m => m.UpdatePaymentTermAsync(It.IsAny<IPaymentTerm>()))
                .Returns(Task.FromResult(_fixture.BuildPaymentTermMock().Object));

            return new CommandHandler(_validatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object);
        }

        private Mock<IUpdatePaymentTermCommand> CreateCommandMock(IPaymentTerm paymentTerm = null)
        {
            Mock<IUpdatePaymentTermCommand> commandMock = new Mock<IUpdatePaymentTermCommand>();
            commandMock.Setup(m => m.ToDomain())
                .Returns(paymentTerm ?? _fixture.BuildPaymentTermMock().Object);
            return commandMock;
        }
    }
}