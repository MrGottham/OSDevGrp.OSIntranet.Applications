using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Cqs.PipelineExtensions.FeatureCancellation;

internal class QueryFeatureCancellation<TRequest, TResponse> : IQueryFeature<TRequest, TResponse>, IQueryDecorator<TRequest, TResponse> where TRequest : IRequest where TResponse : IResponse
{
    #region Private variables

    private readonly IQueryFeature<TRequest, TResponse> _innerFeature;

    #endregion

    #region Constructor

    public QueryFeatureCancellation(IQueryFeature<TRequest, TResponse> innerFeature)
    {
        _innerFeature = innerFeature;
    }

    #endregion

    #region Methods

    public Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return _innerFeature.ExecuteAsync(request, cancellationToken);
    }

    public IQueryFeature<TRequest, TResponse> GetInnerFeature() => _innerFeature;

    #endregion
}