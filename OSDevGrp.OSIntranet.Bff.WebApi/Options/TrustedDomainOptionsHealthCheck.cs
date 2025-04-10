using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Options;

internal class TrustedDomainOptionsHealthCheck : IHealthCheck
{
    #region Private variables

    private readonly IOptions<TrustedDomainOptions> _trustedDomainOptions;

    #endregion

    #region Constructor

    public TrustedDomainOptionsHealthCheck(IOptions<TrustedDomainOptions> trustedDomainOptions)
    {
        _trustedDomainOptions = trustedDomainOptions;
    }

    #endregion

    #region Methods

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.Run(() => 
        {
            TrustedDomainOptions trustedDomainOptions = _trustedDomainOptions.Value;

            if (string.IsNullOrWhiteSpace(trustedDomainOptions.TrustedDomainCollection))
            {
                return HealthCheckResult.Unhealthy($"{nameof(trustedDomainOptions.TrustedDomainCollection)} has not been given in the {trustedDomainOptions.GetType().Name}.");
            }

            IEnumerable<string> trustedDomains = trustedDomainOptions.AsTrustedDomains();
            if (trustedDomains.Any() == false)
            {
                return HealthCheckResult.Unhealthy($"{nameof(trustedDomainOptions.TrustedDomainCollection)} does not contain any values in the {trustedDomainOptions.GetType().Name}.");
            }

            return HealthCheckResult.Healthy();
        }, cancellationToken);
    }

    #endregion
}