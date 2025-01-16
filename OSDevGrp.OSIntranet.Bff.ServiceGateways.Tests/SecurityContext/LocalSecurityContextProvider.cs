using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests.SecurityContext;

internal class LocalSecurityContextProvider : ISecurityContextProvider
{
    #region Private variables

    private readonly IServiceProvider _serviceProvider;
    private readonly TimeProvider _timeProvider;
    private ISecurityContext? _securityContext;

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
            if (_securityContext != null && _securityContext.AccessToken.Expired == false)
            {
                return _securityContext;
            }

            ISecurityGateway securityGateway = _serviceProvider.GetRequiredService<ISecurityGateway>();
            AccessTokenModel accessTokenModel = await securityGateway.AquireTokenAsync(cancellationToken);

            IToken token = new LocalToken(accessTokenModel.Token_type, accessTokenModel.Access_token, _timeProvider.GetUtcNow().AddSeconds(accessTokenModel.Expires_in), _timeProvider);
            return _securityContext = new LocalSecurityContext(token);
        }
        finally
        {
            semaphore.Release();
        }
    }

    #endregion
}