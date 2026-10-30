using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Notifications;

public class NotificationsRequest : PageRequestBase
{
    #region Constructor

    public NotificationsRequest(Guid requestId, IFormatProvider formatProvider, ISecurityContext securityContext)
        : base(requestId, formatProvider, securityContext)
    {
    }

    #endregion
}