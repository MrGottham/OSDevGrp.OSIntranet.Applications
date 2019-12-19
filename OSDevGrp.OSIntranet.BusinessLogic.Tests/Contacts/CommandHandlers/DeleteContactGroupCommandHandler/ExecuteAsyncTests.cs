using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers.DeleteContactGroupCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.CommandHandlers.DeleteContactGroupCommandHandler
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
        public async Task ExecuteAsync_WhenCalled_AssertNumberWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IDeleteContactGroupCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertDeleteContactGroupAsyncWasCalledOnContactRepository()
        {
            CommandHandler sut = CreateSut();

            int number = _fixture.Create<int>();
            IDeleteContactGroupCommand command = CreateCommandMock(number).Object;
            await sut.ExecuteAsync(command);

            _contactRepositoryMock.Verify(m => m.DeleteContactGroupAsync(It.Is<int>(value => value == number)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _contactRepositoryMock.Setup(m => m.DeleteContactGroupAsync(It.IsAny<int>()))
                .Returns(Task.Run(() => (IContactGroup) null));

            return new CommandHandler(_validatorMock.Object, _contactRepositoryMock.Object);
        }

        private Mock<IDeleteContactGroupCommand> CreateCommandMock(int? number = null)
        {
            Mock<IDeleteContactGroupCommand> commandMock = new Mock<IDeleteContactGroupCommand>();
            commandMock.Setup(m => m.Number)
                .Returns(number ?? _fixture.Create<int>());
            return commandMock;
        }
    }
}
