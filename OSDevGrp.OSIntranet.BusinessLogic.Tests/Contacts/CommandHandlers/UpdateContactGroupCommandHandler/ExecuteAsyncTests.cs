using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers.UpdateContactGroupCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.CommandHandlers.UpdateContactGroupCommandHandler
{
    [TestFixture]
    public class ExecuteAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IContactRepository> _contactRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _contactRepositoryMock = new Mock<IContactRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IUpdateContactGroupCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertUpdateContactGroupAsyncWasCalledOnContactRepository()
        {
            CommandHandler sut = CreateSut();

            IContactGroup contactGroup = _fixture.BuildContactGroupMock().Object;
            IUpdateContactGroupCommand command = CreateCommandMock(contactGroup).Object;
            await sut.ExecuteAsync(command);

            _contactRepositoryMock.Verify(m => m.UpdateContactGroupAsync(It.Is<IContactGroup>(value => value == contactGroup)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _contactRepositoryMock.Setup(m => m.UpdateContactGroupAsync(It.IsAny<IContactGroup>()))
                .Returns(Task.Run(() => _fixture.BuildContactGroupMock().Object));

            return new CommandHandler(_validatorMock.Object, _contactRepositoryMock.Object);
        }

        private Mock<IUpdateContactGroupCommand> CreateCommandMock(IContactGroup contactGroup = null)
        {
            Mock<IUpdateContactGroupCommand> commandMock = new Mock<IUpdateContactGroupCommand>();
            commandMock.Setup(m => m.ToDomain())
                .Returns(contactGroup ?? _fixture.BuildContactGroupMock().Object);
            return commandMock;
        }
    }
}
