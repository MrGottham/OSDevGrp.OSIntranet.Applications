namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Notification;

public interface INotificationPublisher
{
    Task PublishAsync(INotification notification, CancellationToken cancellationToken = default);
}