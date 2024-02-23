using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Core.Tests.CommandBus
{
	[TestFixture]
    public class PublishAsyncWithoutResultTests : CommandBusTestBase
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

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.PublishAsync((ICommand) null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("command"));
        }

        [Test]
        [Category("UnitTest")]
        public void PublishAsync_WhenCalledWithUnsupportedCommandType_ThrowsIntranetCommandBusException()
        {
            ICommandBus sut = CreateSut(new List<ICommandHandler>(0));

            Mock<ICommand> commandMock = new Mock<ICommand>();
            IntranetCommandBusException result = Assert.ThrowsAsync<IntranetCommandBusException>(async () => await sut.PublishAsync(commandMock.Object));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.NoCommandHandlerSupportingCommandWithoutResultType));
        }

        [Test]
        [Category("UnitTest")]
        public async Task PublishAsync_WhenCalledWithSupportedCommandType_AssertExecuteAsyncWasCalledOnCommandHandler()
        {
            Mock<ICommandHandler<TestCommand>> commandHandlerMock = CreateCommandHandlerMockWithoutResult<TestCommand>();
            ICommandBus sut = CreateSut(CreateCommandHandlerMockCollection(commandHandlerMock.Object));

            TestCommand testCommand = new TestCommand();
            await sut.PublishAsync(testCommand);

            commandHandlerMock.Verify(m => m.ExecuteAsync(It.Is<TestCommand>(cmd => cmd == testCommand)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void PublishAsync_WhenSupportingCommandHandlerThrowsIntranetExceptionBase_ThrowsIntranetExceptionBase()
        {
            IntranetRepositoryException intranetException = _fixture.Create<IntranetRepositoryException>();

            Mock<ICommandHandler<TestCommand>> commandHandlerMock = CreateCommandHandlerMockWithoutResult<TestCommand>(intranetException);
            ICommandBus sut = CreateSut(CreateCommandHandlerMockCollection(commandHandlerMock.Object));

            IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.PublishAsync(new TestCommand()));

            Assert.That(result, Is.EqualTo(intranetException));
        }

        [Test]
        [Category("UnitTest")]
        public void PublishAsync_WhenSupportingCommandHandlerThrowsAggregateExceptionWhereInnerExceptionIsIntranetExceptionBase_ThrowsIntranetExceptionBase()
        {
            IntranetRepositoryException innerException = _fixture.Create<IntranetRepositoryException>();
            AggregateException aggregateException = new AggregateException(innerException);

            Mock<ICommandHandler<TestCommand>> commandHandlerMock = CreateCommandHandlerMockWithoutResult<TestCommand>(aggregateException);
            ICommandBus sut = CreateSut(CreateCommandHandlerMockCollection(commandHandlerMock.Object));

            IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.PublishAsync(new TestCommand()));

            Assert.That(result, Is.EqualTo(innerException));
        }

        [Test]
        [Category("UnitTest")]
        public void PublishAsync_WhenSupportingCommandHandlerThrowsAggregateExceptionWhereInnerExceptionIsNotIntranetExceptionBase_ThrowsIntranetCommandBusExceptionWithCorrectErrorCode()
        {
            AggregateException aggregateException = new AggregateException(_fixture.Create<Exception>());

            Mock<ICommandHandler<TestCommand>> commandHandlerMock = CreateCommandHandlerMockWithoutResult<TestCommand>(aggregateException);
            ICommandBus sut = CreateSut(CreateCommandHandlerMockCollection(commandHandlerMock.Object));

            IntranetCommandBusException result = Assert.ThrowsAsync<IntranetCommandBusException>(async () => await sut.PublishAsync(new TestCommand()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ErrorWhilePublishingCommandWithoutResultType));
        }

        [Test]
        [Category("UnitTest")]
        public void PublishAsync_WhenSupportingCommandHandlerThrowsAggregateExceptionWhereInnerExceptionIsNotIntranetExceptionBase_ThrowsIntranetCommandBusExceptionWithCorrectInnerException()
        {
            Exception innerException = _fixture.Create<Exception>();
            AggregateException aggregateException = new AggregateException(innerException);

            Mock<ICommandHandler<TestCommand>> commandHandlerMock = CreateCommandHandlerMockWithoutResult<TestCommand>(aggregateException);
            ICommandBus sut = CreateSut(CreateCommandHandlerMockCollection(commandHandlerMock.Object));

            IntranetCommandBusException result = Assert.ThrowsAsync<IntranetCommandBusException>(async () => await sut.PublishAsync(new TestCommand()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.InnerException, Is.EqualTo(innerException));
        }

        [Test]
        [Category("UnitTest")]
        public void PublishAsync_WhenSupportingCommandHandlerThrowsException_ThrowsIntranetCommandBusExceptionWithCorrectErrorCode()
        {
            Exception exception = _fixture.Create<Exception>();

            Mock<ICommandHandler<TestCommand>> commandHandlerMock = CreateCommandHandlerMockWithoutResult<TestCommand>(exception);
            ICommandBus sut = CreateSut(CreateCommandHandlerMockCollection(commandHandlerMock.Object));

            IntranetCommandBusException result = Assert.ThrowsAsync<IntranetCommandBusException>(async () => await sut.PublishAsync(new TestCommand()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ErrorWhilePublishingCommandWithoutResultType));
        }

        [Test]
        [Category("UnitTest")]
        public void PublishAsync_WhenSupportingCommandHandlerThrowsException_ThrowsIntranetCommandBusExceptionWithCorrectInnerException()
        {
            Exception exception = _fixture.Create<Exception>();

            Mock<ICommandHandler<TestCommand>> commandHandlerMock = CreateCommandHandlerMockWithoutResult<TestCommand>(exception);
            ICommandBus sut = CreateSut(CreateCommandHandlerMockCollection(commandHandlerMock.Object));

            IntranetCommandBusException result = Assert.ThrowsAsync<IntranetCommandBusException>(async () => await sut.PublishAsync(new TestCommand()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.InnerException, Is.EqualTo(exception));
        }
    }
}