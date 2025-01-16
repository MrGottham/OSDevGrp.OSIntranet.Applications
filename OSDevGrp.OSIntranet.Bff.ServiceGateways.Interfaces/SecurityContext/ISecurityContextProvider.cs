namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

public interface ISecurityContextProvider
{
    Task<ISecurityContext> GetCurrentSecurityContextAsync(CancellationToken cancellationToken = default);
}