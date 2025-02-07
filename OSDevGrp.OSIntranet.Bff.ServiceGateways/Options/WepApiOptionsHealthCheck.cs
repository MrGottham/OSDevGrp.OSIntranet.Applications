using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Options;

internal class WepApiOptionsHealthCheck : IHealthCheck
{
    #region Private variables

    private readonly IOptions<WebApiOptions> _webApiOptions;

    #endregion

    #region Constructor

    public WepApiOptionsHealthCheck(IOptions<WebApiOptions> webApiOptions)
    {        
        _webApiOptions = webApiOptions;
    }

    #endregion

    #region Methods

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.Run(() => 
        {
            WebApiOptions webApiOptions = _webApiOptions.Value;

            if (string.IsNullOrWhiteSpace(webApiOptions.EndpointAddress))
            {
                return new HealthCheckResult(HealthStatus.Unhealthy, $"{nameof(webApiOptions.EndpointAddress)} has not been given in the {webApiOptions.GetType().Name}.");
            }

            if (Uri.IsWellFormedUriString(webApiOptions.EndpointAddress, UriKind.Absolute) == false)
            {
                return new HealthCheckResult(HealthStatus.Unhealthy, $"{nameof(webApiOptions.EndpointAddress)} has not been given as a wellformed absolute URI in the {webApiOptions.GetType().Name}.");
            }

            if (string.IsNullOrWhiteSpace(webApiOptions.ClientId))
            {
                return new HealthCheckResult(HealthStatus.Unhealthy, $"{nameof(webApiOptions.ClientId)} has not been given in the {webApiOptions.GetType().Name}.");
            }

            if (string.IsNullOrWhiteSpace(webApiOptions.ClientSecret))
            {
                return new HealthCheckResult(HealthStatus.Unhealthy, $"{nameof(webApiOptions.ClientSecret)} has not been given in the {webApiOptions.GetType().Name}.");
            }

            return new HealthCheckResult(HealthStatus.Healthy);
        });
    }

    #endregion
}