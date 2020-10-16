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
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers.CreateAccountCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.CommandHandlers.CreateAccountCommandHandler
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
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnCreateAccountCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<ICreateAccountCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(It.Is<IAccountingRepository>(value => value == _accountingRepositoryMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertCreateAccountAsyncWasCalledOnAccountingRepository()
        {
            CommandHandler sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock().Object;
            ICreateAccountCommand command = CreateCommand(account);
            await sut.ExecuteAsync(command);

            _accountingRepositoryMock.Verify(m => m.CreateAccountAsync(It.Is<IAccount>(value => value == account)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _accountingRepositoryMock.Setup(m => m.CreateAccountAsync(It.IsAny<IAccount>()))
                .Returns(Task.FromResult(_fixture.BuildAccountMock().Object));

            return new CommandHandler(_validatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);
        }

        private ICreateAccountCommand CreateCommand(IAccount account = null)
        {
            return CreateCommandMock(account).Object;
        }

        private Mock<ICreateAccountCommand> CreateCommandMock(IAccount account = null)
        {
            Mock<ICreateAccountCommand> commandMock = new Mock<ICreateAccountCommand>();
            commandMock.Setup(m => m.ToDomain(It.IsAny<IAccountingRepository>()))
                .Returns(account ?? _fixture.BuildAccountMock().Object);
            return commandMock;
        }
    }
}