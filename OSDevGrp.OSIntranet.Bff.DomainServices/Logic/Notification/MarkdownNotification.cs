using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Notification;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Notification;

internal class MarkdownNotification : INotification
{
    #region Constructor

    public MarkdownNotification(string header, string content)
    {
        Header = header ?? throw new ArgumentNullException(nameof(header));
        Content = content ?? throw new ArgumentNullException(nameof(content));
    }

    #endregion

    #region Properties

    public NotificationFormat Format => NotificationFormat.Markdown;

    public string Header { get; }

    public string Content { get; }

    #endregion
}