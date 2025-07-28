using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountingPreCreation;

public class AccountingPreCreationRequest : PageRequestBase
{
    #region Constructor

    public AccountingPreCreationRequest(Guid requestId, IFormatProvider formatProvider, ISecurityContext securityContext)
        : base(requestId, formatProvider, securityContext)
    {
    }

    #endregion
}