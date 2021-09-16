using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Common.CommandHandlers.KeyValueEntryIdentificationCommandHandlerBase<OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands.IKeyValueEntryIdentificationCommand>;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.CommandHandlers.KeyValueEntryIdentificationCommandHandlerBase
{
    [TestFixture]
    public class ExecuteAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<ICommonRepository> _commonRepositoryMock;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _commonRepositoryMock = new Mock<ICommonRepository>();
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenCommandIsNull_ThrowsArgumentNullException()
        {
            CommandHandler sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("command"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCommandIsNotNull_AssertValidateWasCalledOnKeyValueEntryIdentificationCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IKeyValueEntryIdentificationCommand> keyValueEntryIdentificationCommandMock = BuildKeyValueEntryIdentificationCommandMock();
            await sut.ExecuteAsync(keyValueEntryIdentificationCommandMock.Object);

            keyValueEntryIdentificationCommandMock.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
                    It.Is<ICommonRepository>(value => value != null && value == _commonRepositoryMock.Object)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCommandIsNotNull_AssertManageRepositoryAsyncWasCalledOnKeyValueEntryIdentificationCommandHandlerBase()
        {
            CommandHandler sut = CreateSut();

            await sut.ExecuteAsync(BuildKeyValueEntryIdentificationCommand());

            Assert.That(((Sut) sut).ManageRepositoryAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCommandIsNotNull_AssertManageRepositoryAsyncWasCalledOnKeyValueEntryIdentificationCommandHandlerBaseWithKeyValueEntryIdentificationCommand()
        {
            CommandHandler sut = CreateSut();

            IKeyValueEntryIdentificationCommand keyValueEntryIdentificationCommand = BuildKeyValueEntryIdentificationCommand();
            await sut.ExecuteAsync(keyValueEntryIdentificationCommand);

            Assert.That(((Sut)sut).ManageRepositoryAsyncWasCalledWithKeyValueEntryIdentificationCommand, Is.EqualTo(keyValueEntryIdentificationCommand));
        }

        private CommandHandler CreateSut()
        {
            return new Sut(_validatorMock.Object, _commonRepositoryMock.Object);
        }

        private static IKeyValueEntryIdentificationCommand BuildKeyValueEntryIdentificationCommand()
        {
            return BuildKeyValueEntryIdentificationCommandMock().Object;
        }

        private static Mock<IKeyValueEntryIdentificationCommand> BuildKeyValueEntryIdentificationCommandMock()
        {
            return new Mock<IKeyValueEntryIdentificationCommand>();
        }

        private class Sut : CommandHandler
        {
            #region Constructor

            public Sut(IValidator validator, ICommonRepository commonRepository) 
                : base(validator, commonRepository)
            {
            }

            #endregion

            #region Properties

            public bool ManageRepositoryAsyncWasCalled { get; private set; }

            public IKeyValueEntryIdentificationCommand ManageRepositoryAsyncWasCalledWithKeyValueEntryIdentificationCommand { get; private set; }

            #endregion

            #region Methods

            protected override Task ManageRepositoryAsync(IKeyValueEntryIdentificationCommand command)
            {
                NullGuard.NotNull(command, nameof(command));

                ManageRepositoryAsyncWasCalled = true;
                ManageRepositoryAsyncWasCalledWithKeyValueEntryIdentificationCommand = command;

                return Task.CompletedTask;
            }

            #endregion
        }
    }
}