using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Cqs.PipelineExtensions.FeatureLogging;

internal class CommandFeatureLogging<TRequest> : ICommandFeature<TRequest>, ICommandDecorator<TRequest> where TRequest : IRequest
{
    #region Private variables

    private readonly ICommandFeature<TRequest> _innerFeature;
    private readonly ILoggerFactory _loggerFactory;

    #endregion

    #region Constructor

    public CommandFeatureLogging(ICommandFeature<TRequest> innerFeature, ILoggerFactory loggerFactory)
    {
        _innerFeature = innerFeature;
        _loggerFactory = loggerFactory;
    }

    #endregion

    #region Methods

    public async Task ExecuteAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        Type innerMostFeatureType = this.GetInnerMostFeature().GetType();
        ILogger logger = _loggerFactory.CreateLogger(innerMostFeatureType);

        Guid requestId = request.RequestId;

        logger.LogDebug("Starting executing of command feature {FeatureNamespace}.{FeatureName} for request ID {RequestId}", innerMostFeatureType.Namespace, innerMostFeatureType.Name, requestId);
        try
        {
            await _innerFeature.ExecuteAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while executing of command feature {FeatureNamespace}.{FeatureName} for request ID {RequestId}: {Exception}", innerMostFeatureType.Namespace, innerMostFeatureType.Name, requestId, ex);
            throw;
        }
        finally
        {
            logger.LogDebug("Finishing executing of command feature {FeatureNamespace}.{FeatureName} for request ID {RequestId}", innerMostFeatureType.Namespace, innerMostFeatureType.Name, requestId);
        }
    }

    public ICommandFeature<TRequest> GetInnerFeature() => _innerFeature;

    #endregion
}