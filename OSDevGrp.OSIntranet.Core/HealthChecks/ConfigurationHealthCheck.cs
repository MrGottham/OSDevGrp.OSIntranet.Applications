using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core.Interfaces.HealthChecks;

namespace OSDevGrp.OSIntranet.Core.HealthChecks
{
    public class ConfigurationHealthCheck<TConfigurationHealthCheckOptions> : IHealthCheck where TConfigurationHealthCheckOptions : ConfigurationHealthCheckOptionsBase<TConfigurationHealthCheckOptions>
    {
        #region Private variables

        private readonly IOptions<TConfigurationHealthCheckOptions> _options;
        private readonly ILoggerFactory _loggerFactory;

        #endregion

        #region Constructor

        public ConfigurationHealthCheck(IOptions<TConfigurationHealthCheckOptions> options, ILoggerFactory loggerFactory)
        {
            NullGuard.NotNull(options, nameof(options))
                .NotNull(loggerFactory, nameof(loggerFactory));

            _options = options;
            _loggerFactory = loggerFactory;
        }

        #endregion

        #region Methods

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            NullGuard.NotNull(context, nameof(context));

            HealthCheckResult[] healthCheckResultCollection = await Task.WhenAll(_options.Value.ConfigurationValueValidators
                .Select(CheckHealthAsync)
                .ToArray());

            return BuildHealthCheckResult(healthCheckResultCollection);
        }

        private async Task<HealthCheckResult> CheckHealthAsync(IConfigurationValueValidator configurationValueValidator)
        {
            NullGuard.NotNull(configurationValueValidator, nameof(configurationValueValidator));

            try
            {
                await configurationValueValidator.ValidateAsync();

                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                ILogger logger = _loggerFactory.CreateLogger(GetType());
                logger.LogError(ex, ex.Message);

                return HealthCheckResult.Unhealthy(ex.Message, ex);
            }
        }

        private static HealthCheckResult BuildHealthCheckResult(HealthCheckResult[] healthCheckResultCollection)
        {
            NullGuard.NotNull(healthCheckResultCollection, nameof(healthCheckResultCollection));

            if (healthCheckResultCollection.Length == 0 || healthCheckResultCollection.All(healthCheckResult => healthCheckResult.Status == HealthStatus.Healthy))
            {
                return HealthCheckResult.Healthy();
            }

            StringBuilder descriptionBuilder = new StringBuilder();
            foreach (HealthCheckResult healthCheckResult in healthCheckResultCollection.Where(m => m.Status != HealthStatus.Healthy && string.IsNullOrWhiteSpace(m.Description) == false))
            {
                descriptionBuilder.AppendLine(healthCheckResult.Description);
            }

            return HealthCheckResult.Unhealthy(descriptionBuilder.ToString());
        }

        #endregion
    }
}