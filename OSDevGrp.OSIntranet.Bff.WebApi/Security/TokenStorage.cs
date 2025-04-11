using Microsoft.Extensions.Caching.Memory;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Security;

internal class TokenStorage : ITokenStorage
{
    #region Private variables

    private readonly IMemoryCache _memoryCache;
    private readonly ITokenKeyProvider _tokenKeyProvider;
    private readonly ITokenProvider _tokenProvider;

    #endregion

    #region Constructor

    public TokenStorage(IMemoryCache memoryCache, ITokenKeyProvider tokenKeyProvider, ITokenProvider tokenProvider)
    {
        _memoryCache = memoryCache;
        _tokenKeyProvider = tokenKeyProvider;
        _tokenProvider = tokenProvider;
    }

    #endregion

    #region Methods

    public async Task<IToken> GetTokenAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default)
    {
        string tokenKey = await _tokenKeyProvider.ResolveAsync(user, cancellationToken);
        if (_memoryCache.TryGetValue(tokenKey, out IToken? token) && token != null && token.Expired == false)
        {
            return token;
        }

        IToken newToken = await _tokenProvider.ResolveAsync(user, cancellationToken);
        _memoryCache.Set(tokenKey, newToken, newToken.Expires);

        return newToken;
    }

    public async Task StoreTokenAsync(ClaimsPrincipal user, IToken token, CancellationToken cancellationToken = default)
    {
        string tokenKey = await _tokenKeyProvider.ResolveAsync(user, cancellationToken);

        _memoryCache.Set(tokenKey, token, token.Expires);
    }

    public async Task DeleteTokenAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default)
    {
        string tokenKey = await _tokenKeyProvider.ResolveAsync(user, cancellationToken);

        _memoryCache.Remove(tokenKey);
    }

    #endregion
}