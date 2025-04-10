namespace OSDevGrp.OSIntranet.Bff.WebApi.Options;

internal static class ConfigurationExtensions
{
    #region Methods

    internal static OpenIdConnectOptions? GetOpenIdConnectOptions(this IConfiguration configuration)
    {
        return configuration.GetOpenIdConnectSection().Get<OpenIdConnectOptions>();
    }

    internal static IConfigurationSection GetOpenIdConnectSection(this IConfiguration configuration)
    {
        return configuration.GetSection($"{SecurityConfigurationKeys.SecuritySectionName}:{SecurityConfigurationKeys.OpenIdConnectSectionName}");
    }

    internal static TrustedDomainOptions? GetTrustedDomainOptions(this IConfiguration configuration)
    {
        return configuration.GetTrustedDomainSection().Get<TrustedDomainOptions>();
    }

    internal static IConfigurationSection GetTrustedDomainSection(this IConfiguration configuration)
    {
        return configuration.GetSecuritySection();
    }

    private static IConfigurationSection GetSecuritySection(this IConfiguration configuration)
    {
        return configuration.GetSection($"{SecurityConfigurationKeys.SecuritySectionName}");
    }

    #endregion
}