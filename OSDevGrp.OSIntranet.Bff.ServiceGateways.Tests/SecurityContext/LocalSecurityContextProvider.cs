using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Runtime.Caching;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests.SecurityContext;

internal class LocalSecurityContextProvider : ISecurityContextProvider
{
    #region Private variables

    private readonly IServiceProvider _serviceProvider;
    private readonly TimeProvider _timeProvider;
    private readonly MemoryCache _memoryCache = MemoryCache.Default;

    #endregion

    #region Constructor

    public LocalSecurityContextProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _timeProvider = _serviceProvider.GetRequiredService<TimeProvider>();
    }

    #endregion

    #region Methods

    public async Task<ISecurityContext> GetCurrentSecurityContextAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        await semaphore.WaitAsync();
        try
        {
            ISecurityContext? securityContext = _memoryCache.Get(typeof(LocalSecurityContext).FullName) as ISecurityContext;
            if (securityContext != null && securityContext.AccessToken.Expired == false)
            {
                return securityContext;
            }

            ISecurityGateway securityGateway = _serviceProvider.GetRequiredService<ISecurityGateway>();
            AccessTokenModel accessTokenModel = await securityGateway.AquireTokenAsync(cancellationToken);

            ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(Array.Empty<Claim>()));
            IToken token = new LocalToken(accessTokenModel.Token_type, accessTokenModel.Access_token, _timeProvider.GetUtcNow().AddSeconds(accessTokenModel.Expires_in), _timeProvider);
            securityContext = new LocalSecurityContext(user, token);

            TimeProvider timeProvider = _serviceProvider.GetRequiredService<TimeProvider>();
            _memoryCache.Set(typeof(LocalSecurityContext).FullName, securityContext, new CacheItemPolicy {AbsoluteExpiration = timeProvider.GetUtcNow().AddSeconds(accessTokenModel.Expires_in)});

            return securityContext;
        }
        finally
        {
            semaphore.Release();
        }
    }

    #endregion
}