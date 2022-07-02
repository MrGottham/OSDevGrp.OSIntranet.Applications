using System.Collections.Generic;
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

        #endregion
    }
}