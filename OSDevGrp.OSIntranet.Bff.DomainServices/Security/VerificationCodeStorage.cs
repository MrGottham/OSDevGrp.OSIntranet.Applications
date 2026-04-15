using Microsoft.Extensions.Caching.Memory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Security;

internal class VerificationCodeStorage : IVerificationCodeStorage
{
    #region Private variables

    private readonly IMemoryCache _memoryCache;

    #endregion

    #region Constructor

    public VerificationCodeStorage(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    #endregion

    #region Methods

    public Task<string?> GetVerificationCodeAsync(string verificationKey, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => _memoryCache.Get<string?>(verificationKey), cancellationToken);
    }

    public Task StoreVerificationCodeAsync(string verificationKey, string verificationCode, DateTimeOffset expires, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => _memoryCache.Set(verificationKey, verificationCode, expires), cancellationToken);
    }

    public Task RemoveVerificationCodeAsync(string verificationKey, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => _memoryCache.Remove(verificationKey), cancellationToken);
    }

    #endregion
}