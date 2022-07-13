using System;
using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.HealthChecks;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories
{
    public static class HealthChecksBuilderExtensions
    {
        public static IHealthChecksBuilder AddRepositoryHealthChecks(this IHealthChecksBuilder healthChecksBuilder, Action<RepositoryHealthCheckOptions> configure)
        {
            NullGuard.NotNull(healthChecksBuilder, nameof(healthChecksBuilder))
                .NotNull(configure, nameof(configure));

            healthChecksBuilder.Services.Configure(configure);
            healthChecksBuilder.AddCheck<ConfigurationHealthCheck<RepositoryHealthCheckOptions>>("RepositoryConfigurationHealthCheck");

            RepositoryHealthCheckOptions repositoryHealthCheckOptions = new RepositoryHealthCheckOptions();
            configure(repositoryHealthCheckOptions);
            if (repositoryHealthCheckOptions.ValidateRepositoryContext)
            {
                healthChecksBuilder.AddDbContextCheck<RepositoryContext>();
            }

            return healthChecksBuilder;
        }
    }
}