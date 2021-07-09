using System;
using System.Collections.Concurrent;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;

namespace OSDevGrp.OSIntranet.Core.Tests.EventPublisher
{
    [TestFixture]
    public class AddSubscriberTests
    {
        [Test]
        [Category("UnitTest")]
        public void AddSubscriber_WhenEventHandlerIsNull_ThrowsArgumentNullException()
        {
            IEventPublisher sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddSubscriber(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("eventHandler"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void AddSubscriber_WhenCalledWithNonAddedEventHandler_AddsEventHandler()
        {
            IEventPublisher sut = CreateSut();

            using TestEventHandler testEventHandler = new TestEventHandler();

            sut.AddSubscriber(testEventHandler);

            Assert.That(((ConcurrentDictionary<IEventHandler, int>) sut).ContainsKey(testEventHandler));
        }

        [Test]
        [Category("UnitTest")]
        public void AddSubscriber_WhenCalledWithSameEventHandlerMultipleTimes_EventHandlerHasOnlyBeenAddedOnce()
        {
            IEventPublisher sut = CreateSut();

            using TestEventHandler testEventHandler = new TestEventHandler();

            sut.AddSubscriber(testEventHandler);
            sut.AddSubscriber(testEventHandler);
            sut.AddSubscriber(testEventHandler);

            Assert.That(((ConcurrentDictionary<IEventHandler, int>) sut).Count, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        public void AddSubscriber_WhenCalledMultipleNonAddedEventHandler_AddsNonAddedEventHandlers()
        {
            IEventPublisher sut = CreateSut();

            using TestEventHandler firstTestEventHandler = new TestEventHandler();
            using TestEventHandler secondTestEventHandler = new TestEventHandler();
            using TestEventHandler thirdTestEventHandler = new TestEventHandler();

            sut.AddSubscriber(firstTestEventHandler);
            sut.AddSubscriber(secondTestEventHandler);
            sut.AddSubscriber(thirdTestEventHandler);

            Assert.That(((ConcurrentDictionary<IEventHandler, int>) sut).Count, Is.EqualTo(3));
        }

        private static IEventPublisher CreateSut()
        {
            return new Core.EventPublisher();
        }
    }
}