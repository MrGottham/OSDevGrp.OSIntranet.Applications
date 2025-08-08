using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Cqs.PipelineExtensions.FeatureHumanVerifier;

internal class CommandFeatureHumanVerifier<TRequest> : ICommandFeature<TRequest>, ICommandDecorator<TRequest> where TRequest : IRequest
{
    #region Private variables

    private readonly ICommandFeature<TRequest> _innerFeature;
    private readonly IVerificationCodeVerifier _verificationCodeVerifier;
    private readonly IVerificationCodeStorage _verificationCodeStorage;

    #endregion

    #region Constructor

    public CommandFeatureHumanVerifier(ICommandFeature<TRequest> innerFeature, IVerificationCodeVerifier verificationCodeVerifier, IVerificationCodeStorage verificationCodeStorage)
    {
        _innerFeature = innerFeature;
        _verificationCodeVerifier = verificationCodeVerifier;
        _verificationCodeStorage = verificationCodeStorage;
    }

    #endregion

    #region Methods

    public async Task ExecuteAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        if (request is not IHumanVerifiableRequest humanVerifiableRequest)
        {
            await _innerFeature.ExecuteAsync(request, cancellationToken);
            return;
        }

        string verificationKey = humanVerifiableRequest.VerificationKey;
        string verificationCode = humanVerifiableRequest.VerificationCode;
        try
        {
            if (await _verificationCodeVerifier.VerifyAsync(verificationKey, verificationCode, cancellationToken) == false)
            {
                throw new VerificationFailedException();
            }

            await _innerFeature.ExecuteAsync(request, cancellationToken);
        }
        finally
        {
            await _verificationCodeStorage.RemoveVerificationCodeAsync(verificationKey, cancellationToken);
        }
    }

    public ICommandFeature<TRequest> GetInnerFeature() => _innerFeature;

    #endregion
}