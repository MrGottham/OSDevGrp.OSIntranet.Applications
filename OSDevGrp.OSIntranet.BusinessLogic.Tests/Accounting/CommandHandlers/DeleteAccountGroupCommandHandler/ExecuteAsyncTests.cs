using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;
using CommandHandler = OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers.DeleteAccountGroupCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.CommandHandlers.DeleteAccountGroupCommandHandler
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
        public async Task ExecuteAsync_WhenCalled_AssertToNumberWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IDeleteAccountGroupCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertDeleteAccountGroupAsyncWasCalledOnAccountingRepository()
        {
            CommandHandler sut = CreateSut();

            int number = _fixture.Create<int>();
            IDeleteAccountGroupCommand command = CreateCommandMock(number).Object;
            await sut.ExecuteAsync(command);

            _accountingRepositoryMock.Verify(m => m.DeleteAccountGroupAsync(It.Is<int>(value => value == number)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _accountingRepositoryMock.Setup(m => m.DeleteAccountGroupAsync(It.IsAny<int>()))
                .Returns(Task.FromResult((IAccountGroup)null));

            return new CommandHandler(_validatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object);
        }

        private Mock<IDeleteAccountGroupCommand> CreateCommandMock(int? number = null)
        {
            Mock<IDeleteAccountGroupCommand> commandMock = new Mock<IDeleteAccountGroupCommand>();
            commandMock.Setup(m => m.Number)
                .Returns(number ?? _fixture.Create<int>());
            return commandMock;
        }
    }
}