using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.Accounting;

public class AccountingRequest : AccountingIdentificationRequestBase
{
    #region Constructor

    public AccountingRequest(Guid requestId, int accountingNumber, DateTimeOffset statusDate, int numberOfPostingLines, IFormatProvider formatProvider, ISecurityContext securityContext)
        : base(requestId, accountingNumber, statusDate, formatProvider, securityContext)
    {
        NumberOfPostingLines = numberOfPostingLines;
    }

    #endregion

    #region Properties

    public int NumberOfPostingLines { get; }

    #endregion
}