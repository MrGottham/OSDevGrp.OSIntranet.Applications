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
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers.CreateContactCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.CommandHandlers.CreateContactCommandHandler
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
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<ICreateContactCommand> commandMock = CreateCreateContactCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(
                    It.Is<IContactRepository>(value => value == _contactRepositoryMock.Object),
                    It.Is<IAccountingRepository>(value => value == _accountingRepositoryMock.Object)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertCreateContactAsyncWasCalledOnMicrosoftGraphRepository()
        {
            CommandHandler sut = CreateSut();

            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock().Object;
            IContact contact = _fixture.BuildContactMock().Object;
            ICreateContactCommand command = CreateCreateContactCommandMock(refreshableToken, contact).Object;
            await sut.ExecuteAsync(command);

            _microsoftGraphRepositoryMock.Verify(m => m.CreateContactAsync(
                    It.Is<IRefreshableToken>(value => value == refreshableToken),
                    It.Is<IContact>(value => value == contact)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndNoCreatedContactWasReturnedFromMicrosoftGraphRepository_AssertExternalIdentifierWasNotCalledOnCreatedContactFromMicrosoftGraphRepository()
        {
            Mock<IContact> createdMicrosoftGraphContactMock = _fixture.BuildContactMock();
            CommandHandler sut = CreateSut(false, createdMicrosoftGraphContactMock.Object);

            ICreateContactCommand command = CreateCreateContactCommandMock().Object;
            await sut.ExecuteAsync(command);

            createdMicrosoftGraphContactMock.Verify(m => m.ExternalIdentifier, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndNoCreatedContactWasReturnedFromMicrosoftGraphRepository_AssertExternalIdentifierSetterWasNotCalledOnContactFromToDomainOnCommand()
        {
            CommandHandler sut = CreateSut(false);

            Mock<IContact> contactMock = _fixture.BuildContactMock();
            ICreateContactCommand command = CreateCreateContactCommandMock(contact: contactMock.Object).Object;
            await sut.ExecuteAsync(command);

            contactMock.VerifySet(m => m.ExternalIdentifier = It.IsAny<string>(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndNoCreatedContactWasReturnedFromMicrosoftGraphRepository_AssertCreateOrUpdateContactSupplementAsyncWasNotCalledOnContactRepository()
        {
            CommandHandler sut = CreateSut(false);

            ICreateContactCommand command = CreateCreateContactCommandMock().Object;
            await sut.ExecuteAsync(command);

            _contactRepositoryMock.Verify(m => m.CreateOrUpdateContactSupplementAsync(It.IsAny<IContact>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCreatedContactWasReturnedFromMicrosoftGraphRepository_AssertExternalIdentifierWasCalledOnCreatedContactFromMicrosoftGraphRepository()
        {
            Mock<IContact> createdMicrosoftGraphContactMock = _fixture.BuildContactMock();
            CommandHandler sut = CreateSut(createdMicrosoftGraphContact: createdMicrosoftGraphContactMock.Object);

            ICreateContactCommand command = CreateCreateContactCommandMock().Object;
            await sut.ExecuteAsync(command);

            createdMicrosoftGraphContactMock.Verify(m => m.ExternalIdentifier, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCreatedContactWasReturnedFromMicrosoftGraphRepository_AssertExternalIdentifierSetterWasCalledOnContactFromToDomainOnCommandExternalIdentifierFromCreatedContactFromMicrosoftGraphRepository()
        {
            string externalIdentifier = _fixture.Create<string>();
            IContact createdMicrosoftGraphContact = _fixture.BuildContactMock(externalIdentifier: externalIdentifier).Object;
            CommandHandler sut = CreateSut(createdMicrosoftGraphContact: createdMicrosoftGraphContact);

            Mock<IContact> contactMock = _fixture.BuildContactMock();
            ICreateContactCommand command = CreateCreateContactCommandMock(contact: contactMock.Object).Object;
            await sut.ExecuteAsync(command);

            contactMock.VerifySet(m => m.ExternalIdentifier = It.Is<string>(value => string.CompareOrdinal(value, externalIdentifier) == 0), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCreatedContactWasReturnedFromMicrosoftGraphRepository_AssertCreateOrUpdateContactSupplementAsyncWasCalledOnContactRepositoryWithContactFromToDomainOnCommand()
        {
            CommandHandler sut = CreateSut();

            IContact contact = _fixture.BuildContactMock().Object;
            ICreateContactCommand command = CreateCreateContactCommandMock(contact: contact).Object;
            await sut.ExecuteAsync(command);

            _contactRepositoryMock.Verify(m => m.CreateOrUpdateContactSupplementAsync(It.Is<IContact>(value => value == contact), It.Is<string>(value => value == null)), Times.Once);
        }

        private CommandHandler CreateSut(bool hasCreatedMicrosoftGraphContact = true, IContact createdMicrosoftGraphContact = null)
        {
            _microsoftGraphRepositoryMock.Setup(m => m.CreateContactAsync(It.IsAny<IRefreshableToken>(), It.IsAny<IContact>()))
                .Returns(Task.Run(() => hasCreatedMicrosoftGraphContact ? createdMicrosoftGraphContact ?? _fixture.BuildContactMock().Object : null));
            _contactRepositoryMock.Setup(m => m.CreateOrUpdateContactSupplementAsync(It.IsAny<IContact>(), It.IsAny<string>()))
                .Returns(Task.Run(() => _fixture.BuildContactMock().Object));

            return new CommandHandler(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);
        }

        private Mock<ICreateContactCommand> CreateCreateContactCommandMock(IRefreshableToken refreshableToken = null, IContact contact = null)
        {
            Mock<ICreateContactCommand> createContactCommandMock = new Mock<ICreateContactCommand>();
            createContactCommandMock.Setup(m => m.ToToken())
                .Returns(refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object);
            createContactCommandMock.Setup(m => m.ToDomain(It.IsAny<IContactRepository>(), It.IsAny<IAccountingRepository>()))
                .Returns(contact ?? _fixture.BuildContactMock().Object);
            return createContactCommandMock;
        }
    }
}