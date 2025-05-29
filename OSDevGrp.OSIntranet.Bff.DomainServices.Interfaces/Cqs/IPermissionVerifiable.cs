using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

public interface IPermissionVerifiable<TRequest> where TRequest : IRequest
{
    Task<bool> VerifyPermissionAsync(ISecurityContext securityContext, TRequest request, CancellationToken cancellationToken = default);
}