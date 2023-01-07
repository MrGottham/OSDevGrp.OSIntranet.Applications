using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;
using CommandHandler = OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers.DeleteContactAccountCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.CommandHandlers.DeleteContactAccountCommandHandler
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
        public void ExecuteAsync_WhenCommandIsNull_ThrowsArgumentNullException()
        {
            CommandHandler sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("command"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertAccountingNumberWasCalledOnDeleteContactAccountCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IDeleteContactAccountCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.AccountingNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertAccountNumberWasCalledOnDeleteContactAccountCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IDeleteContactAccountCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.AccountNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertDeleteContactAccountAsyncWasCalledOnAccountingRepository()
        {
            CommandHandler sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            IDeleteContactAccountCommand command = CreateCommand(accountingNumber, accountNumber);
            await sut.ExecuteAsync(command);

            _accountingRepositoryMock.Verify(m => m.DeleteContactAccountAsync(
                    It.Is<int>(value => value == accountingNumber),
                    It.Is<string>(value => string.CompareOrdinal(value, accountNumber) == 0)),
                Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _accountingRepositoryMock.Setup(m => m.DeleteContactAccountAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult<IContactAccount>(null));

            return new CommandHandler(_validatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);
        }

        private IDeleteContactAccountCommand CreateCommand(int? accountingNumber = null, string accountNumber = null)
        {
            return CreateCommandMock(accountingNumber, accountNumber).Object;
        }

        private Mock<IDeleteContactAccountCommand> CreateCommandMock(int? accountingNumber = null, string accountNumber = null)
        {
            Mock<IDeleteContactAccountCommand> commandMock = new Mock<IDeleteContactAccountCommand>();
            commandMock.Setup(m => m.AccountingNumber)
                .Returns(accountingNumber ?? _fixture.Create<int>());
            commandMock.Setup(m => m.AccountNumber)
                .Returns(accountNumber ?? _fixture.Create<string>());
            return commandMock;
        }
    }
}