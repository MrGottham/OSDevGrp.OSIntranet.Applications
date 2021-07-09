using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;

namespace OSDevGrp.OSIntranet.Core.Tests.EventPublisher
{
    [TestFixture]
    public class PublishAsyncTests
    {
        [Test]
        [Category("UnitTest")]
        public void PublishAsync_WhenEventHandlerIsNull_ThrowsArgumentNullException()
        {
            IEventPublisher sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.PublishAsync<TestEvent>(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("e"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task PublishAsync_WhenCalled_AssertHandleAsyncWasCalledOnEachEventHandlerWhichHandlesEvent()
        {
            IEventPublisher sut = CreateSut();

            using TestEventHandler firstTestEventHandler = new TestEventHandler();
            using TestEventHandler secondTestEventHandler = new TestEventHandler();
            using TestEventHandler thirdTestEventHandler = new TestEventHandler();

            sut.AddSubscriber(firstTestEventHandler);
            sut.AddSubscriber(secondTestEventHandler);
            sut.AddSubscriber(thirdTestEventHandler);

            await sut.PublishAsync(new TestEvent());

            Assert.That(firstTestEventHandler.HandleAsyncWasCalled, Is.True);
            Assert.That(secondTestEventHandler.HandleAsyncWasCalled, Is.True);
            Assert.That(thirdTestEventHandler.HandleAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PublishAsync__WhenCalled_AssertHandleAsyncWasCalledWithEvent()
        {
            IEventPublisher sut = CreateSut();

            using TestEventHandler testEventHandler = new TestEventHandler();

            sut.AddSubscriber(testEventHandler);

            TestEvent testEvent = new TestEvent();
            await sut.PublishAsync(testEvent);

            Assert.That(testEventHandler.HandleAsyncWasCalledWithEvent, Is.EqualTo(testEvent));
        }

        private static IEventPublisher CreateSut()
        {
            return new Core.EventPublisher();
        }
    }
}