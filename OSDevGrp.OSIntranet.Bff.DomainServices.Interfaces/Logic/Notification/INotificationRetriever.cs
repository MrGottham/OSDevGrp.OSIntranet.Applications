namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Notification;

public interface INotificationRetriever
{
    Task<IReadOnlyCollection<INotification>> RetrieveAsync(CancellationToken cancellationToken = default);
}