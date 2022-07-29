using System.Collections.Generic;
using OSDevGrp.OSIntranet.Core.HealthChecks;
using OSDevGrp.OSIntranet.Core.Interfaces.HealthChecks;

namespace OSDevGrp.OSIntranet.Core.Tests.HealthChecks.ConfigurationHealthCheck
{
    public class ConfigurationHealthCheckTestOptions : ConfigurationHealthCheckOptionsBase<ConfigurationHealthCheckTestOptions>
    {
        #region Consturctor

        public ConfigurationHealthCheckTestOptions(params IConfigurationValueValidator[] configurationValueValidatorCollection)
        {
            Core.NullGuard.NotNull(configurationValueValidatorCollection, nameof(configurationValueValidatorCollection));

            foreach (IConfigurationValueValidator configurationValueValidator in configurationValueValidatorCollection)
            {
                ((IList<IConfigurationValueValidator>)ConfigurationValueValidators).Add(configurationValueValidator);
            }
        }

        #endregion

        #region Properties

        protected override ConfigurationHealthCheckTestOptions HealthCheckOptions => this;

        #endregion
    }
}