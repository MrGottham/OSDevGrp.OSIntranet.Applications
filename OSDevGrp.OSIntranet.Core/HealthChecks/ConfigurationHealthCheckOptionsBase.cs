using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core.Interfaces.HealthChecks;

namespace OSDevGrp.OSIntranet.Core.HealthChecks
{
    public abstract class ConfigurationHealthCheckOptionsBase<THealthCheckOptions> : HealthCheckOptionsBase where THealthCheckOptions : HealthCheckOptionsBase
    {
        #region Private variables

        private readonly IList<IConfigurationValueValidator> _configurationValueValidators = new List<IConfigurationValueValidator>();

        #endregion

        #region Properties

        public IEnumerable<IConfigurationValueValidator> ConfigurationValueValidators => _configurationValueValidators;

        protected abstract THealthCheckOptions HealthCheckOptions { get; } 

        #endregion

        #region Methods

        protected THealthCheckOptions AddConnectionStringValidation(IConfiguration configuration, string name)
        {
            NullGuard.NotNull(configuration, nameof(configuration))
                .NotNullOrWhiteSpace(name, nameof(name));

            _configurationValueValidators.Add(new ConnectionStringValidator(configuration, name));

            return HealthCheckOptions;
        }

        protected THealthCheckOptions AddStringConfigurationValidation(IConfiguration configuration, string key)
        {
            NullGuard.NotNull(configuration, nameof(configuration))
                .NotNullOrWhiteSpace(key, nameof(key));

            _configurationValueValidators.Add(new StringConfigurationValidator(configuration, key));

            return HealthCheckOptions;
        }

        protected THealthCheckOptions AddStringCollectionConfigurationValidation(IConfiguration configuration, string key, string separator, int minLength = 1, int maxLength = 32)
        {
            NullGuard.NotNull(configuration, nameof(configuration))
                .NotNullOrWhiteSpace(key, nameof(key))
                .NotNullOrWhiteSpace(separator, nameof(separator));

            _configurationValueValidators.Add(new StringCollectionConfigurationValidator(configuration, key, separator, minLength, maxLength));

            return HealthCheckOptions;
        }

        protected THealthCheckOptions AddRegularExpressionConfigurationValidation(IConfiguration configuration, string key, Regex regularExpression)
        {
            NullGuard.NotNull(configuration, nameof(configuration))
                .NotNullOrWhiteSpace(key, nameof(key))
                .NotNull(regularExpression, nameof(regularExpression));

            _configurationValueValidators.Add(new RegularExpressionConfigurationValidator(configuration, key, regularExpression));

            return HealthCheckOptions;
        }

        protected THealthCheckOptions AddEndpointConfigurationValidation(IConfiguration configuration, string key)
        {
            NullGuard.NotNull(configuration, nameof(configuration))
                .NotNullOrWhiteSpace(key, nameof(key));

            _configurationValueValidators.Add(new EndpointConfigurationValidator(configuration, key));

            return HealthCheckOptions;
        }

        #endregion
    }
}