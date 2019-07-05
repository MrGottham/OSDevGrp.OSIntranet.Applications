using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers.DeleteCountryCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.CommandHandlers.DeleteCountryCommandHandler
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
        public async Task ExecuteAsync_WhenCalled_AssertCountryCodeWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IDeleteCountryCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.CountryCode, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertDeleteCountryAsyncWasCalledOnContactRepository()
        {
            CommandHandler sut = CreateSut();

            string countryCode = _fixture.Create<string>();
            IDeleteCountryCommand command = CreateCommandMock(countryCode).Object;
            await sut.ExecuteAsync(command);

            _contactRepositoryMock.Verify(m => m.DeleteCountryAsync(It.Is<string>(value => string.CompareOrdinal(value, countryCode) == 0)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _contactRepositoryMock.Setup(m => m.DeleteCountryAsync(It.IsAny<string>()))
                .Returns(Task.Run(() => (ICountry) null));

            return new CommandHandler(_validatorMock.Object, _contactRepositoryMock.Object);
        }

        private Mock<IDeleteCountryCommand> CreateCommandMock(string countryCode = null)
        {
            Mock<IDeleteCountryCommand> commandMock = new Mock<IDeleteCountryCommand>();
            commandMock.Setup(m => m.CountryCode)
                .Returns(countryCode ?? _fixture.Create<string>());
            return commandMock;
        }
    }
}
