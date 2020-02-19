using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers.DeleteContactCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.CommandHandlers.DeleteContactCommandHandler
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
        public async Task ExecuteAsync_WhenCalled_AssertExternalIdentifierWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IDeleteContactCommand> commandMock = CreateDeleteContactCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ExternalIdentifier, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandDoesNotHaveExternalIdentifier_AssertGetExistingContactAsyncWasNotCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IDeleteContactCommand> commandMock = CreateDeleteContactCommandMock(false);
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.GetExistingContactAsync(It.IsAny<IMicrosoftGraphRepository>(), It.IsAny<IContactRepository>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandDoesNotHaveExternalIdentifier_AssertExternalIdentifierWasNotCalledOnExistingContactFromCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IContact> existingContactMock = _fixture.BuildContactMock();
            IDeleteContactCommand command = CreateDeleteContactCommandMock(false, existingContact: existingContactMock.Object).Object;
            await sut.ExecuteAsync(command);

            existingContactMock.Verify(m => m.ExternalIdentifier, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandDoesNotHaveExternalIdentifier_AssertDeleteContactSupplementAsyncWasNotCalledOnContactRepository()
        {
            CommandHandler sut = CreateSut();

            IDeleteContactCommand command = CreateDeleteContactCommandMock(false).Object;
            await sut.ExecuteAsync(command);

            _contactRepositoryMock.Verify(m => m.DeleteContactSupplementAsync(It.IsAny<IContact>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandDoesNotHaveExternalIdentifier_AssertDeleteContactAsyncWasNotCalledOnMicrosoftGraphRepository()
        {
            CommandHandler sut = CreateSut();

            IDeleteContactCommand command = CreateDeleteContactCommandMock(false).Object;
            await sut.ExecuteAsync(command);

            _microsoftGraphRepositoryMock.Verify(m => m.DeleteContactAsync(It.IsAny<IRefreshableToken>(), It.IsAny<string>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifier_AssertGetExistingContactAsyncWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IDeleteContactCommand> commandMock = CreateDeleteContactCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.GetExistingContactAsync(
                    It.Is<IMicrosoftGraphRepository>(value => value == _microsoftGraphRepositoryMock.Object),
                    It.Is<IContactRepository>(value => value == _contactRepositoryMock.Object)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndNoExistingContactWasReturnedFromCommand_AssertExternalIdentifierWasNotCalledOnExistingContactFromCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IContact> existingContactMock = _fixture.BuildContactMock();
            IDeleteContactCommand command = CreateDeleteContactCommandMock(hasExistingContact: false, existingContact: existingContactMock.Object).Object;
            await sut.ExecuteAsync(command);

            existingContactMock.Verify(m => m.ExternalIdentifier, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndNoExistingContactWasReturnedFromCommand_AssertDeleteContactSupplementAsyncWasNotCalledOnContactRepository()
        {
            CommandHandler sut = CreateSut();

            IDeleteContactCommand command = CreateDeleteContactCommandMock(hasExistingContact: false).Object;
            await sut.ExecuteAsync(command);

            _contactRepositoryMock.Verify(m => m.DeleteContactSupplementAsync(It.IsAny<IContact>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndNoExistingContactWasReturnedFromCommand_AssertDeleteContactAsyncWasNotCalledOnMicrosoftGraphRepository()
        {
            CommandHandler sut = CreateSut();

            IDeleteContactCommand command = CreateDeleteContactCommandMock(hasExistingContact: false).Object;
            await sut.ExecuteAsync(command);

            _microsoftGraphRepositoryMock.Verify(m => m.DeleteContactAsync(It.IsAny<IRefreshableToken>(), It.IsAny<string>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndExistingContactWasReturnedFromCommand_AssertExternalIdentifierWasCalledOnExistingContactFromCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IContact> existingContactMock = _fixture.BuildContactMock();
            IDeleteContactCommand command = CreateDeleteContactCommandMock(existingContact: existingContactMock.Object).Object;
            await sut.ExecuteAsync(command);

            existingContactMock.Verify(m => m.ExternalIdentifier, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndExistingContactWithoutExternalIdentifierWasReturnedFromCommand_AssertDeleteContactSupplementAsyncWasNotCalledOnContactRepository()
        {
            CommandHandler sut = CreateSut();

            IContact existingContact = _fixture.BuildContactMock(hasExternalIdentifier: false).Object;
            IDeleteContactCommand command = CreateDeleteContactCommandMock(existingContact: existingContact).Object;
            await sut.ExecuteAsync(command);

            _contactRepositoryMock.Verify(m => m.DeleteContactSupplementAsync(It.IsAny<IContact>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndExistingContactWithoutExternalIdentifierWasReturnedFromCommand_AssertDeleteContactAsyncWasNotCalledOnMicrosoftGraphRepository()
        {
            CommandHandler sut = CreateSut();

            IContact existingContact = _fixture.BuildContactMock(hasExternalIdentifier: false).Object;
            IDeleteContactCommand command = CreateDeleteContactCommandMock(existingContact: existingContact).Object;
            await sut.ExecuteAsync(command);

            _microsoftGraphRepositoryMock.Verify(m => m.DeleteContactAsync(It.IsAny<IRefreshableToken>(), It.IsAny<string>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndExistingContactWithExternalIdentifierWasReturnedFromCommand_AssertDeleteContactSupplementAsyncWasCalledOnContactRepositoryWithExistingContactFromCommand()
        {
            CommandHandler sut = CreateSut();

            IContact existingContact = _fixture.BuildContactMock().Object;
            IDeleteContactCommand command = CreateDeleteContactCommandMock(existingContact: existingContact).Object;
            await sut.ExecuteAsync(command);

            _contactRepositoryMock.Verify(m => m.DeleteContactSupplementAsync(It.Is<IContact>(value => value == existingContact)), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndExistingContactWithExternalIdentifierWasReturnedFromCommand_AssertDeleteContactAsyncWasCalledOnMicrosoftGraphRepositoryWithExternalIdentifierFromExistingContactFromCommand()
        {
            CommandHandler sut = CreateSut();

            string externalIdentifier = _fixture.Create<string>();
            IContact existingContact = _fixture.BuildContactMock(externalIdentifier: externalIdentifier).Object;
            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock().Object;
            IDeleteContactCommand command = CreateDeleteContactCommandMock(existingContact: existingContact, refreshableToken: refreshableToken).Object;
            await sut.ExecuteAsync(command);

            _microsoftGraphRepositoryMock.Verify(m => m.DeleteContactAsync(
                    It.Is<IRefreshableToken>(value => value == refreshableToken),
                    It.Is<string>(value => string.CompareOrdinal(value, externalIdentifier) == 0)),
                Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _contactRepositoryMock.Setup(m => m.DeleteContactSupplementAsync(It.IsAny<IContact>()))
                .Returns(Task.Run(() => (IContact) null));
            _microsoftGraphRepositoryMock.Setup(m => m.DeleteContactAsync(It.IsAny<IRefreshableToken>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            return new CommandHandler(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);
        }

        private Mock<IDeleteContactCommand> CreateDeleteContactCommandMock(bool hasExternalIdentifier = true, string externalIdentifier = null, bool hasExistingContact = true, IContact existingContact = null, IRefreshableToken refreshableToken = null)
        {
            Mock<IDeleteContactCommand> deleteContactCommandMock = new Mock<IDeleteContactCommand>();
            deleteContactCommandMock.Setup(m => m.ExternalIdentifier)
                .Returns(hasExternalIdentifier ? externalIdentifier ?? _fixture.Create<string>() : null);
            deleteContactCommandMock.Setup(m => m.GetExistingContactAsync(It.IsAny<IMicrosoftGraphRepository>(), It.IsAny<IContactRepository>()))
                .Returns(Task.Run(() => hasExistingContact ? existingContact ?? _fixture.BuildContactMock().Object : null));
            deleteContactCommandMock.Setup(m => m.ToToken())
                .Returns(refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object);
            return deleteContactCommandMock;
        }
    }
}