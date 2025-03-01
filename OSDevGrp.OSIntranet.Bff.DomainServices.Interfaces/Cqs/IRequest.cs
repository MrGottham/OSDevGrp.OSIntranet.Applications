using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

public interface IRequest
{
    Guid RequestId { get; }

    ISecurityContext SecurityContext { get; }
}