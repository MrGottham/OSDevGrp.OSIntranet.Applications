using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Accounting.AccountIdentificationFeatureBase;

public class MyAccountIdentificationRequest : AccountIdentificationRequestBase
{
    #region Constructor

    public MyAccountIdentificationRequest(Guid requestId, int accountingNumber, string accountNumber, DateTimeOffset statusDate, IFormatProvider formatProvider, ISecurityContext securityContext)
        : base(requestId, accountingNumber, accountNumber, statusDate, formatProvider, securityContext)
    {
    }

    #endregion
}