using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountingSummary;

public class AccountingSummaryRequest : AccountingIdentificationRequestBase
{
    #region Constructor

    public AccountingSummaryRequest(Guid requestId, int accountingNumber, DateTimeOffset statusDate, int numberOfPostingLines, IFormatProvider formatProvider, ISecurityContext securityContext) 
        : base(requestId, accountingNumber, statusDate, formatProvider, securityContext)
    {
        NumberOfPostingLines = numberOfPostingLines;
    }

    #endregion

    #region Properties

    public int NumberOfPostingLines { get; }

    #endregion
}