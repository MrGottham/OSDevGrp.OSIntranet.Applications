using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.Error;

public class ErrorRequest : PageRequestBase
{
    #region Constructor

    public ErrorRequest(Guid requestId, string errorMessage, IFormatProvider formatProvider, ISecurityContext securityContext)
        : base(requestId, formatProvider, securityContext)
    {
        ErrorMessage = errorMessage;
    }

    #endregion

    #region Properties

    public string ErrorMessage { get; }

    #endregion
}