using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Notification;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Notifications;

internal class NotificationsFeature : PageFeatureBase<NotificationsRequest, NotificationsResponse, object>
{
    #region Private variables

    private readonly INotificationRetriever _notificationRetriever;

    #endregion

    #region Constructor

    public NotificationsFeature(IStaticTextProvider staticTextProvider, INotificationRetriever notificationRetriever)
        : base(staticTextProvider)
    {
        _notificationRetriever = notificationRetriever ?? throw new ArgumentNullException(nameof(notificationRetriever));
    }

    #endregion

    #region Methods

    public override async Task<NotificationsResponse> ExecuteAsync(NotificationsRequest request, CancellationToken cancellationToken = default)
    {
        IReadOnlyDictionary<StaticTextKey, string> staticTexts = await GetStaticTextsAsync(request, new object(), cancellationToken);
        IReadOnlyCollection<INotification> notifications = await _notificationRetriever.RetrieveAsync(cancellationToken);

        return new NotificationsResponse(notifications, staticTexts);
    }

    protected override IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> GetStaticTextSpecifications(NotificationsRequest request, object argument)
    {
        return new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.Notifications, StaticTextKey.Notifications.DefaultArguments() }
        };
    }

    #endregion
}