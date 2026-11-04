using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Cqs.PipelineExtensions.FeatureHumanVerifier;

internal class CommandFeatureHumanVerifier<TRequest> : ICommandFeature<TRequest>, ICommandDecorator<TRequest> where TRequest : IRequest
{
    #region Private variables

    private readonly ICommandFeature<TRequest> _innerFeature;
    private readonly IVerificationCodeVerifier _verificationCodeVerifier;
    private readonly IVerificationCodeStorage _verificationCodeStorage;
    private readonly IStaticTextProvider _staticTextProvider;

    #endregion

    #region Constructor

    public CommandFeatureHumanVerifier(ICommandFeature<TRequest> innerFeature, IVerificationCodeVerifier verificationCodeVerifier, IVerificationCodeStorage verificationCodeStorage, IStaticTextProvider staticTextProvider)
    {
        _innerFeature = innerFeature;
        _verificationCodeVerifier = verificationCodeVerifier;
        _verificationCodeStorage = verificationCodeStorage;
        _staticTextProvider = staticTextProvider;
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
                string message = await _staticTextProvider.GetStaticTextAsync(StaticTextKey.VerificationFailed, Array.Empty<object>(), humanVerifiableRequest.FormatProvider, cancellationToken);
                throw new VerificationFailedException(message);
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