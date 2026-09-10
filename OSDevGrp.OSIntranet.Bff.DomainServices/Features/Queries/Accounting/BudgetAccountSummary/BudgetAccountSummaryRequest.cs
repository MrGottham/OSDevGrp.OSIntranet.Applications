using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.BudgetAccountSummary;

public class BudgetAccountSummaryRequest : AccountIdentificationRequestBase
{
    #region Constructor

    public BudgetAccountSummaryRequest(Guid requestId, int accountingNumber, string accountNumber, DateTimeOffset statusDate, IFormatProvider formatProvider, ISecurityContext securityContext)
        : base(requestId, accountingNumber, accountNumber, statusDate, formatProvider, securityContext)
    {
    }

    #endregion
}