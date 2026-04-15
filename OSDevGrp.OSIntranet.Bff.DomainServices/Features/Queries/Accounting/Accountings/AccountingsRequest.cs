using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.Accountings;

public class AccountingsRequest : PageRequestBase
{
    #region Constructor

    public AccountingsRequest(Guid requestId, IFormatProvider formatProvider, ISecurityContext securityContext) 
        : base(requestId, formatProvider, securityContext)
    {
    }

    #endregion
}