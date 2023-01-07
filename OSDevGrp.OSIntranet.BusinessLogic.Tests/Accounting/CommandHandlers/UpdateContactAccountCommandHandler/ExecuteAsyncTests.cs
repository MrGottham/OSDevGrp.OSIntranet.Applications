using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;
using CommandHandler = OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers.UpdateContactAccountCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.CommandHandlers.UpdateContactAccountCommandHandler
{
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
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnUpdateContactAccountCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IUpdateContactAccountCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(It.Is<IAccountingRepository>(value => value == _accountingRepositoryMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertUpdateBudgetAccountAsyncWasCalledOnAccountingRepository()
        {
            CommandHandler sut = CreateSut();

            IContactAccount contactAccount = _fixture.BuildContactAccountMock().Object;
            IUpdateContactAccountCommand command = CreateCommand(contactAccount);
            await sut.ExecuteAsync(command);

            _accountingRepositoryMock.Verify(m => m.UpdateContactAccountAsync(It.Is<IContactAccount>(value => value == contactAccount)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _accountingRepositoryMock.Setup(m => m.UpdateContactAccountAsync(It.IsAny<IContactAccount>()))
                .Returns(Task.FromResult(_fixture.BuildContactAccountMock().Object));

            return new CommandHandler(_validatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);
        }

        private IUpdateContactAccountCommand CreateCommand(IContactAccount contactAccount = null)
        {
            return CreateCommandMock(contactAccount).Object;
        }

        private Mock<IUpdateContactAccountCommand> CreateCommandMock(IContactAccount contactAccount = null)
        {
            Mock<IUpdateContactAccountCommand> commandMock = new Mock<IUpdateContactAccountCommand>();
            commandMock.Setup(m => m.ToDomain(It.IsAny<IAccountingRepository>()))
                .Returns(contactAccount ?? _fixture.BuildContactAccountMock().Object);
            return commandMock;
        }
    }
}