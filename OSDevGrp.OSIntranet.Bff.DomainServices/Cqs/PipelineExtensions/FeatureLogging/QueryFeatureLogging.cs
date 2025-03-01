using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Cqs.PipelineExtensions.FeatureLogging;

internal class QueryFeatureLogging<TRequest, TResponse> : IQueryFeature<TRequest, TResponse>, IQueryDecorator<TRequest, TResponse> where TRequest : IRequest where TResponse : IResponse
{
    #region Private variables

    private readonly IQueryFeature<TRequest, TResponse> _innerFeature;
    private readonly ILoggerFactory _loggerFactory;

    #endregion

    #region Constructor

    public QueryFeatureLogging(IQueryFeature<TRequest, TResponse> innerFeature, ILoggerFactory loggerFactory)
    {
        _innerFeature = innerFeature;
        _loggerFactory = loggerFactory;
    }

    #endregion

    #region Methods

    public async Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        Type innerMostFeatureType = this.GetInnerMostFeature().GetType();
        ILogger logger = _loggerFactory.CreateLogger(innerMostFeatureType);

        Guid requestId = request.RequestId;

        logger.LogDebug("Starting executing of query feature {FeatureNamespace}.{FeatureName} for request ID {RequestId}", innerMostFeatureType.Namespace, innerMostFeatureType.Name, requestId);
        try
        {
            return await _innerFeature.ExecuteAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while executing of query feature {FeatureNamespace}.{FeatureName} for request ID {RequestId}: {Exception}", innerMostFeatureType.Namespace, innerMostFeatureType.Name, requestId, ex);
            throw;
        }
        finally
        {
            logger.LogDebug("Finishing executing of query feature {FeatureNamespace}.{FeatureName} for request ID {RequestId}", innerMostFeatureType.Namespace, innerMostFeatureType.Name, requestId);
        }
    }

    public IQueryFeature<TRequest, TResponse> GetInnerFeature() => _innerFeature;

    #endregion
}