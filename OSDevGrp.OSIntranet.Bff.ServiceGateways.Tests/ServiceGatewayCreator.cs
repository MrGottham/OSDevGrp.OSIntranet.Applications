using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests.Options;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests.SecurityContext;

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
        serviceCollection.Configure<AccountingTestOptions>(configuration.GetSection("TestData:Accounting"));

        serviceCollection.AddSingleton(TimeProvider.System);
        serviceCollection.AddServiceGateways<LocalSecurityContextProvider>(configuration);

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

    public TimeProvider GetTimeProvider()
    {
        return _serviceScope.ServiceProvider.GetRequiredService<TimeProvider>();
    }

    public AccountingTestOptions GetAccountingTestOptions()
    {
        return _serviceScope.ServiceProvider.GetRequiredService<IOptions<AccountingTestOptions>>().Value;
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