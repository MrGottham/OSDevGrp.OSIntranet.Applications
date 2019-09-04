using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers.DeletePostalCodeCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.CommandHandlers.DeletePostalCodeCommandHandler
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

            Mock<IDeletePostalCodeCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.CountryCode, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertPostalCodeWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IDeletePostalCodeCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.PostalCode, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertDeletePostalCodeAsyncWasCalledOnContactRepository()
        {
            CommandHandler sut = CreateSut();

            string countryCode = _fixture.Create<string>();
            string postalCode = _fixture.Create<string>();
            IDeletePostalCodeCommand command = CreateCommandMock(countryCode, postalCode).Object;
            await sut.ExecuteAsync(command);

            _contactRepositoryMock.Verify(m => m.DeletePostalCodeAsync(
                    It.Is<string>(value => string.CompareOrdinal(value, countryCode) == 0),
                    It.Is<string>(value => string.CompareOrdinal(value, postalCode) == 0)),
                Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _contactRepositoryMock.Setup(m => m.DeletePostalCodeAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.Run(() => _fixture.BuildPostalCodeMock().Object));

            return new CommandHandler(_validatorMock.Object, _contactRepositoryMock.Object);
        }

        private Mock<IDeletePostalCodeCommand> CreateCommandMock(string countryCode = null, string postalCode = null)
        {
            Mock<IDeletePostalCodeCommand> commandMock = new Mock<IDeletePostalCodeCommand>();
            commandMock.Setup(m => m.CountryCode)
                .Returns(countryCode ?? _fixture.Create<string>());
            commandMock.Setup(m => m.PostalCode)
                .Returns(postalCode ?? _fixture.Create<string>());
            return commandMock;
        }
    }
}
