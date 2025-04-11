using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

public interface IPermissionVerifiable
{
    Task<bool> VerifyPermissionAsync<TRequest>(ISecurityContext securityContext, TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest;
}