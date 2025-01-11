using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Configuration;
using OSDevGrp.OSIntranet.Repositories.Interfaces.Configuration;

namespace OSDevGrp.OSIntranet.Repositories.Options
{
    public static class ConfigurationExtensions
    {
        #region Methods

        public static OpenIdConnectOptions GetOpenIdConnectOptions(this IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            return configuration.GetOpenIdConnectSection().Get<OpenIdConnectOptions>();
        }

        public static MicrosoftSecurityOptions GetMicrosoftSecurityOptions(this IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            return configuration.GetMicrosoftSecuritySection().Get<MicrosoftSecurityOptions>();
        }

        public static GoogleSecurityOptions GetGoogleSecurityOptions(this IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            return configuration.GetGoogleSecuritySection().Get<GoogleSecurityOptions>();
        }

        internal static ExternalDashboardOptions GetExternalDashboardOptions(this IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            return configuration.GetExternalDashboardSection().Get<ExternalDashboardOptions>();
        }

        internal static IConfigurationSection GetOpenIdConnectSection(this IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            return configuration.GetSection($"{SecurityConfigurationKeys.SecuritySectionName}:{SecurityConfigurationKeys.OpenIdConnectSectionName}");
        }

        internal static IConfigurationSection GetMicrosoftSecuritySection(this IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            return configuration.GetSection($"{SecurityConfigurationKeys.SecuritySectionName}:{SecurityConfigurationKeys.MicrosoftSectionName}");
        }

        internal static IConfigurationSection GetGoogleSecuritySection(this IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            return configuration.GetSection($"{SecurityConfigurationKeys.SecuritySectionName}:{SecurityConfigurationKeys.GoogleSectionName}");
        }

        internal static IConfigurationSection GetExternalDashboardSection(this IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            return configuration.GetSection($"{ExternalDataConfigurationKeys.ExternalDataSectionName}:{ExternalDataConfigurationKeys.DashboardSectionName}");
        }

        #endregion
    }
}