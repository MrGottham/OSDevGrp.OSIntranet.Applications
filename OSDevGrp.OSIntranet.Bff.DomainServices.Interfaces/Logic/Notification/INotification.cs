namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Notification;

public interface INotification
{
    Guid Identifier { get; }

    NotificationFormat Format { get; }

    NotificationType Type { get; }

    string Header { get; }

    string Content { get; }
}