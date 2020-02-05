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
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnCommand()
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
        public async Task ExecuteAsync_WhenCalled_AssertUpdateContactAsyncWasCalledOnMicrosoftGraphRepository()
        {
            CommandHandler sut = CreateSut();

            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock().Object;
            IContact contact = _fixture.BuildContactMock().Object;
            IUpdateContactCommand command = CreateUpdateContactCommandMock(refreshableToken, contact).Object;
            await sut.ExecuteAsync(command);

            _microsoftGraphRepositoryMock.Verify(m => m.UpdateContactAsync(
                    It.Is<IRefreshableToken>(value => value == refreshableToken),
                    It.Is<IContact>(value => value == contact)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndNoUpdatedContactWasReturnedFromMicrosoftGraphRepository_AssertCreateOrUpdateContactSupplementAsyncWasNotCalledOnContactRepository()
        {
            CommandHandler sut = CreateSut(false);

            IUpdateContactCommand command = CreateUpdateContactCommandMock().Object;
            await sut.ExecuteAsync(command);

            _contactRepositoryMock.Verify(m => m.CreateOrUpdateContactSupplementAsync(It.IsAny<IContact>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndUpdatedContactWasReturnedFromMicrosoftGraphRepository_AssertCreateOrUpdateContactSupplementAsyncWasCalledOnContactRepositoryWithUpdatedContactFromMicrosoftGraphRepository()
        {
            IContact updatedMicrosoftGraphContact = _fixture.BuildContactMock().Object;
            CommandHandler sut = CreateSut(updatedMicrosoftGraphContact: updatedMicrosoftGraphContact);

            IUpdateContactCommand command = CreateUpdateContactCommandMock().Object;
            await sut.ExecuteAsync(command);

            _contactRepositoryMock.Verify(m => m.CreateOrUpdateContactSupplementAsync(It.Is<IContact>(value => value == updatedMicrosoftGraphContact)), Times.Once);
        }

        private CommandHandler CreateSut(bool hasUpdatedMicrosoftGraphContact = true, IContact updatedMicrosoftGraphContact = null)
        {
            _microsoftGraphRepositoryMock.Setup(m => m.UpdateContactAsync(It.IsAny<IRefreshableToken>(), It.IsAny<IContact>()))
                .Returns(Task.Run(() => hasUpdatedMicrosoftGraphContact ? updatedMicrosoftGraphContact ?? _fixture.BuildContactMock().Object : null));
            _contactRepositoryMock.Setup(m => m.CreateOrUpdateContactSupplementAsync(It.IsAny<IContact>()))
                .Returns(Task.Run(() => _fixture.BuildContactMock().Object));

            return new CommandHandler(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);
        }

        private Mock<IUpdateContactCommand> CreateUpdateContactCommandMock(IRefreshableToken refreshableToken = null, IContact contact = null)
        {
            Mock<IUpdateContactCommand> updateContactCommandMock = new Mock<IUpdateContactCommand>();
            updateContactCommandMock.Setup(m => m.ToToken())
                .Returns(refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object);
            updateContactCommandMock.Setup(m => m.ToDomain(It.IsAny<IContactRepository>(), It.IsAny<IAccountingRepository>()))
                .Returns(contact ?? _fixture.BuildContactMock().Object);
            return updateContactCommandMock;
        }
    }
}