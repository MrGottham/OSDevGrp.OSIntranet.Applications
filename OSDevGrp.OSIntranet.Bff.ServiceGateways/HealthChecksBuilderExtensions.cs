using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Options;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways;

public static class HealthChecksBuilderExtensions
{
    #region Methods

    public static IHealthChecksBuilder AddServiceGatewayHealthCheck(this IHealthChecksBuilder healthChecksBuilder)
    {
        return healthChecksBuilder.AddCheck<WepApiOptionsHealthCheck>(nameof(WepApiOptionsHealthCheck));
    }

    #endregion
}