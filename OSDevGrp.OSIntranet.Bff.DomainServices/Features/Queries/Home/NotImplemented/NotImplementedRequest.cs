using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.NotImplemented;

public class NotImplementedRequest : PageRequestBase
{
    #region Constructor

    public NotImplementedRequest(Guid requestId, IFormatProvider formatProvider, ISecurityContext securityContext)
        : base(requestId, formatProvider, securityContext)
    {
    }

    #endregion
}