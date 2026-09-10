using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting;

public abstract class AccountIdentificationRequestBase : AccountingIdentificationRequestBase
{
    #region Constructor

    protected AccountIdentificationRequestBase(Guid requestId, int accountingNumber, string accountNumber, DateTimeOffset statusDate, IFormatProvider formatProvider, ISecurityContext securityContext)
        : base(requestId, accountingNumber, statusDate, formatProvider, securityContext)
    {
        AccountNumber = accountNumber;
    }

    #endregion

    #region Properties

    public string AccountNumber { get; set; }

    #endregion
}