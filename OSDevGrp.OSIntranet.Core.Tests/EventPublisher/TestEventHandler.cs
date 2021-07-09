using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;

namespace OSDevGrp.OSIntranet.Core.Tests.EventPublisher
{
    internal class TestEventHandler : IEventHandler<TestEvent>
    {
        #region Properties

        internal bool HandleAsyncWasCalled { get; private set; }

        internal TestEvent HandleAsyncWasCalledWithEvent { get; private set; }

        #endregion

        #region Methods

        public Task HandleAsync(TestEvent testEvent)
        {
            Core.NullGuard.NotNull(testEvent, nameof(testEvent));

            HandleAsyncWasCalled = true;
            HandleAsyncWasCalledWithEvent = testEvent;

            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }

        #endregion
    }
}