using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Common.CommandHandlers.UpdateLetterHeadCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.CommandHandlers.UpdateLetterHeadCommandHandler
{
    [TestFixture]
    public class ExecuteAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<ICommonRepository> _commonRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _commonRepositoryMock = new Mock<ICommonRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IUpdateLetterHeadCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertUpdateLetterHeadAsyncWasCalledOnCommonRepository()
        {
            CommandHandler sut = CreateSut();

            ILetterHead letterHead = _fixture.BuildLetterHeadMock().Object;
            IUpdateLetterHeadCommand command = CreateCommandMock(letterHead).Object;
            await sut.ExecuteAsync(command);

            _commonRepositoryMock.Verify(m => m.UpdateLetterHeadAsync(It.Is<ILetterHead>(value => value == letterHead)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _commonRepositoryMock.Setup(m => m.UpdateLetterHeadAsync(It.IsAny<ILetterHead>()))
                .Returns(Task.Run(() => _fixture.BuildLetterHeadMock().Object));

            return new CommandHandler(_validatorMock.Object, _commonRepositoryMock.Object);
        }

        private Mock<IUpdateLetterHeadCommand> CreateCommandMock(ILetterHead letterHead = null)
        {
            Mock<IUpdateLetterHeadCommand> commandMock = new Mock<IUpdateLetterHeadCommand>();
            commandMock.Setup(m => m.ToDomain())
                .Returns(letterHead ?? _fixture.BuildLetterHeadMock().Object);
            return commandMock;
        }
    }
}