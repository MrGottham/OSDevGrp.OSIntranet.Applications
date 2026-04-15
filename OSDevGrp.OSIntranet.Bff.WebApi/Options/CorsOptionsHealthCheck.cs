using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Options;

internal class CorsOptionsHealthCheck : IHealthCheck
{
    #region Properties

    private readonly IOptions<CorsOptions> _corsOptions;

    #endregion

    #region Constructor

    public CorsOptionsHealthCheck(IOptions<CorsOptions> corsOptions)
    {
        _corsOptions = corsOptions;
    }

    #endregion

    #region Methods

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.Run(() => 
        {
            CorsOptions corsOptions = _corsOptions.Value;

            if (string.IsNullOrWhiteSpace(corsOptions.OriginCollection))
            {
                return HealthCheckResult.Unhealthy($"{nameof(corsOptions.OriginCollection)} has not been given in the {corsOptions.GetType().Name}.");
            }

            IEnumerable<string> origins = corsOptions.AsOrigins();
            if (origins.Any() == false)
            {
                return HealthCheckResult.Unhealthy($"{nameof(corsOptions.OriginCollection)} does not contain any values in the {corsOptions.GetType().Name}.");
            }

            return HealthCheckResult.Healthy();
        }, cancellationToken);
    }

    #endregion
}