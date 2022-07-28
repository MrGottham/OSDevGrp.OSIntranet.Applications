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

        public SecurityHealthCheckOptions WithMicrosoftValidation(IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            return AddStringConfigurationValidation(configuration, SecurityConfigurationKeys.MicrosoftClientId)
                .AddStringConfigurationValidation(configuration, SecurityConfigurationKeys.MicrosoftClientSecret)
                .AddStringConfigurationValidation(configuration, SecurityConfigurationKeys.MicrosoftTenant);
        }

        public SecurityHealthCheckOptions WithGoogleValidation(IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            return AddStringConfigurationValidation(configuration, SecurityConfigurationKeys.GoogleClientId)
                .AddStringConfigurationValidation(configuration, SecurityConfigurationKeys.GoogleClientSecret);
        }

        public SecurityHealthCheckOptions WithTrustedDomainCollectionValidation(IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            return AddStringCollectionConfigurationValidation(configuration, SecurityConfigurationKeys.TrustedDomainCollection, ";");
        }

        public SecurityHealthCheckOptions WithAcmeChallengeValidation(IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            return AddStringConfigurationValidation(configuration, SecurityConfigurationKeys.AcmeChallengeWellKnownChallengeToken)
                .AddStringConfigurationValidation(configuration, SecurityConfigurationKeys.AcmeChallengeConstructedKeyAuthorization);
        }

        #endregion
    }
}