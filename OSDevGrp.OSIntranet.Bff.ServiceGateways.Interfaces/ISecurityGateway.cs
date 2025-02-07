using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;

public interface ISecurityGateway : IServiceGateway
{
    Task<AccessTokenModel> AquireTokenAsync(CancellationToken cancellationToken = default);
}