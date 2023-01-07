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
using CommandHandler = OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers.CreateAccountingCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.CommandHandlers.CreateAccountingCommandHandler
{
    [TestFixture]
    public class ExecuteAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Mock<ICommonRepository> _commonRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _commonRepositoryMock = new Mock<ICommonRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<ICreateAccountingCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertCreateAccountingAsyncWasCalledOnAccountingRepository()
        {
            CommandHandler sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            ICreateAccountingCommand command = CreateCommandMock(accounting).Object;
            await sut.ExecuteAsync(command);

            _accountingRepositoryMock.Verify(m => m.CreateAccountingAsync(It.Is<IAccounting>(value => value == accounting)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _accountingRepositoryMock.Setup(m => m.CreateAccountingAsync(It.IsAny<IAccounting>()))
                .Returns(Task.FromResult(_fixture.BuildAccountingMock().Object));

            return new CommandHandler(_validatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);
        }

        private Mock<ICreateAccountingCommand> CreateCommandMock(IAccounting accounting = null)
        {
            Mock<ICreateAccountingCommand> commandMock = new Mock<ICreateAccountingCommand>();
            commandMock.Setup(m => m.ToDomain(It.IsAny<ICommonRepository>()))
                .Returns(accounting ?? _fixture.BuildAccountingMock().Object);
            return commandMock;
        }
    }
}