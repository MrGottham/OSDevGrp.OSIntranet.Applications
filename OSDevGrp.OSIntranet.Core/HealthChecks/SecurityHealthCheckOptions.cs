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

            return AddRegularExpressionConfigurationValidation(configuration, SecurityConfigurationKeys.JwtKeyKty, ConfigurationValueRegularExpressions.JwtKeyTypeRegularExpression)
                .AddRegularExpressionConfigurationValidation(configuration, SecurityConfigurationKeys.JwtKeyN, ConfigurationValueRegularExpressions.JwtKeyBase64UrlRegularExpression)
                .AddRegularExpressionConfigurationValidation(configuration, SecurityConfigurationKeys.JwtKeyE, ConfigurationValueRegularExpressions.JwtKeyBase64UrlRegularExpression)
                .AddRegularExpressionConfigurationValidation(configuration, SecurityConfigurationKeys.JwtKeyD, ConfigurationValueRegularExpressions.JwtKeyBase64UrlRegularExpression)
                .AddRegularExpressionConfigurationValidation(configuration, SecurityConfigurationKeys.JwtKeyDp, ConfigurationValueRegularExpressions.JwtKeyBase64UrlRegularExpression)
                .AddRegularExpressionConfigurationValidation(configuration, SecurityConfigurationKeys.JwtKeyDq, ConfigurationValueRegularExpressions.JwtKeyBase64UrlRegularExpression)
                .AddRegularExpressionConfigurationValidation(configuration, SecurityConfigurationKeys.JwtKeyP, ConfigurationValueRegularExpressions.JwtKeyBase64UrlRegularExpression)
                .AddRegularExpressionConfigurationValidation(configuration, SecurityConfigurationKeys.JwtKeyQ, ConfigurationValueRegularExpressions.JwtKeyBase64UrlRegularExpression)
                .AddRegularExpressionConfigurationValidation(configuration, SecurityConfigurationKeys.JwtKeyQi, ConfigurationValueRegularExpressions.JwtKeyBase64UrlRegularExpression)
                .AddEndpointConfigurationValidation(configuration, SecurityConfigurationKeys.JwtIssuer)
                .AddEndpointConfigurationValidation(configuration, SecurityConfigurationKeys.JwtAudience);
        }

        public SecurityHealthCheckOptions WithOpenIdConnect(IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            return AddEndpointConfigurationValidation(configuration, SecurityConfigurationKeys.OpenIdConnectAuthority)
                .AddStringConfigurationValidation(configuration, SecurityConfigurationKeys.OpenIdConnectClientId)
                .AddStringConfigurationValidation(configuration, SecurityConfigurationKeys.OpenIdConnectClientSecret);

        }

        public SecurityHealthCheckOptions WithMicrosoftValidation(IConfiguration configuration, bool requireTenant)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            SecurityHealthCheckOptions options = AddStringConfigurationValidation(configuration, SecurityConfigurationKeys.MicrosoftClientId)
                .AddStringConfigurationValidation(configuration, SecurityConfigurationKeys.MicrosoftClientSecret);

            return requireTenant
                ? options.AddStringConfigurationValidation(configuration, SecurityConfigurationKeys.MicrosoftTenant)
                : options;
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