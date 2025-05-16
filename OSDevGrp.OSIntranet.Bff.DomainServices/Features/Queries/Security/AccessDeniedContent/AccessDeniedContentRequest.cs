using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.AccessDeniedContent;

public class AccessDeniedContentRequest : PageRequestBase
{
    #region Constructor

    public AccessDeniedContentRequest(Guid requestId, IFormatProvider formatProvider, ISecurityContext securityContext) 
        : base(requestId, formatProvider, securityContext)
    {
    }

    #endregion
}