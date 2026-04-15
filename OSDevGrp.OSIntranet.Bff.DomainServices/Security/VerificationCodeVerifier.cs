using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Security;

internal class VerificationCodeVerifier : IVerificationCodeVerifier
{
    #region Private variables

    private readonly IVerificationCodeStorage _verificationCodeStorage;

    #endregion

    #region Constructor

    public VerificationCodeVerifier(IVerificationCodeStorage verificationCodeStorage)
    {
        _verificationCodeStorage = verificationCodeStorage;
    }

    #endregion

    #region Methods

    public async Task<bool> VerifyAsync(string verificationKey, string verificationCode, CancellationToken cancellationToken = default)
    {
        string? storedVerificationCode = await _verificationCodeStorage.GetVerificationCodeAsync(verificationKey, cancellationToken);
        if (string.IsNullOrWhiteSpace(storedVerificationCode))
        {
            return false;
        }

        return storedVerificationCode == verificationCode;
    }

    #endregion
}