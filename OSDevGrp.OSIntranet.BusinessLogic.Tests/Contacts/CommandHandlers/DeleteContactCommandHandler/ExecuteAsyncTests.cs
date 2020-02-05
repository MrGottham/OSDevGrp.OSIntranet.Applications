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
        public async Task ExecuteAsync_WhenCalledAndCommandDoesNotHaveExternalIdentifier_AssertDeleteContactSupplementAsyncWasNotCalledOnContactRepository()
        {
            CommandHandler sut = CreateSut();

            IDeleteContactCommand command = CreateDeleteContactCommandMock(hasExternalIdentifier: false).Object;
            await sut.ExecuteAsync(command);

            _contactRepositoryMock.Verify(m => m.DeleteContactSupplementAsync(It.IsAny<string>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandDoesNotHaveExternalIdentifier_AssertDeleteContactAsyncWasNotCalledOnMicrosoftGraphRepository()
        {
            CommandHandler sut = CreateSut();

            IDeleteContactCommand command = CreateDeleteContactCommandMock(hasExternalIdentifier: false).Object;
            await sut.ExecuteAsync(command);

            _microsoftGraphRepositoryMock.Verify(m => m.DeleteContactAsync(
                    It.IsAny<IRefreshableToken>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifier_AssertDeleteContactSupplementAsyncWasCalledOnContactRepository()
        {
            CommandHandler sut = CreateSut();

            string externalIdentifier = _fixture.Create<string>();
            IDeleteContactCommand command = CreateDeleteContactCommandMock(externalIdentifier: externalIdentifier).Object;
            await sut.ExecuteAsync(command);

            _contactRepositoryMock.Verify(m => m.DeleteContactSupplementAsync(It.Is<string>(value => string.CompareOrdinal(value, externalIdentifier) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalledAndCommandHasExternalIdentifier_AssertDeleteContactAsyncWasCalledOnMicrosoftGraphRepository()
        {
            CommandHandler sut = CreateSut();

            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock().Object;
            string externalIdentifier = _fixture.Create<string>();
            IDeleteContactCommand command = CreateDeleteContactCommandMock(refreshableToken, externalIdentifier: externalIdentifier).Object;
            await sut.ExecuteAsync(command);

            _microsoftGraphRepositoryMock.Verify(m => m.DeleteContactAsync(
                    It.Is<IRefreshableToken>(value => value == refreshableToken),
                    It.Is<string>(value => string.CompareOrdinal(value, externalIdentifier) == 0)),
                Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _contactRepositoryMock.Setup(m => m.DeleteContactSupplementAsync(It.IsAny<string>()))
                .Returns(Task.Run(() => (IContact) null));
            _microsoftGraphRepositoryMock.Setup(m => m.DeleteContactAsync(It.IsAny<IRefreshableToken>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            return new CommandHandler(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);
        }

        private Mock<IDeleteContactCommand> CreateDeleteContactCommandMock(IRefreshableToken refreshableToken = null, bool hasExternalIdentifier = true, string externalIdentifier = null)
        {
            Mock<IDeleteContactCommand> deleteContactCommandMock = new Mock<IDeleteContactCommand>();
            deleteContactCommandMock.Setup(m => m.ToToken())
                .Returns(refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object);
            deleteContactCommandMock.Setup(m => m.ExternalIdentifier)
                .Returns(hasExternalIdentifier ? externalIdentifier ?? _fixture.Create<string>() : null);
            return deleteContactCommandMock;
        }
    }
}