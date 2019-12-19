using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers.CreateContactGroupCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.CommandHandlers.CreateContactGroupCommandHandler
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

            Mock<ICreateContactGroupCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertCreateContactGroupAsyncWasCalledOnContactRepository()
        {
            CommandHandler sut = CreateSut();

            IContactGroup contactGroup = _fixture.BuildContactGroupMock().Object;
            ICreateContactGroupCommand command = CreateCommandMock(contactGroup).Object;
            await sut.ExecuteAsync(command);

            _contactRepositoryMock.Verify(m => m.CreateContactGroupAsync(It.Is<IContactGroup>(value => value == contactGroup)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _contactRepositoryMock.Setup(m => m.CreateContactGroupAsync(It.IsAny<IContactGroup>()))
                .Returns(Task.Run(() => _fixture.BuildContactGroupMock().Object));

            return new CommandHandler(_validatorMock.Object, _contactRepositoryMock.Object);
        }

        private Mock<ICreateContactGroupCommand> CreateCommandMock(IContactGroup contactGroup = null)
        {
            Mock<ICreateContactGroupCommand> commandMock = new Mock<ICreateContactGroupCommand>();
            commandMock.Setup(m => m.ToDomain())
                .Returns(contactGroup ?? _fixture.BuildContactGroupMock().Object);
            return commandMock;
        }
    }
}
