using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.Core.Tests.CommandBus
{
    [TestFixture]
    public class PublishAsyncWithResultTests : CommandBusTestBase
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void PublishAsync_WhenCommandIsNull_ThrowsArgumentNullException()
        {
            ICommandBus sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.PublishAsync<ICommand, object>((ICommand) null));

            Assert.AreEqual(result.ParamName, "command");
        }

        [Test]
        [Category("UnitTest")]
        public void PublishAsync_WhenCalledWithUnsupportedCommandType_ThrowsIntranetCommandBusException()
        {
            ICommandBus sut = CreateSut(new List<ICommandHandler>(0));

            Mock<ICommand> commandMock = new Mock<ICommand>();
            IntranetCommandBusException result = Assert.Throws<IntranetCommandBusException>(() => sut.PublishAsync<ICommand, object>(commandMock.Object));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.NoCommandHandlerSupportingCommandWithResultType));
        }

        [Test]
        [Category("UnitTest")]
        public async Task PublishAsync_WhenCalledWithSupportedCommandType_AssertExecuteAsyncWasCalledOnCommandHandler()
        {
            Mock<ICommandHandler<TestCommand, object>> commandHandlerMock = CreateCommandHandlerMockWithResult<TestCommand, object>(_fixture.Create<object>());
            ICommandBus sut = CreateSut(CreateCommandHandlerMockCollection(commandHandlerMock.Object));

            TestCommand testCommand = new TestCommand();
            await sut.PublishAsync<TestCommand, object>(testCommand);
            
            commandHandlerMock.Verify(m => m.ExecuteAsync(It.Is<TestCommand>(cmd => cmd == testCommand)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PublishAsync_WhenCalledWithSupportedCommandType_ReturnsResultFromCommandHandler()
        {
            object expectedResult = _fixture.Create<object>();

            Mock<ICommandHandler<TestCommand, object>> commandHandlerMock = CreateCommandHandlerMockWithResult<TestCommand, object>(expectedResult);
            ICommandBus sut = CreateSut(CreateCommandHandlerMockCollection(commandHandlerMock.Object));

            object result = await sut.PublishAsync<TestCommand, object>(new TestCommand());

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [Category("UnitTest")]
        public void PublishAsync_WhenSupportingCommandHandlerThrowsIntranetExceptionBase_ThrowsIntranetExceptionBase()
        {
            IntranetRepositoryException intranetException = _fixture.Create<IntranetRepositoryException>();

            Mock<ICommandHandler<TestCommand, object>> commandHandlerMock = CreateCommandHandlerMockWithResult<TestCommand, object>(_fixture.Create<object>(), intranetException);
            ICommandBus sut = CreateSut(CreateCommandHandlerMockCollection(commandHandlerMock.Object));

            IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.PublishAsync<TestCommand, object>(new TestCommand()));

            Assert.That(result, Is.EqualTo(intranetException));
        }

        [Test]
        [Category("UnitTest")]
        public void PublishAsync_WhenSupportingCommandHandlerThrowsAggregateExceptionWhereInnerExceptionIsIntranetExceptionBase_ThrowsIntranetExceptionBase()
        {
            IntranetRepositoryException innerException = _fixture.Create<IntranetRepositoryException>();
            AggregateException aggregateException = new AggregateException(new Exception[] {innerException});

            Mock<ICommandHandler<TestCommand, object>> commandHandlerMock = CreateCommandHandlerMockWithResult<TestCommand, object>(_fixture.Create<object>(), aggregateException);
            ICommandBus sut = CreateSut(CreateCommandHandlerMockCollection(commandHandlerMock.Object));

            IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.PublishAsync<TestCommand, object>(new TestCommand()));

            Assert.That(result, Is.EqualTo(innerException));
        }

        [Test]
        [Category("UnitTest")]
        public void PublishAsync_WhenSupportingCommandHandlerThrowsAggregateExceptionWhereInnerExceptionIsNotIntranetExceptionBase_ThrowsIntranetCommandBusExceptionWithCorrectErrorCode()
        {
            AggregateException aggregateException = new AggregateException(new Exception[] {_fixture.Create<Exception>()});

            Mock<ICommandHandler<TestCommand, object>> commandHandlerMock = CreateCommandHandlerMockWithResult<TestCommand, object>(_fixture.Create<object>(), aggregateException);
            ICommandBus sut = CreateSut(CreateCommandHandlerMockCollection(commandHandlerMock.Object));

            IntranetCommandBusException result = Assert.ThrowsAsync<IntranetCommandBusException>(async () => await sut.PublishAsync<TestCommand, object>(new TestCommand()));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ErrorWhilePublishingCommandWithResultType));
        }

        [Test]
        [Category("UnitTest")]
        public void PublishAsync_WhenSupportingCommandHandlerThrowsAggregateExceptionWhereInnerExceptionIsNotIntranetExceptionBase_ThrowsIntranetCommandBusExceptionWithCorrectInnerException()
        {
            Exception innerException = _fixture.Create<Exception>();
            AggregateException aggregateException = new AggregateException(new Exception[] {innerException});

            Mock<ICommandHandler<TestCommand, object>> commandHandlerMock = CreateCommandHandlerMockWithResult<TestCommand, object>(_fixture.Create<object>(), aggregateException);
            ICommandBus sut = CreateSut(CreateCommandHandlerMockCollection(commandHandlerMock.Object));

            IntranetCommandBusException result = Assert.ThrowsAsync<IntranetCommandBusException>(async () => await sut.PublishAsync<TestCommand, object>(new TestCommand()));

            Assert.That(result.InnerException, Is.EqualTo(innerException));
        }

        [Test]
        [Category("UnitTest")]
        public void PublishAsync_WhenSupportingCommandHandlerThrowsException_ThrowsIntranetCommandBusExceptionWithCorrectErrorCode()
        {
            Exception exception = _fixture.Create<Exception>();

            Mock<ICommandHandler<TestCommand, object>> commandHandlerMock = CreateCommandHandlerMockWithResult<TestCommand, object>(_fixture.Create<object>(), exception);
            ICommandBus sut = CreateSut(CreateCommandHandlerMockCollection(commandHandlerMock.Object));

            IntranetCommandBusException result = Assert.ThrowsAsync<IntranetCommandBusException>(async () => await sut.PublishAsync<TestCommand, object>(new TestCommand()));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ErrorWhilePublishingCommandWithResultType));
        }

        [Test]
        [Category("UnitTest")]
        public void PublishAsync_WhenSupportingCommandHandlerThrowsException_ThrowsIntranetCommandBusExceptionWithCorrectInnerException()
        {
            Exception exception = _fixture.Create<Exception>();

            Mock<ICommandHandler<TestCommand, object>> commandHandlerMock = CreateCommandHandlerMockWithResult<TestCommand, object>(_fixture.Create<object>(), exception);
            ICommandBus sut = CreateSut(CreateCommandHandlerMockCollection(commandHandlerMock.Object));

            IntranetCommandBusException result = Assert.ThrowsAsync<IntranetCommandBusException>(async () => await sut.PublishAsync<TestCommand, object>(new TestCommand()));

            Assert.That(result.InnerException, Is.EqualTo(exception));
        }
    }
}