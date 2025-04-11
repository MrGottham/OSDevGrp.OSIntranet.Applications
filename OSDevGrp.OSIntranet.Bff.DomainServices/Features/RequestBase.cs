using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features;

public abstract class RequestBase : IRequest
{
    #region Constructor

    protected RequestBase(Guid requestId, ISecurityContext securityContext)
    {
        RequestId = requestId;
        SecurityContext = securityContext ?? throw new ArgumentNullException(nameof(securityContext));
    }

    #endregion

    #region Properties

    public Guid RequestId { get; }

    public ISecurityContext SecurityContext { get; }

    #endregion
}