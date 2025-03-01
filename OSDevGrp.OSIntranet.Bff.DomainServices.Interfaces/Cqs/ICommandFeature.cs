namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

public interface ICommandFeature<TRequest> where TRequest : IRequest
{
    Task ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);
}