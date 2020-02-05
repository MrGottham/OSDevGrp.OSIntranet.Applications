using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers.ContactCommandHandlerBase<OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands.IContactCommand>;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.CommandHandlers.ContactCommandHandlerBase
{
    [TestFixture]
    public class ExecuteAsyncTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Mock<IMicrosoftGraphRepository> _microsoftGraphRepositoryMock;
        private Mock<IContactRepository> _contactRepositoryMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _microsoftGraphRepositoryMock = new Mock<IMicrosoftGraphRepository>();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _contactRepositoryMock = new Mock<IContactRepository>();
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
        public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IContactCommand> commandMock = CreateContactCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value == _validatorMockContext.ValidatorMock.Object),
                    It.Is<IMicrosoftGraphRepository>(value => value == _microsoftGraphRepositoryMock.Object),
                    It.Is<IContactRepository>(value => value == _contactRepositoryMock.Object),
                    It.Is<IAccountingRepository>(value => value == _accountingRepositoryMock.Object)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertToTokenWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IContactCommand> commandMock = CreateContactCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToToken(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertManageRepositoryAsyncWasCalledWasCalledOnCommandHandler()
        {
            CommandHandler sut = CreateSut();

            IContactCommand command = CreateContactCommandMock().Object;
            await sut.ExecuteAsync(command);

            Assert.That(((Sut) sut).ManageRepositoryAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertManageRepositoryAsyncWasCalledWasCalledOnCommandHandlerWithSameCommand()
        {
            CommandHandler sut = CreateSut();

            IContactCommand command = CreateContactCommandMock().Object;
            await sut.ExecuteAsync(command);

            Assert.That(((Sut) sut).Command, Is.EqualTo(command));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertManageRepositoryAsyncWasCalledWasCalledOnCommandHandlerWithTokenFromCommand()
        {
            CommandHandler sut = CreateSut();

            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock().Object;
            IContactCommand command = CreateContactCommandMock(refreshableToken).Object;
            await sut.ExecuteAsync(command);

            Assert.That(((Sut) sut).Token, Is.EqualTo(refreshableToken));
        }

        private CommandHandler CreateSut()
        {
            return new Sut(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);
        }

        private Mock<IContactCommand> CreateContactCommandMock(IRefreshableToken refreshableToken = null)
        {
            Mock<IContactCommand> contactCommandMock = new Mock<IContactCommand>();
            contactCommandMock.Setup(m => m.ToToken())
                .Returns(refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object);
            return contactCommandMock;
        }

        private class Sut : CommandHandler
        {
            #region Constructor

            public Sut(IValidator validator, IMicrosoftGraphRepository microsoftGraphRepository, IContactRepository contactRepository, IAccountingRepository accountingRepository) 
                : base(validator, microsoftGraphRepository, contactRepository, accountingRepository)
            {
            }

            #endregion

            #region Properties

            public bool ManageRepositoryAsyncWasCalled { get; private set; }

            public IContactCommand Command { get; private set; }

            public IRefreshableToken Token { get; private set; }

            #endregion

            #region Methods

            protected override Task ManageRepositoryAsync(IContactCommand command, IRefreshableToken token)
            {
                Assert.That(command, Is.Not.Null);
                Assert.That(token, Is.Not.Null);

                ManageRepositoryAsyncWasCalled = true;
                Command = command;
                Token = token;

                return Task.CompletedTask;
            }

            #endregion
        }
    }
}