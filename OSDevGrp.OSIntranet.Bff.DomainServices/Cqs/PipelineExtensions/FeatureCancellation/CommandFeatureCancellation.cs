using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Cqs.PipelineExtensions.FeatureCancellation;

internal class CommandFeatureCancellation<TRequest> : ICommandFeature<TRequest>, ICommandDecorator<TRequest> where TRequest : IRequest
{
    #region Private variables

    private readonly ICommandFeature<TRequest> _innerFeature;

    #endregion

    #region Constructor

    public CommandFeatureCancellation(ICommandFeature<TRequest> innerFeature)
    {
        _innerFeature = innerFeature;
    }

    #endregion

    #region Methods

    public Task ExecuteAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return _innerFeature.ExecuteAsync(request, cancellationToken);
    }

    public ICommandFeature<TRequest> GetInnerFeature() => _innerFeature;

    #endregion
}