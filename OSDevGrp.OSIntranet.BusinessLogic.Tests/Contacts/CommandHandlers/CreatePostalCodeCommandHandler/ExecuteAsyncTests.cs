using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers.CreatePostalCodeCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.CommandHandlers.CreatePostalCodeCommandHandler
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

            Mock<ICreatePostalCodeCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(It.Is<IContactRepository>(value => value == _contactRepositoryMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertCreatePostalCodeAsyncWasCalledOnContactRepository()
        {
            CommandHandler sut = CreateSut();

            IPostalCode postalCode = _fixture.BuildPostalCodeMock().Object;
            ICreatePostalCodeCommand command = CreateCommandMock(postalCode).Object;
            await sut.ExecuteAsync(command);

            _contactRepositoryMock.Verify(m => m.CreatePostalCodeAsync(It.Is<IPostalCode>(value => value == postalCode)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _contactRepositoryMock.Setup(m => m.CreatePostalCodeAsync(It.IsAny<IPostalCode>()))
                .Returns(Task.Run(() => _fixture.BuildPostalCodeMock().Object));

            return new CommandHandler(_validatorMock.Object, _contactRepositoryMock.Object);
        }

        private Mock<ICreatePostalCodeCommand> CreateCommandMock(IPostalCode postalCode = null)
        {
            Mock<ICreatePostalCodeCommand> commandMock = new Mock<ICreatePostalCodeCommand>();
            commandMock.Setup(m => m.ToDomain(It.IsAny<IContactRepository>()))
                .Returns(postalCode ?? _fixture.BuildPostalCodeMock().Object);
            return commandMock;
        }
    }
}
