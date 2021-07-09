using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher
{
    public interface IEventPublisher
    {
        Task PublishAsync<TEvent>(TEvent e) where TEvent : class, IEvent;

        void AddSubscriber(IEventHandler eventHandler);

        void RemoveSubscriber(IEventHandler eventHandler);
    }
}