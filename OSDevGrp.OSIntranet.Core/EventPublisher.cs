using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;

namespace OSDevGrp.OSIntranet.Core
{
    public class EventPublisher : ConcurrentDictionary<IEventHandler, int>, IEventPublisher
    {
        #region Methods

        public async Task PublishAsync<TEvent>(TEvent e) where TEvent : class, IEvent
        {
            NullGuard.NotNull(e, nameof(e));

            IList<Task> handleTaskCollection = new List<Task>();
            foreach (KeyValuePair<IEventHandler, int> item in this)
            {
                IEventHandler<TEvent> eventHandler = item.Key as IEventHandler<TEvent>;
                if (eventHandler == null)
                {
                    continue;
                }

                handleTaskCollection.Add(eventHandler.HandleAsync(e));
            }

            await Task.WhenAll(handleTaskCollection);
        }

        public void AddSubscriber(IEventHandler eventHandler)
        {
            NullGuard.NotNull(eventHandler, nameof(eventHandler));

            TryAdd(eventHandler, eventHandler.GetHashCode());
        }

        public void RemoveSubscriber(IEventHandler eventHandler)
        {
            NullGuard.NotNull(eventHandler, nameof(eventHandler));

            // ReSharper disable UnusedVariable
            TryRemove(eventHandler, out int value);
            // ReSharper restore UnusedVariable
        }

        #endregion
    }
}