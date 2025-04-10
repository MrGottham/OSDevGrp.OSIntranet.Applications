using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Options;

internal class OpenIdConnectOptionsHealthCheck : IHealthCheck
{
    #region Private variables

    private readonly IOptions<OpenIdConnectOptions> _openIdConnectOptions;

    #endregion

    #region Constructor

    public OpenIdConnectOptionsHealthCheck(IOptions<OpenIdConnectOptions> webApiOptions)
    {        
        _openIdConnectOptions = webApiOptions;
    }

    #endregion

    #region Methods

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.Run(() => 
        {
            OpenIdConnectOptions openIdConnectOptions = _openIdConnectOptions.Value;

            if (string.IsNullOrWhiteSpace(openIdConnectOptions.Authority))
            {
                return new HealthCheckResult(HealthStatus.Unhealthy, $"{nameof(openIdConnectOptions.Authority)} has not been given in the {openIdConnectOptions.GetType().Name}.");
            }

            if (Uri.IsWellFormedUriString(openIdConnectOptions.Authority, UriKind.Absolute) == false)
            {
                return new HealthCheckResult(HealthStatus.Unhealthy, $"{nameof(openIdConnectOptions.Authority)} has not been given as a wellformed absolute URI in the {openIdConnectOptions.GetType().Name}.");
            }

            if (string.IsNullOrWhiteSpace(openIdConnectOptions.ClientId))
            {
                return new HealthCheckResult(HealthStatus.Unhealthy, $"{nameof(openIdConnectOptions.ClientId)} has not been given in the {openIdConnectOptions.GetType().Name}.");
            }

            if (string.IsNullOrWhiteSpace(openIdConnectOptions.ClientSecret))
            {
                return new HealthCheckResult(HealthStatus.Unhealthy, $"{nameof(openIdConnectOptions.ClientSecret)} has not been given in the {openIdConnectOptions.GetType().Name}.");
            }

            return new HealthCheckResult(HealthStatus.Healthy);
        }, cancellationToken);
    }

    #endregion
}