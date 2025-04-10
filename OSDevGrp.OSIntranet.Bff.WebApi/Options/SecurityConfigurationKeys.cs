namespace OSDevGrp.OSIntranet.Bff.WebApi.Options;

internal static class SecurityConfigurationKeys
{
    internal const string SecuritySectionName = "Security";
    internal const string OpenIdConnectSectionName = "OIDC";

    internal static readonly string OpenIdConnectAuthority = $"{SecuritySectionName}:{OpenIdConnectSectionName}:Authority";
    internal static readonly string OpenIdConnectClientId = $"{SecuritySectionName}:{OpenIdConnectSectionName}:ClientId";
    internal static readonly string OpenIdConnectClientSecret = $"{SecuritySectionName}:{OpenIdConnectSectionName}:ClientSecret";

    internal static readonly string TrustedDomainCollection = $"{SecuritySectionName}:TrustedDomainCollection";
}