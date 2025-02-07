using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;

public interface ICommonGateway : IServiceGateway
{
    Task<IEnumerable<LetterHeadModel>> GetLetterHeadsAsync(CancellationToken cancellationToken = default);
}