using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using System.Security.Cryptography;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Security;

internal class VerificationCodeGenerator : IVerificationCodeGenerator
{
    #region Private variables

    private static readonly char[] _characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

    #endregion

    #region Methods

    public Task<string> GenerateAsync(CancellationToken cancellationToken = default)
    {
        return Task.Run(() => RandomNumberGenerator.GetString(_characters, 6), cancellationToken);
    }

    #endregion
}