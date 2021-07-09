using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher
{
    public interface IEventHandler
    {
    }

    public interface IEventHandler<in TEvent> : IDisposable, IEventHandler where TEvent : IEvent
    {
        Task HandleAsync(TEvent e);
    }
}