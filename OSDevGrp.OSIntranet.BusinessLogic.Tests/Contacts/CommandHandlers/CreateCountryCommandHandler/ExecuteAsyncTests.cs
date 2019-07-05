using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers.CreateCountryCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.CommandHandlers.CreateCountryCommandHandler
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

            Mock<ICreateCountryCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertCreateCountryAsyncWasCalledOnContactRepository()
        {
            CommandHandler sut = CreateSut();

            ICountry country = _fixture.BuildCountryMock().Object;
            ICreateCountryCommand command = CreateCommandMock(country).Object;
            await sut.ExecuteAsync(command);

            _contactRepositoryMock.Verify(m => m.CreateCountryAsync(It.Is<ICountry>(value => value == country)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _contactRepositoryMock.Setup(m => m.CreateCountryAsync(It.IsAny<ICountry>()))
                .Returns(Task.Run(() => _fixture.BuildCountryMock().Object));

            return new CommandHandler(_validatorMock.Object, _contactRepositoryMock.Object);
        }

        private Mock<ICreateCountryCommand> CreateCommandMock(ICountry country = null)
        {
            Mock<ICreateCountryCommand> commandMock = new Mock<ICreateCountryCommand>();
            commandMock.Setup(m => m.ToDomain())
                .Returns(country ?? _fixture.BuildCountryMock().Object);
            return commandMock;
        }
    }
}
