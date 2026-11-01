namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Notification;

public interface INotification
{
    NotificationFormat Format { get; }

    NotificationType Type { get; }

    string Header { get; }

    string Content { get; }
}