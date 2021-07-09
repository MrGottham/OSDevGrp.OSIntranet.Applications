using System;
using System.Collections.Concurrent;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;

namespace OSDevGrp.OSIntranet.Core.Tests.EventPublisher
{
    [TestFixture]
    public class RemoveSubscriberTests
    {
        [Test]
        [Category("UnitTest")]
        public void RemoveSubscriber_WhenEventHandlerIsNull_ThrowsArgumentNullException()
        {
            IEventPublisher sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.RemoveSubscriber(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("eventHandler"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void RemoveSubscriber_WhenCalledWithNonAddedEventHandler_ExpectNoRemovedEventHandlers()
        {
            IEventPublisher sut = CreateSut();

            using TestEventHandler firstTestEventHandler = new TestEventHandler();
            using TestEventHandler secondTestEventHandler = new TestEventHandler();

            sut.AddSubscriber(firstTestEventHandler);

            Assert.That(((ConcurrentDictionary<IEventHandler, int>) sut).Count, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        public void RemoveSubscriber_WhenCalledWithAddedEventHandler_ExpectEventHandlerHasBeenRemoved()
        {
            IEventPublisher sut = CreateSut();

            using TestEventHandler testEventHandler = new TestEventHandler();

            sut.AddSubscriber(testEventHandler);

            Assert.That(((ConcurrentDictionary<IEventHandler, int>) sut).ContainsKey(testEventHandler), Is.True);

            sut.RemoveSubscriber(testEventHandler);

            Assert.That(((ConcurrentDictionary<IEventHandler, int>) sut).ContainsKey(testEventHandler), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void RemoveSubscriber_WhenCalledWithAddedEventHandlerMultipleTimes_ExpectEventHandlerHasOnlyBeenRemovedOnce()
        {
            IEventPublisher sut = CreateSut();

            using TestEventHandler testEventHandler = new TestEventHandler();

            sut.AddSubscriber(testEventHandler);

            Assert.That(((ConcurrentDictionary<IEventHandler, int>) sut).ContainsKey(testEventHandler), Is.True);

            sut.RemoveSubscriber(testEventHandler);
            sut.RemoveSubscriber(testEventHandler);
            sut.RemoveSubscriber(testEventHandler);

            Assert.That(((ConcurrentDictionary<IEventHandler, int>) sut).ContainsKey(testEventHandler), Is.False);
        }

        private static IEventPublisher CreateSut()
        {
            return new Core.EventPublisher();
        }
    }
}