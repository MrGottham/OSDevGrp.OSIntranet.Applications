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
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers.UpdateAccountCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.CommandHandlers.UpdateAccountCommandHandler
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
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnUpdateAccountCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IUpdateAccountCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(It.Is<IAccountingRepository>(value => value == _accountingRepositoryMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertUpdateAccountAsyncWasCalledOnAccountingRepository()
        {
            CommandHandler sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock().Object;
            IUpdateAccountCommand command = CreateCommand(account);
            await sut.ExecuteAsync(command);

            _accountingRepositoryMock.Verify(m => m.UpdateAccountAsync(It.Is<IAccount>(value => value == account)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _accountingRepositoryMock.Setup(m => m.UpdateAccountAsync(It.IsAny<IAccount>()))
                .Returns(Task.FromResult(_fixture.BuildAccountMock().Object));

            return new CommandHandler(_validatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);
        }

        private IUpdateAccountCommand CreateCommand(IAccount account = null)
        {
            return CreateCommandMock(account).Object;
        }

        private Mock<IUpdateAccountCommand> CreateCommandMock(IAccount account = null)
        {
            Mock<IUpdateAccountCommand> commandMock = new Mock<IUpdateAccountCommand>();
            commandMock.Setup(m => m.ToDomain(It.IsAny<IAccountingRepository>()))
                .Returns(account ?? _fixture.BuildAccountMock().Object);
            return commandMock;
        }
    }
}