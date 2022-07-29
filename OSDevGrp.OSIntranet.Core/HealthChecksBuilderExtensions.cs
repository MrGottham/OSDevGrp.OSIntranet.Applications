using System;
using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.Core.HealthChecks;

namespace OSDevGrp.OSIntranet.Core
{
    public static class HealthChecksBuilderExtensions
    {
        public static IHealthChecksBuilder AddSecurityHealthChecks(this IHealthChecksBuilder healthChecksBuilder, Action<SecurityHealthCheckOptions> configure)
        {
            NullGuard.NotNull(healthChecksBuilder, nameof(healthChecksBuilder))
                .NotNull(configure, nameof(configure));

            healthChecksBuilder.Services.Configure(configure);
            healthChecksBuilder.AddCheck<ConfigurationHealthCheck<SecurityHealthCheckOptions>>("SecurityConfigurationHealthCheck");

            return healthChecksBuilder;
        }
    }
}