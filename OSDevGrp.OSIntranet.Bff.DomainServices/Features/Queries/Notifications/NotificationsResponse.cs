using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Notification;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Notifications;

public class NotificationsResponse : PageResponseBase
{
    #region Constructor

    public NotificationsResponse(IReadOnlyCollection<INotification> notifications, IReadOnlyDictionary<StaticTextKey, string> staticTexts)
        : base(staticTexts)
    {
        Notifications = notifications ?? throw new ArgumentNullException(nameof(notifications));
    }

    #endregion

    #region Properties

    public IReadOnlyCollection<INotification> Notifications { get; }

    #endregion
}