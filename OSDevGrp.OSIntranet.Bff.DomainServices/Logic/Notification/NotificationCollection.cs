using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Notification;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Notification;

internal class NotificationCollection
{
    #region Private variables

    private readonly object _syncRoot = new();
    private readonly Queue<INotification> _notifications = new();

    #endregion

    #region Methods

    public void Publish(INotification notification)
    {
        lock (_syncRoot)
        {
            _notifications.Enqueue(notification);
        }
    }

    public IReadOnlyCollection<INotification> Retrieve()
    {
        lock (_syncRoot)
        {
            List<INotification> notifications = new(_notifications.Count);
            while (_notifications.Count > 0)
            {
                notifications.Add(_notifications.Dequeue());
            }

            return notifications.AsReadOnly();
        }
    }

    #endregion
}