using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers.CountryIdentificationCommandHandlerBase<OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands.ICountryIdentificationCommand>;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.CommandHandlers.CountryIdentificationCommandHandlerBase
{
    [TestFixture]
    public class ExecuteAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IContactRepository> _contactRepositoryMock;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _contactRepositoryMock = new Mock<IContactRepository>();
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
        public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<ICountryIdentificationCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.Validate(It.Is<IValidator>(value => value == _validatorMock.Object), It.Is<IContactRepository>(value => value == _contactRepositoryMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertManageRepositoryAsyncWasCalledOnCommandHandler()
        {
            CommandHandler sut = CreateSut();

            ICountryIdentificationCommand command = CreateCommandMock().Object;
            await sut.ExecuteAsync(command);

            Assert.That(((Sut) sut).ManageRepositoryAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertManageRepositoryAsyncWasCalledWithSameCommand()
        {
            CommandHandler sut = CreateSut();

            ICountryIdentificationCommand command = CreateCommandMock().Object;
            await sut.ExecuteAsync(command);

            Assert.That(((Sut) sut).Command, Is.EqualTo(command));
        }

        private CommandHandler CreateSut()
        {
            return new Sut(_validatorMock.Object, _contactRepositoryMock.Object);
        }

        private Mock<ICountryIdentificationCommand> CreateCommandMock()
        {
            return new Mock<ICountryIdentificationCommand>();
        }

        private class Sut : CommandHandler
        {
            #region Constructor

            public Sut(IValidator validator, IContactRepository contactRepository)
                : base(validator, contactRepository)
            {
            }

            #endregion

            #region Properties

            public bool ManageRepositoryAsyncWasCalled { get; private set; }

            public ICountryIdentificationCommand Command { get; private set; }

            #endregion

            #region Methods

            protected override Task ManageRepositoryAsync(ICountryIdentificationCommand command)
            {
                NullGuard.NotNull(command, nameof(command));

                return Task.Run(() =>
                {
                    ManageRepositoryAsyncWasCalled = true;
                    Command = command;
                });
            }

            #endregion
        }
    }
}
