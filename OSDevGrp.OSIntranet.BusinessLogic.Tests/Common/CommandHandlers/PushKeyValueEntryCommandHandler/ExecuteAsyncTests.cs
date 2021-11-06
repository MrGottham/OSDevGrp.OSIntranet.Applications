using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Common.CommandHandlers.PushKeyValueEntryCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.CommandHandlers.PushKeyValueEntryCommandHandler
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
        public async Task ExecuteAsync_WhenCommandIsNotNull_AssertValidateWasCalledOnPushKeyValueEntryCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IPushKeyValueEntryCommand> pushKeyValueEntryCommandMock = BuildPushKeyValueEntryCommandMock();
            await sut.ExecuteAsync(pushKeyValueEntryCommandMock.Object);

            pushKeyValueEntryCommandMock.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
                    It.Is<ICommonRepository>(value => value != null && value == _commonRepositoryMock.Object)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCommandIsNotNull_AssertToDomainWasCalledOnPushKeyValueEntryCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IPushKeyValueEntryCommand> pushKeyValueEntryCommandMock = BuildPushKeyValueEntryCommandMock();
            await sut.ExecuteAsync(pushKeyValueEntryCommandMock.Object);

            pushKeyValueEntryCommandMock.Verify(m => m.ToDomain(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCommandIsNotNull_AssertPushKeyValueEntryAsyncWasCalledOnCommonRepository()
        {
            CommandHandler sut = CreateSut();

            IKeyValueEntry keyValueEntry = _fixture.BuildKeyValueEntryMock<object>().Object;
            IPushKeyValueEntryCommand pushKeyValueEntryCommand = BuildPushKeyValueEntryCommand(keyValueEntry);
            await sut.ExecuteAsync(pushKeyValueEntryCommand);

            _commonRepositoryMock.Verify(m => m.PushKeyValueEntryAsync(It.Is<IKeyValueEntry>(value => value != null && value == keyValueEntry)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _commonRepositoryMock.Setup(m => m.PushKeyValueEntryAsync(It.IsAny<IKeyValueEntry>()))
                .Returns(Task.FromResult(_fixture.BuildKeyValueEntryMock<object>().Object));

            return new CommandHandler(_validatorMock.Object, _commonRepositoryMock.Object);
        }

        private IPushKeyValueEntryCommand BuildPushKeyValueEntryCommand(IKeyValueEntry keyValueEntry = null)
        {
            return BuildPushKeyValueEntryCommandMock(keyValueEntry).Object;
        }

        private Mock<IPushKeyValueEntryCommand> BuildPushKeyValueEntryCommandMock(IKeyValueEntry keyValueEntry = null)
        {
            Mock<IPushKeyValueEntryCommand> pushKeyValueEntryCommandMock = new Mock<IPushKeyValueEntryCommand>();
            pushKeyValueEntryCommandMock.Setup(m => m.ToDomain())
                .Returns(keyValueEntry ?? _fixture.BuildKeyValueEntryMock<object>().Object);
            return pushKeyValueEntryCommandMock;
        }
    }
}