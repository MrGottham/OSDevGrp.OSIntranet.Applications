using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using System.Security;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Cqs.PipelineExtensions.FeaturePermissionVerifier;

internal class QueryFeaturePermissionVerifier<TRequest, TResponse> : IQueryFeature<TRequest, TResponse>, IQueryDecorator<TRequest, TResponse> where TRequest : IRequest where TResponse : IResponse
{
    #region Private variables

    private readonly IQueryFeature<TRequest, TResponse> _innerFeature;

    #endregion

    #region Constructor

    public QueryFeaturePermissionVerifier(IQueryFeature<TRequest, TResponse> innerFeature)
    {
        _innerFeature = innerFeature;
    }

    #endregion

    #region Methods

    public async Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        if (this.GetInnerMostFeature() is IPermissionVerifiable permissionVerifiable)
        {
            if (await permissionVerifiable.VerifyPermissionAsync(request.SecurityContext, request, cancellationToken) == false)
            {
                throw new SecurityException("Access denied due to insufficient privileges.");
            }
        }

        return await _innerFeature.ExecuteAsync(request, cancellationToken);
    }

    public IQueryFeature<TRequest, TResponse> GetInnerFeature() => _innerFeature;

    #endregion
}