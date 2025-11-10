using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountingSummary;

public class AccountingSummaryRequest : AccountingIdentificationRequestBase
{
    #region Constructor

    public AccountingSummaryRequest(Guid requestId, int accountingNumber, DateTimeOffset statusDate, IFormatProvider formatProvider, ISecurityContext securityContext) 
        : base(requestId, accountingNumber, statusDate, formatProvider, securityContext)
    {
    }

    #endregion
}