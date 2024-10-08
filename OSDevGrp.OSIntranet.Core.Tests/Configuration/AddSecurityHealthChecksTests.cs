﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.HealthChecks;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Core.Tests.Configuration
{
    [TestFixture]
    public class AddSecurityHealthChecksTests
    {
        [Test]
        [Category("IntegrationTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task AddSecurityHealthChecks_WhenCalled_ExpectConfigurationHealthCheckReturnsHealthStatusWhereStatusIsEqualToHealthy(bool requireTenant)
        {
            IConfiguration sut = CreateSut();

            IServiceCollection serviceCollection = new ServiceCollection();
            // ReSharper disable UnusedParameter.Local
            serviceCollection.AddTransient(serviceProvider => CreateLoggerFactory());
            // ReSharper restore UnusedParameter.Local
            serviceCollection.AddHealthChecks()
                .AddSecurityHealthChecks(opt =>
                {
                    opt.WithJwtValidation(sut);
                    opt.WithMicrosoftValidation(sut, requireTenant);
                    opt.WithGoogleValidation(sut);
                    opt.WithTrustedDomainCollectionValidation(sut);
                    opt.WithAcmeChallengeValidation(sut);
                });

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            using IServiceScope serviceScope = serviceProvider.CreateScope();

            ConfigurationHealthCheck<SecurityHealthCheckOptions> configurationHealthCheck = new ConfigurationHealthCheck<SecurityHealthCheckOptions>(
                serviceScope.ServiceProvider.GetRequiredService<IOptions<SecurityHealthCheckOptions>>(),
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
    }
}