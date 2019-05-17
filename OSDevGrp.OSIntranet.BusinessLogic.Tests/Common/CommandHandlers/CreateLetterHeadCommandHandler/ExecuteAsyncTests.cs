using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Common.CommandHandlers.CreateLetterHeadCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.CommandHandlers.CreateLetterHeadCommandHandler
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

            Mock<ICreateLetterHeadCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertCreateLetterHeadAsyncWasCalledOnCommonRepository()
        {
            CommandHandler sut = CreateSut();

            ILetterHead letterHead = _fixture.BuildLetterHeadMock().Object;
            ICreateLetterHeadCommand command = CreateCommandMock(letterHead).Object;
            await sut.ExecuteAsync(command);

            _commonRepositoryMock.Verify(m => m.CreateLetterHeadAsync(It.Is<ILetterHead>(value => value == letterHead)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _commonRepositoryMock.Setup(m => m.CreateLetterHeadAsync(It.IsAny<ILetterHead>()))
                .Returns(Task.Run(() => _fixture.BuildLetterHeadMock().Object));

            return new CommandHandler(_validatorMock.Object, _commonRepositoryMock.Object);
        }

        private Mock<ICreateLetterHeadCommand> CreateCommandMock(ILetterHead letterHead = null)
        {
            Mock<ICreateLetterHeadCommand> commandMock = new Mock<ICreateLetterHeadCommand>();
            commandMock.Setup(m => m.ToDomain())
                .Returns(letterHead ?? _fixture.BuildLetterHeadMock().Object);
            return commandMock;
        }
    }
}