using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Accounting.AccountingIdentificationFeatureBase;

public class MyAccountingIdentificationRequest : AccountingIdentificationRequestBase
{
    #region Constructor

    public MyAccountingIdentificationRequest(Guid requestId, int accountingNumber, DateTimeOffset statusDate, IFormatProvider formatProvider, ISecurityContext securityContext)
        : base(requestId, accountingNumber, statusDate, formatProvider, securityContext)
    {
    }

    #endregion
}