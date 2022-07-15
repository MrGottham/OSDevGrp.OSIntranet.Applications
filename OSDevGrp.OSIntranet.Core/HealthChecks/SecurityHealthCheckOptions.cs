using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core.Interfaces.Configuration;

namespace OSDevGrp.OSIntranet.Core.HealthChecks
{
    public class SecurityHealthCheckOptions : ConfigurationHealthCheckOptionsBase<SecurityHealthCheckOptions>
    {
        #region Properites

        protected override SecurityHealthCheckOptions HealthCheckOptions => this;

        #endregion

        #region Methods

        public SecurityHealthCheckOptions WithJwtValidation(IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            return AddRegularExpressionConfigurationValidation(configuration, SecurityConfigurationKeys.JwtKey, ConfigurationValueRegularExpressions.JwtKeyRegularExpression);
        }

        #endregion
    }
}