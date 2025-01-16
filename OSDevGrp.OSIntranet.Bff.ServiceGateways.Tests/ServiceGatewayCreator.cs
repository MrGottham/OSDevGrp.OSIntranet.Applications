using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests;

internal class ServiceGatewayCreator : IDisposable, IAsyncDisposable
{
    #region Private variables

    private readonly ServiceProvider _serviceProvider;
    private readonly IServiceScope _serviceScope;

    #endregion

    #region Constructor

    public ServiceGatewayCreator(IConfiguration configuration)
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddServiceGateways(configuration);

        _serviceProvider = serviceCollection.BuildServiceProvider();
        _serviceScope = _serviceProvider.CreateScope();
    }

    #endregion

    #region Methods

    public void Dispose()
    {
        _serviceScope.Dispose();
        _serviceProvider.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        _serviceScope.Dispose();
        await _serviceProvider.DisposeAsync();
    }

    public IAccountingGateway CreateAccountingGateway()
    {
        return _serviceScope.ServiceProvider.GetRequiredService<IAccountingGateway>();
    }

    public ICommonGateway CreateCommonGateway()
    {
        return _serviceScope.ServiceProvider.GetRequiredService<ICommonGateway>();
    }

    public ISecurityGateway CreateSecurityGateway()
    {
        return _serviceScope.ServiceProvider.GetRequiredService<ISecurityGateway>();
    }

    #endregion
}