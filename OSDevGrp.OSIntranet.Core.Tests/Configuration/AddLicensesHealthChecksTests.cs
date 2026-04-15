using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.HealthChecks;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Core.Tests.Configuration;

[TestFixture]
public class AddLicensesHealthChecksTests
{
    #region Methods

    [Test]
    [Category("IntegrationTest")]
    public async Task AddLicensesHealthChecks_WhenCalled_ExpectConfigurationHealthCheckReturnsHealthStatusWhereStatusIsEqualToHealthy()
    {
        IConfiguration sut = CreateSut();

        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient(_ => CreateLoggerFactory());
        serviceCollection.AddHealthChecks()
            .AddLicensesHealthChecks(opt => 
            {
                opt.WithAutoMapperLicense(sut);
            });

        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        using IServiceScope serviceScope = serviceProvider.CreateScope();

        ConfigurationHealthCheck<LicensesHealthCheckOptions> configurationHealthCheck = new ConfigurationHealthCheck<LicensesHealthCheckOptions>(
            serviceScope.ServiceProvider.GetRequiredService<IOptions<LicensesHealthCheckOptions>>(),
            serviceScope.ServiceProvider.GetRequiredService<ILoggerFactory>());

        HealthCheckResult result = await configurationHealthCheck.CheckHealthAsync(new HealthCheckContext());

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Status, Is.EqualTo(HealthStatus.Healthy), result.Description);
    }

    private IConfiguration CreateSut()
    {
        return new ConfigurationBuilder()
            .AddUserSecrets<ConfigurationTests>()
            .Build();
    }

    private ILoggerFactory CreateLoggerFactory()
    {
        return NullLoggerFactory.Instance;
    }

    #endregion
}