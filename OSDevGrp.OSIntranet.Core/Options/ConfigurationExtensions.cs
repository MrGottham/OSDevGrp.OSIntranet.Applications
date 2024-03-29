﻿using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core.Interfaces.Configuration;

namespace OSDevGrp.OSIntranet.Core.Options
{
    public static class ConfigurationExtensions
    {
        #region Methods

        internal static IConfigurationSection GetTrustedDomainSection(this IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            return configuration.GetSecuritySection();
        }

        internal static IConfigurationSection GetAcmeChallengeSection(this IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            return configuration.GetSection($"{SecurityConfigurationKeys.SecuritySectionName}:{SecurityConfigurationKeys.AcmeChallengeSectionName}");
        }

        private static IConfigurationSection GetSecuritySection(this IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            return configuration.GetSection($"{SecurityConfigurationKeys.SecuritySectionName}");
        }

        #endregion
    }
}