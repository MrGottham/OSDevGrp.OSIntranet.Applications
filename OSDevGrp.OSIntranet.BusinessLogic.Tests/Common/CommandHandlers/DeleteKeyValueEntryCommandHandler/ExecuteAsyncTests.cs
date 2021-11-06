using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Common.CommandHandlers.DeleteKeyValueEntryCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.CommandHandlers.DeleteKeyValueEntryCommandHandler
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
        public async Task ExecuteAsync_WhenCommandIsNotNull_AssertValidateWasCalledOnDeleteKeyValueEntryCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IDeleteKeyValueEntryCommand> deleteKeyValueEntryCommandMock = BuildDeleteKeyValueEntryCommandMock();
            await sut.ExecuteAsync(deleteKeyValueEntryCommandMock.Object);

            deleteKeyValueEntryCommandMock.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
                    It.Is<ICommonRepository>(value => value != null && value == _commonRepositoryMock.Object)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCommandIsNotNull_AssertKeyWasCalledOnDeleteKeyValueEntryCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IDeleteKeyValueEntryCommand> deleteKeyValueEntryCommandMock = BuildDeleteKeyValueEntryCommandMock();
            await sut.ExecuteAsync(deleteKeyValueEntryCommandMock.Object);

            deleteKeyValueEntryCommandMock.Verify(m => m.Key, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCommandIsNotNull_AssertDeleteKeyValueEntryAsyncWasCalledOnCommonRepository()
        {
            CommandHandler sut = CreateSut();

            string key = _fixture.Create<string>();
            IDeleteKeyValueEntryCommand deleteKeyValueEntryCommand = BuildDeleteKeyValueEntryCommand(key);
            await sut.ExecuteAsync(deleteKeyValueEntryCommand);

            _commonRepositoryMock.Verify(m => m.DeleteKeyValueEntryAsync(It.Is<string>(value => string.CompareOrdinal(value, key) == 0)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            _commonRepositoryMock.Setup(m => m.DeleteKeyValueEntryAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IKeyValueEntry>(null));

            return new CommandHandler(_validatorMock.Object, _commonRepositoryMock.Object);
        }

        private IDeleteKeyValueEntryCommand BuildDeleteKeyValueEntryCommand(string key = null)
        {
            return BuildDeleteKeyValueEntryCommandMock(key).Object;
        }

        private Mock<IDeleteKeyValueEntryCommand> BuildDeleteKeyValueEntryCommandMock(string key = null)
        {
            Mock<IDeleteKeyValueEntryCommand> deleteKeyValueEntryCommandMock = new Mock<IDeleteKeyValueEntryCommand>();
            deleteKeyValueEntryCommandMock.Setup(m => m.Key)
                .Returns(key ?? _fixture.Create<string>());
            return deleteKeyValueEntryCommandMock;
        }
    }
}