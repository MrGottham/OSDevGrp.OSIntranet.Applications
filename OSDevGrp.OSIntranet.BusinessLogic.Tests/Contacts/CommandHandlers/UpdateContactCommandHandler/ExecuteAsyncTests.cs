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
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers.UpdateContactCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.CommandHandlers.UpdateContactCommandHandler
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

            Mock<IUpdateContactCommand> commandMock = CreateUpdateContactCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ExternalIdentifier, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandDoesNotHaveExternalIdentifier_AssertToDomainWasNotCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IUpdateContactCommand> commandMock = CreateUpdateContactCommandMock(false);
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(It.IsAny<IContactRepository>(), It.IsAny<IAccountingRepository>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandDoesNotHaveExternalIdentifier_AssertGetExistingContactAsyncWasNotCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IUpdateContactCommand> commandMock = CreateUpdateContactCommandMock(false);
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.GetExistingContactAsync(It.IsAny<IMicrosoftGraphRepository>(), It.IsAny<IContactRepository>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandDoesNotHaveExternalIdentifier_AssertInternalIdentifierWasNotCalledOnExistingContactFromCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IContact> existingContactMock = _fixture.BuildContactMock();
            IUpdateContactCommand command = CreateUpdateContactCommandMock(false, existingContact: existingContactMock.Object).Object;
            await sut.ExecuteAsync(command);

            existingContactMock.Verify(m => m.InternalIdentifier, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandDoesNotHaveExternalIdentifier_AssertInternalIdentifierSetterWasNotCalledOnContactFromToDomainOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IContact> toDomainContactMock = _fixture.BuildContactMock();
            IUpdateContactCommand command = CreateUpdateContactCommandMock(false, toDomainContactMock.Object).Object;
            await sut.ExecuteAsync(command);

            toDomainContactMock.VerifySet(m => m.InternalIdentifier = It.IsAny<string>(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandDoesNotHaveExternalIdentifier_AssertUpdateContactAsyncWasNotCalledOnMicrosoftGraphRepository()
        {
            CommandHandler sut = CreateSut();

            IUpdateContactCommand command = CreateUpdateContactCommandMock(false).Object;
            await sut.ExecuteAsync(command);

            _microsoftGraphRepositoryMock.Verify(m => m.UpdateContactAsync(It.IsAny<IRefreshableToken>(), It.IsAny<IContact>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandDoesNotHaveExternalIdentifier_AssertExternalIdentifierWasNotCalledOnUpdatedContactFromMicrosoftGraphRepository()
        {
            Mock<IContact> updatedMicrosoftGraphContactMock = _fixture.BuildContactMock();
            CommandHandler sut = CreateSut(updatedMicrosoftGraphContact: updatedMicrosoftGraphContactMock.Object);

            IUpdateContactCommand command = CreateUpdateContactCommandMock(false).Object;
            await sut.ExecuteAsync(command);

            updatedMicrosoftGraphContactMock.Verify(m => m.ExternalIdentifier, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandDoesNotHaveExternalIdentifier_AssertExternalIdentifierSetterWasNotCalledOnContactFromToDomainOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IContact> toDomainContactMock = _fixture.BuildContactMock();
            IUpdateContactCommand command = CreateUpdateContactCommandMock(false, toDomainContactMock.Object).Object;
            await sut.ExecuteAsync(command);

            toDomainContactMock.VerifySet(m => m.ExternalIdentifier = It.IsAny<string>(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandDoesNotHaveExternalIdentifier_AssertExternalIdentifierWasNotCalledOnExistingContactFromCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IContact> existingContactMock = _fixture.BuildContactMock();
            IUpdateContactCommand command = CreateUpdateContactCommandMock(false, existingContact: existingContactMock.Object).Object;
            await sut.ExecuteAsync(command);

            existingContactMock.Verify(m => m.ExternalIdentifier, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandDoesNotHaveExternalIdentifier_AssertCreateOrUpdateContactSupplementAsyncWasNotCalledOnContactRepository()
        {
            CommandHandler sut = CreateSut();

            IUpdateContactCommand command = CreateUpdateContactCommandMock(false).Object;
            await sut.ExecuteAsync(command);

            _contactRepositoryMock.Verify(m => m.CreateOrUpdateContactSupplementAsync(It.IsAny<IContact>(), It.IsAny<string>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifier_AssertToDomainWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IUpdateContactCommand> commandMock = CreateUpdateContactCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(
                    It.Is<IContactRepository>(value => value == _contactRepositoryMock.Object),
                    It.Is<IAccountingRepository>(value => value == _accountingRepositoryMock.Object)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifier_AssertGetExistingContactAsyncWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IUpdateContactCommand> commandMock = CreateUpdateContactCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.GetExistingContactAsync(
                    It.Is<IMicrosoftGraphRepository>(value => value == _microsoftGraphRepositoryMock.Object),
                    It.Is<IContactRepository>(value => value == _contactRepositoryMock.Object)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndNoExistingContactWasReturnedFromCommand_AssertInternalIdentifierWasNotCalledOnExistingContactFromCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IContact> existingContactMock = _fixture.BuildContactMock();
            IUpdateContactCommand command = CreateUpdateContactCommandMock(hasExistingContact: false, existingContact: existingContactMock.Object).Object;
            await sut.ExecuteAsync(command);

            existingContactMock.Verify(m => m.InternalIdentifier, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndNoExistingContactWasReturnedFromCommand_AssertInternalIdentifierSetterWasNotCalledOnContactFromToDomainOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IContact> toDomainContactMock = _fixture.BuildContactMock();
            IUpdateContactCommand command = CreateUpdateContactCommandMock(toDomainContact: toDomainContactMock.Object, hasExistingContact: false).Object;
            await sut.ExecuteAsync(command);

            toDomainContactMock.VerifySet(m => m.InternalIdentifier = It.IsAny<string>(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndNoExistingContactWasReturnedFromCommand_AssertUpdateContactAsyncWasNotCalledOnMicrosoftGraphRepository()
        {
            CommandHandler sut = CreateSut();

            IUpdateContactCommand command = CreateUpdateContactCommandMock(hasExistingContact: false).Object;
            await sut.ExecuteAsync(command);

            _microsoftGraphRepositoryMock.Verify(m => m.UpdateContactAsync(It.IsAny<IRefreshableToken>(), It.IsAny<IContact>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndNoExistingContactWasReturnedFromCommand_AssertExternalIdentifierWasNotCalledOnUpdatedContactFromMicrosoftGraphRepository()
        {
            Mock<IContact> updatedMicrosoftGraphContactMock = _fixture.BuildContactMock();
            CommandHandler sut = CreateSut(updatedMicrosoftGraphContact: updatedMicrosoftGraphContactMock.Object);

            IUpdateContactCommand command = CreateUpdateContactCommandMock(hasExistingContact: false).Object;
            await sut.ExecuteAsync(command);

            updatedMicrosoftGraphContactMock.Verify(m => m.ExternalIdentifier, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndNoExistingContactWasReturnedFromCommand_AssertExternalIdentifierSetterWasNotCalledOnContactFromToDomainOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IContact> toDomainContactMock = _fixture.BuildContactMock();
            IUpdateContactCommand command = CreateUpdateContactCommandMock(toDomainContact: toDomainContactMock.Object, hasExistingContact: false).Object;
            await sut.ExecuteAsync(command);

            toDomainContactMock.VerifySet(m => m.ExternalIdentifier = It.IsAny<string>(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndNoExistingContactWasReturnedFromCommand_AssertExternalIdentifierWasNotCalledOnExistingContactFromCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IContact> existingContactMock = _fixture.BuildContactMock();
            IUpdateContactCommand command = CreateUpdateContactCommandMock(hasExistingContact: false, existingContact: existingContactMock.Object).Object;
            await sut.ExecuteAsync(command);

            existingContactMock.Verify(m => m.ExternalIdentifier, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndNoExistingContactWasReturnedFromCommand_AssertCreateOrUpdateContactSupplementAsyncWasNotCalledOnContactRepository()
        {
            CommandHandler sut = CreateSut();

            IUpdateContactCommand command = CreateUpdateContactCommandMock(hasExistingContact: false).Object;
            await sut.ExecuteAsync(command);

            _contactRepositoryMock.Verify(m => m.CreateOrUpdateContactSupplementAsync(It.IsAny<IContact>(), It.IsAny<string>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndExistingContactWasReturnedFromCommand_AssertInternalIdentifierWasCalledOnExistingContactFromCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IContact> existingContactMock = _fixture.BuildContactMock();
            IUpdateContactCommand command = CreateUpdateContactCommandMock(existingContact: existingContactMock.Object).Object;
            await sut.ExecuteAsync(command);

            existingContactMock.Verify(m => m.InternalIdentifier, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndExistingContactWithoutInternalIdentifierWasReturnedFromCommand_AssertInternalIdentifierSetterWasNotCalledOnContactFromToDomainOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IContact> toDomainContactMock = _fixture.BuildContactMock();
            IContact existingContact = _fixture.BuildContactMock(hasInternalIdentifier: false).Object;
            IUpdateContactCommand command = CreateUpdateContactCommandMock(toDomainContact: toDomainContactMock.Object, existingContact: existingContact).Object;
            await sut.ExecuteAsync(command);

            toDomainContactMock.VerifySet(m => m.InternalIdentifier = It.IsAny<string>(), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndExistingContactWithInternalIdentifierWasReturnedFromCommand_AssertInternalIdentifierSetterWasCalledOnContactFromToDomainOnCommandWithInternalIdentifierFromExistingContactFromCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IContact> toDomainContactMock = _fixture.BuildContactMock();
            string internalIdentifier = _fixture.Create<string>();
            IContact existingContact = _fixture.BuildContactMock(internalIdentifier: internalIdentifier).Object;
            IUpdateContactCommand command = CreateUpdateContactCommandMock(toDomainContact: toDomainContactMock.Object, existingContact: existingContact).Object;
            await sut.ExecuteAsync(command);

            toDomainContactMock.VerifySet(m => m.InternalIdentifier = It.Is<string>(value => string.CompareOrdinal(value, internalIdentifier) == 0), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndExistingContactWasReturnedFromCommand_AssertUpdateContactAsyncWasCalledOnMicrosoftGraphRepositoryWithContactFromToDomainOnCommand()
        {
            CommandHandler sut = CreateSut();

            IContact toDomainContact = _fixture.BuildContactMock().Object;
            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock().Object;
            IUpdateContactCommand command = CreateUpdateContactCommandMock(toDomainContact: toDomainContact, refreshableToken: refreshableToken).Object;
            await sut.ExecuteAsync(command);

            _microsoftGraphRepositoryMock.Verify(m => m.UpdateContactAsync(
                    It.Is<IRefreshableToken>(value => value == refreshableToken),
                    It.Is<IContact>(value => value == toDomainContact)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndExistingContactWasReturnedFromCommandAndNoUpdatedContactWasReturnedFromMicrosoftGraphRepository_AssertExternalIdentifierWasNotCalledOnUpdatedContactFromMicrosoftGraphRepository()
        {
            Mock<IContact> updatedMicrosoftGraphContactMock = _fixture.BuildContactMock();
            CommandHandler sut = CreateSut(false, updatedMicrosoftGraphContact: updatedMicrosoftGraphContactMock.Object);

            IUpdateContactCommand command = CreateUpdateContactCommandMock().Object;
            await sut.ExecuteAsync(command);

            updatedMicrosoftGraphContactMock.Verify(m => m.ExternalIdentifier, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndExistingContactWasReturnedFromCommandAndNoUpdatedContactWasReturnedFromMicrosoftGraphRepository_AssertExternalIdentifierSetterWasNotCalledOnContactFromToDomainOnCommand()
        {
            CommandHandler sut = CreateSut(false);

            Mock<IContact> toDomainContactMock = _fixture.BuildContactMock();
            IUpdateContactCommand command = CreateUpdateContactCommandMock(toDomainContact: toDomainContactMock.Object).Object;
            await sut.ExecuteAsync(command);

            toDomainContactMock.VerifySet(m => m.ExternalIdentifier = It.IsAny<string>(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndExistingContactWasReturnedFromCommandAndNoUpdatedContactWasReturnedFromMicrosoftGraphRepository_AssertExternalIdentifierWasNotCalledOnExistingContactFromCommand()
        {
            CommandHandler sut = CreateSut(false);

            Mock<IContact> existingContactMock = _fixture.BuildContactMock();
            IUpdateContactCommand command = CreateUpdateContactCommandMock(existingContact: existingContactMock.Object).Object;
            await sut.ExecuteAsync(command);

            existingContactMock.Verify(m => m.ExternalIdentifier, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndExistingContactWasReturnedFromCommandAndNoUpdatedContactWasReturnedFromMicrosoftGraphRepository_AssertCreateOrUpdateContactSupplementAsyncWasNotCalledOnContactRepository()
        {
            CommandHandler sut = CreateSut(false);

            IUpdateContactCommand command = CreateUpdateContactCommandMock().Object;
            await sut.ExecuteAsync(command);

            _contactRepositoryMock.Verify(m => m.CreateOrUpdateContactSupplementAsync(It.IsAny<IContact>(), It.IsAny<string>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndExistingContactWasReturnedFromCommandAndUpdatedContactWasReturnedFromMicrosoftGraphRepository_AssertExternalIdentifierWasCalledOnUpdatedContactFromMicrosoftGraphRepository()
        {
            Mock<IContact> updatedMicrosoftGraphContactMock = _fixture.BuildContactMock();
            CommandHandler sut = CreateSut(updatedMicrosoftGraphContact: updatedMicrosoftGraphContactMock.Object);

            IUpdateContactCommand command = CreateUpdateContactCommandMock().Object;
            await sut.ExecuteAsync(command);

            updatedMicrosoftGraphContactMock.Verify(m => m.ExternalIdentifier, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndExistingContactWasReturnedFromCommandAndUpdatedContactWithoutExternalIdentifierWasReturnedFromMicrosoftGraphRepository_AssertExternalIdentifierSetterWasNotCalledOnContactFromToDomainOnCommand()
        {
            IContact updatedMicrosoftGraphContact = _fixture.BuildContactMock(hasExternalIdentifier: false).Object;
            CommandHandler sut = CreateSut(updatedMicrosoftGraphContact: updatedMicrosoftGraphContact);

            Mock<IContact> toDomainContactMock = _fixture.BuildContactMock();
            IUpdateContactCommand command = CreateUpdateContactCommandMock(toDomainContact: toDomainContactMock.Object).Object;
            await sut.ExecuteAsync(command);

            toDomainContactMock.VerifySet(m => m.ExternalIdentifier = It.IsAny<string>(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndExistingContactWasReturnedFromCommandAndUpdatedContactWithoutExternalIdentifierWasReturnedFromMicrosoftGraphRepository_AssertExternalIdentifierWasNotCalledOnExistingContactFromCommand()
        {
            IContact updatedMicrosoftGraphContact = _fixture.BuildContactMock(hasExternalIdentifier: false).Object;
            CommandHandler sut = CreateSut(updatedMicrosoftGraphContact: updatedMicrosoftGraphContact);

            Mock<IContact> existingContactMock = _fixture.BuildContactMock();
            IUpdateContactCommand command = CreateUpdateContactCommandMock(existingContact: existingContactMock.Object).Object;
            await sut.ExecuteAsync(command);

            existingContactMock.Verify(m => m.ExternalIdentifier, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndExistingContactWasReturnedFromCommandAndUpdatedContactWithoutExternalIdentifierWasReturnedFromMicrosoftGraphRepository_AssertCreateOrUpdateContactSupplementAsyncWasNotCalledOnContactRepository()
        {
            IContact updatedMicrosoftGraphContact = _fixture.BuildContactMock(hasExternalIdentifier: false).Object;
            CommandHandler sut = CreateSut(updatedMicrosoftGraphContact: updatedMicrosoftGraphContact);

            IUpdateContactCommand command = CreateUpdateContactCommandMock().Object;
            await sut.ExecuteAsync(command);

            _contactRepositoryMock.Verify(m => m.CreateOrUpdateContactSupplementAsync(It.IsAny<IContact>(), It.IsAny<string>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndExistingContactWasReturnedFromCommandAndUpdatedContactWithExternalIdentifierWasReturnedFromMicrosoftGraphRepository_AssertExternalIdentifierSetterWasCalledOnContactFromToDomainOnCommandWithExternalIdentifierFromUpdatedContractFormMicrosoftGraphRepository()
        {
            string externalIdentifier = _fixture.Create<string>();
            IContact updatedMicrosoftGraphContact = _fixture.BuildContactMock(externalIdentifier: externalIdentifier).Object;
            CommandHandler sut = CreateSut(updatedMicrosoftGraphContact: updatedMicrosoftGraphContact);

            Mock<IContact> toDomainContactMock = _fixture.BuildContactMock();
            IUpdateContactCommand command = CreateUpdateContactCommandMock(toDomainContact: toDomainContactMock.Object).Object;
            await sut.ExecuteAsync(command);

            toDomainContactMock.VerifySet(m => m.ExternalIdentifier = It.Is<string>(value => string.CompareOrdinal(value, externalIdentifier) == 0), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndExistingContactWasReturnedFromCommandAndUpdatedContactWithExternalIdentifierWasReturnedFromMicrosoftGraphRepository_AssertExternalIdentifierWasCalledOnExistingContactFromCommand()
        {
            IContact updatedMicrosoftGraphContact = _fixture.BuildContactMock().Object;
            CommandHandler sut = CreateSut(updatedMicrosoftGraphContact: updatedMicrosoftGraphContact);

            Mock<IContact> existingContactMock = _fixture.BuildContactMock();
            IUpdateContactCommand command = CreateUpdateContactCommandMock(existingContact: existingContactMock.Object).Object;
            await sut.ExecuteAsync(command);

            existingContactMock.Verify(m => m.ExternalIdentifier, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifierAndExistingContactWasReturnedFromCommandAndUpdatedContactWithExternalIdentifierWasReturnedFromMicrosoftGraphRepository_AssertCreateOrUpdateContactSupplementAsyncWasCalledOnContactRepositoryWithContactFromToDomainOnCommandAndWithExternalIdentifierFromExistingContactFromCommand()
        {
            CommandHandler sut = CreateSut();

            IContact toDomainContact = _fixture.BuildContactMock().Object;
            string externalIdentifier = _fixture.Create<string>();
            IContact existingContact = _fixture.BuildContactMock(externalIdentifier: externalIdentifier).Object;
            IUpdateContactCommand command = CreateUpdateContactCommandMock(toDomainContact: toDomainContact, existingContact: existingContact).Object;
            await sut.ExecuteAsync(command);

            _contactRepositoryMock.Verify(m => m.CreateOrUpdateContactSupplementAsync(
                    It.Is<IContact>(value => value == toDomainContact),
                    It.Is<string>(value => string.CompareOrdinal(value, externalIdentifier) == 0)),
                Times.Once());
        }

        private CommandHandler CreateSut(bool hasUpdatedMicrosoftGraphContact = true, IContact updatedMicrosoftGraphContact = null)
        {
            _microsoftGraphRepositoryMock.Setup(m => m.UpdateContactAsync(It.IsAny<IRefreshableToken>(), It.IsAny<IContact>()))
                .Returns(Task.Run(() => hasUpdatedMicrosoftGraphContact ? updatedMicrosoftGraphContact ?? _fixture.BuildContactMock().Object : null));
            _contactRepositoryMock.Setup(m => m.CreateOrUpdateContactSupplementAsync(It.IsAny<IContact>(), It.IsAny<string>()))
                .Returns(Task.Run(() => _fixture.BuildContactMock().Object));

            return new CommandHandler(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);
        }

        private Mock<IUpdateContactCommand> CreateUpdateContactCommandMock(bool hasExternalIdentifier = true, IContact toDomainContact = null, bool hasExistingContact = true, IContact existingContact = null, IRefreshableToken refreshableToken = null)
        {
            Mock<IUpdateContactCommand> updateContactCommandMock = new Mock<IUpdateContactCommand>();
            updateContactCommandMock.Setup(m => m.ExternalIdentifier)
                .Returns(hasExternalIdentifier ? _fixture.Create<string>() : null);
            updateContactCommandMock.Setup(m => m.ToDomain(It.IsAny<IContactRepository>(), It.IsAny<IAccountingRepository>()))
                .Returns(toDomainContact ?? _fixture.BuildContactMock().Object);
            updateContactCommandMock.Setup(m => m.GetExistingContactAsync(It.IsAny<IMicrosoftGraphRepository>(), It.IsAny<IContactRepository>()))
                .Returns(Task.Run(() => hasExistingContact ? existingContact ?? _fixture.BuildContactMock().Object : null));
            updateContactCommandMock.Setup(m => m.ToToken())
                .Returns(refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object);
            return updateContactCommandMock;
        }
    }
}