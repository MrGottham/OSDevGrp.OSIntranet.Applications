using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Common.CommandHandlers.LetterHeadIdentificationCommandHandlerBase<OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands.ILetterHeadIdentificationCommand>;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.CommandHandlers.LetterHeadIdentificationCommandHandlerBase
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

            Assert.That(result.ParamName, Is.EqualTo("command"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<ILetterHeadIdentificationCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.Validate(It.Is<IValidator>(value => value == _validatorMock.Object), It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertManageRepositoryAsyncWasCalledOnCommandHandler()
        {
            CommandHandler sut = CreateSut();

            ILetterHeadIdentificationCommand command = CreateCommandMock().Object;
            await sut.ExecuteAsync(command);

            Assert.That(((Sut) sut).ManageRepositoryAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertManageRepositoryAsyncWasCalledWithSameCommand()
        {
            CommandHandler sut = CreateSut();

            ILetterHeadIdentificationCommand command = CreateCommandMock().Object;
            await sut.ExecuteAsync(command);

            Assert.That(((Sut) sut).Command, Is.EqualTo(command));
        }

        private CommandHandler CreateSut()
        {
            return new Sut(_validatorMock.Object, _commonRepositoryMock.Object);
        }

        private Mock<ILetterHeadIdentificationCommand> CreateCommandMock()
        {
            return new Mock<ILetterHeadIdentificationCommand>();
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

            public ILetterHeadIdentificationCommand Command { get; private set; }

            #endregion

            #region Methods

            protected override Task ManageRepositoryAsync(ILetterHeadIdentificationCommand command)
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