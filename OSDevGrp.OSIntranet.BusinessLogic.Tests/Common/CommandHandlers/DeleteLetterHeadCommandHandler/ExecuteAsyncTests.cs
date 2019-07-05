using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Common.CommandHandlers.DeleteLetterHeadCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.CommandHandlers.DeleteLetterHeadCommandHandler
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
        public async Task ExecuteAsync_WhenCalled_AssertNumberWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IDeleteLetterHeadCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertDeleteLetterHeadAsyncWasCalledOnCommonRepository()
        {
            CommandHandler sut = CreateSut();

            int number = _fixture.Create<int>();
            IDeleteLetterHeadCommand command = CreateCommandMock(number).Object;
            await sut.ExecuteAsync(command);

            _commonRepositoryMock.Verify(m => m.DeleteLetterHeadAsync(It.Is<int>(value => value == number)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _commonRepositoryMock.Setup(m => m.DeleteLetterHeadAsync(It.IsAny<int>()))
                .Returns(Task.Run(() => (ILetterHead) null));

            return new CommandHandler(_validatorMock.Object, _commonRepositoryMock.Object);
        }

        private Mock<IDeleteLetterHeadCommand> CreateCommandMock(int? number = null)
        {
            Mock<IDeleteLetterHeadCommand> commandMock = new Mock<IDeleteLetterHeadCommand>();
            commandMock.Setup(m => m.Number)
                .Returns(number ?? _fixture.Create<int>());
            return commandMock;
        }
    }
}