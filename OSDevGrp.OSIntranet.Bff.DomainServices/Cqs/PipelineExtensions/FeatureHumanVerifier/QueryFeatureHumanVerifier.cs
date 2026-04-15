using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Cqs.PipelineExtensions.FeatureHumanVerifier;

internal class QueryFeatureHumanVerifier<TRequest, TResponse> : IQueryFeature<TRequest, TResponse>, IQueryDecorator<TRequest, TResponse> where TRequest : IRequest where TResponse : IResponse
{
    #region Private variables

    private readonly IQueryFeature<TRequest, TResponse> _innerFeature;
    private readonly IVerificationCodeVerifier _verificationCodeVerifier;
    private readonly IVerificationCodeStorage _verificationCodeStorage;

    #endregion

    #region Constructor

    public QueryFeatureHumanVerifier(IQueryFeature<TRequest, TResponse> innerFeature, IVerificationCodeVerifier verificationCodeVerifier, IVerificationCodeStorage verificationCodeStorage)
    {
        _innerFeature = innerFeature;
        _verificationCodeVerifier = verificationCodeVerifier;
        _verificationCodeStorage = verificationCodeStorage;
    }

    #endregion

    #region Methods

    public async Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        if (request is not IHumanVerifiableRequest humanVerifiableRequest)
        {
            return await _innerFeature.ExecuteAsync(request, cancellationToken);
        }

        string verificationKey = humanVerifiableRequest.VerificationKey;
        string verificationCode = humanVerifiableRequest.VerificationCode;
        try
        {
            if (await _verificationCodeVerifier.VerifyAsync(verificationKey, verificationCode, cancellationToken) == false)
            {
                throw new VerificationFailedException();
            }

            return await _innerFeature.ExecuteAsync(request, cancellationToken);
        }
        finally
        {
            await _verificationCodeStorage.RemoveVerificationCodeAsync(verificationKey, cancellationToken);
        }
    }

    public IQueryFeature<TRequest, TResponse> GetInnerFeature() => _innerFeature;

    #endregion
}