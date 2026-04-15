using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.Core.HealthChecks;
using System;

namespace OSDevGrp.OSIntranet.Core;

public static class HealthChecksBuilderExtensions
{
    #region Methods

    public static IHealthChecksBuilder AddSecurityHealthChecks(this IHealthChecksBuilder healthChecksBuilder, Action<SecurityHealthCheckOptions> configure)
    {
        NullGuard.NotNull(healthChecksBuilder, nameof(healthChecksBuilder))
            .NotNull(configure, nameof(configure));

        healthChecksBuilder.Services.Configure(configure);
        healthChecksBuilder.AddCheck<ConfigurationHealthCheck<SecurityHealthCheckOptions>>("SecurityConfigurationHealthCheck");

        return healthChecksBuilder;
    }

    public static IHealthChecksBuilder AddLicensesHealthChecks(this IHealthChecksBuilder healthChecksBuilder, Action<LicensesHealthCheckOptions> configure)
    {
        NullGuard.NotNull(healthChecksBuilder, nameof(healthChecksBuilder))
            .NotNull(configure, nameof(configure));

        healthChecksBuilder.Services.Configure(configure);
        healthChecksBuilder.AddCheck<ConfigurationHealthCheck<LicensesHealthCheckOptions>>("LicensesConfigurationHealthCheck");

        return healthChecksBuilder;
    }

    #endregion
}