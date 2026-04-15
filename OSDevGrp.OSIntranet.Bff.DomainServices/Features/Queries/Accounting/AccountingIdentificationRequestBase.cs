using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting;

public abstract class AccountingIdentificationRequestBase : PageRequestBase
{
    #region Constructor

    protected AccountingIdentificationRequestBase(Guid requestId, int accountingNumber, DateTimeOffset statusDate, IFormatProvider formatProvider, ISecurityContext securityContext)
        : base(requestId, formatProvider, securityContext)
    {
        AccountingNumber = accountingNumber;
        StatusDate = statusDate;
    }

    #endregion

    #region Properties

    public int AccountingNumber { get; set; }

    public DateTimeOffset StatusDate { get; set; }

    #endregion
}