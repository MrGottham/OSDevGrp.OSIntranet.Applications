using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Configuration;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Options
{
    public static class ConfigurationExtensions
    {
        #region Methods

        public static TokenGeneratorOptions GetTokenGeneratorOptions(this IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            return GetTokenGeneratorSection(configuration).Get<TokenGeneratorOptions>();
        }

        internal static IConfigurationSection GetTokenGeneratorSection(this IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            return configuration.GetSection($"{SecurityConfigurationKeys.SecuritySectionName}:{SecurityConfigurationKeys.JwtSectionName}");
        }

        #endregion
    }
}