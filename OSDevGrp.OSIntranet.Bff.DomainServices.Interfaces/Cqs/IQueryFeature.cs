namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

public interface IQueryFeature<in TRequest, TResponse> where TRequest : IRequest where TResponse : IResponse
{
    Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);
}