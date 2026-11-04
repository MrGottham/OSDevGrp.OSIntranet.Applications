using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Notification;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Notification;

internal class NotificationPublisher : INotificationPublisher
{
    #region Private variables

    private readonly NotificationCollection _notificationCollection;

    #endregion

    #region Constructor

    public NotificationPublisher(NotificationCollection notificationCollection)
    {
        _notificationCollection = notificationCollection ?? throw new ArgumentNullException(nameof(notificationCollection));
    }

    #endregion

    #region Methods

    public Task PublishAsync(INotification notification, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(notification);
        cancellationToken.ThrowIfCancellationRequested();

        _notificationCollection.Publish(notification);
        return Task.CompletedTask;
    }

    #endregion
}