using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.HealthChecks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.Configuration
{
    [TestFixture]
    public class AddRepositoryHealthChecksTests : ConfigurationTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task AddRepositoryHealthChecks_WhenCalled_ExpectConfigurationHealthCheckReturnsHealthStatusWhereStatusIsEqualToHealthy()
        {
            IConfiguration sut = CreateSut();

            IServiceCollection serviceCollection = new ServiceCollection();
            // ReSharper disable UnusedParameter.Local
            serviceCollection.AddTransient(serviceProvider => CreateLoggerFactory());
            // ReSharper restore UnusedParameter.Local
            serviceCollection.AddHealthChecks()
                .AddRepositoryHealthChecks(opt =>
                {
                    opt.WithRepositoryContextValidation();
                    opt.WithConnectionStringsValidation(sut);
                    opt.WithExternalDataDashboardValidation(sut);
                });

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            using IServiceScope serviceScope = serviceProvider.CreateScope();

            ConfigurationHealthCheck<Repositories.RepositoryHealthCheckOptions> configurationHealthCheck = new ConfigurationHealthCheck<Repositories.RepositoryHealthCheckOptions>(
                serviceScope.ServiceProvider.GetRequiredService<IOptions<Repositories.RepositoryHealthCheckOptions>>(),
                serviceScope.ServiceProvider.GetRequiredService<ILoggerFactory>());

            HealthCheckResult result = await configurationHealthCheck.CheckHealthAsync(new HealthCheckContext());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(HealthStatus.Healthy), result.Description);
        }
    }
}