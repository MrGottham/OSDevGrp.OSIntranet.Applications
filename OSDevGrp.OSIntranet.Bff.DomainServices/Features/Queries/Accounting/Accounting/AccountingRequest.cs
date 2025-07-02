using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.Accounting;

public class AccountingRequest : AccountingIdentificationRequestBase
{
    #region Constructor

    public AccountingRequest(Guid requestId, int accountingNumber, DateTimeOffset statusDate, IFormatProvider formatProvider, ISecurityContext securityContext)
        : base(requestId, accountingNumber, statusDate, formatProvider, securityContext)
    {
    }

    #endregion
}