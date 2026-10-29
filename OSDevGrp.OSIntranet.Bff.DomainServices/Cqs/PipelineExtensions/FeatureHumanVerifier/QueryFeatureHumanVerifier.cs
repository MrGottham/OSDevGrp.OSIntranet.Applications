using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Cqs.PipelineExtensions.FeatureHumanVerifier;

internal class QueryFeatureHumanVerifier<TRequest, TResponse> : IQueryFeature<TRequest, TResponse>, IQueryDecorator<TRequest, TResponse> where TRequest : IRequest where TResponse : IResponse
{
    #region Private variables

    private readonly IQueryFeature<TRequest, TResponse> _innerFeature;
    private readonly IVerificationCodeVerifier _verificationCodeVerifier;
    private readonly IVerificationCodeStorage _verificationCodeStorage;
    private readonly IStaticTextProvider _staticTextProvider;

    #endregion

    #region Constructor

    public QueryFeatureHumanVerifier(IQueryFeature<TRequest, TResponse> innerFeature, IVerificationCodeVerifier verificationCodeVerifier, IVerificationCodeStorage verificationCodeStorage, IStaticTextProvider staticTextProvider)
    {
        _innerFeature = innerFeature;
        _verificationCodeVerifier = verificationCodeVerifier;
        _verificationCodeStorage = verificationCodeStorage;
        _staticTextProvider = staticTextProvider;
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
                string message = await _staticTextProvider.GetStaticTextAsync(StaticTextKey.VerificationFailed, Array.Empty<object>(), humanVerifiableRequest.FormatProvider, cancellationToken);
                throw new VerificationFailedException(message);
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