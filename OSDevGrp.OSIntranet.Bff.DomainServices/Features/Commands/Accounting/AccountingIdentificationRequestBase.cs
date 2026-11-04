using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting;

public abstract class AccountingIdentificationRequestBase : RequestBase
{
    #region Constructor

    protected AccountingIdentificationRequestBase(Guid requestId, int accountingNumber, ISecurityContext securityContext)
        : base(requestId, securityContext)
    {
        AccountingNumber = accountingNumber;
    }

    #endregion

    #region Properties

    public int AccountingNumber { get; }

    #endregion
}