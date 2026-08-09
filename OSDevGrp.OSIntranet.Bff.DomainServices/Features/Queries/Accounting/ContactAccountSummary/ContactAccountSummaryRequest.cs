using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.ContactAccountSummary;

public class ContactAccountSummaryRequest : AccountIdentificationRequestBase
{
    #region Constructor

    public ContactAccountSummaryRequest(Guid requestId, int accountingNumber, string accountNumber, DateTimeOffset statusDate, IFormatProvider formatProvider, ISecurityContext securityContext)
        : base(requestId, accountingNumber, accountNumber, statusDate, formatProvider, securityContext)
    {
    }

    #endregion
}