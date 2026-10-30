using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Notification;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Notification;

internal class NotificationRetriever : INotificationRetriever
{
    #region Private variables

    private readonly NotificationCollection _notificationCollection;

    #endregion

    #region Constructor

    public NotificationRetriever(NotificationCollection notificationCollection)
    {
        _notificationCollection = notificationCollection ?? throw new ArgumentNullException(nameof(notificationCollection));
    }

    #endregion

    #region Methods

    public Task<IReadOnlyCollection<INotification>> RetrieveAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(_notificationCollection.Retrieve());
    }

    #endregion
}