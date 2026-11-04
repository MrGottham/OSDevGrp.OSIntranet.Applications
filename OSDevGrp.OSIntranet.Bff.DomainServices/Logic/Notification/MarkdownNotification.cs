using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Notification;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Notification;

internal class MarkdownNotification : INotification
{
    #region Constructor

    public MarkdownNotification(Guid identifier, NotificationType type, string header, string content)
    {
        Identifier = identifier;
        Type = type;
        Header = header ?? throw new ArgumentNullException(nameof(header));
        Content = content ?? throw new ArgumentNullException(nameof(content));
    }

    #endregion

    #region Properties

    public Guid Identifier { get; }

    public NotificationFormat Format => NotificationFormat.Markdown;

    public NotificationType Type { get; }

    public string Header { get; }

    public string Content { get; }

    #endregion
}