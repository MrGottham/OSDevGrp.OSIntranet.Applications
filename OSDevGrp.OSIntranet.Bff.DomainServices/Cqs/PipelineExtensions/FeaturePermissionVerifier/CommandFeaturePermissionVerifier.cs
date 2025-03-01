using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using System.Security;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Cqs.PipelineExtensions.FeaturePermissionVerifier;

internal class CommandFeaturePermissionVerifier<TRequest> : ICommandFeature<TRequest>, ICommandDecorator<TRequest> where TRequest : IRequest
{
    #region Private variables

    private readonly ICommandFeature<TRequest> _innerFeature;

    #endregion

    #region Constructor

    public CommandFeaturePermissionVerifier(ICommandFeature<TRequest> innerFeature)
    {
        _innerFeature = innerFeature;
    }

    #endregion

    #region Methods

    public async Task ExecuteAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        if (this.GetInnerMostFeature() is IPermissionVerifiable permissionVerifiable)
        {
            if (await permissionVerifiable.VerifyPermissionAsync(request.SecurityContext, request, cancellationToken) == false)
            {
                throw new SecurityException("Access denied due to insufficient privileges.");
            }
        }

        await _innerFeature.ExecuteAsync(request, cancellationToken);
    }

    public ICommandFeature<TRequest> GetInnerFeature() => _innerFeature;

    #endregion
}