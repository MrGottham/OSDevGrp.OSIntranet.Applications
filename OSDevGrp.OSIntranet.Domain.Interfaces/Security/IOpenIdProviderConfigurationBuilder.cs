using System;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IOpenIdProviderConfigurationBuilder
    {
        IOpenIdProviderConfigurationBuilder WithUserInfoEndpoint(Uri userInfoEndpoint);

        IOpenIdProviderConfigurationBuilder WithRegistrationEndpoint(Uri registrationEndpoint);

        IOpenIdProviderConfigurationBuilder WithScopesSupported(params string[] scopesSupported);

        IOpenIdProviderConfigurationBuilder WithResponseModesSupported(params string[] responseModesSupported);

        IOpenIdProviderConfigurationBuilder WithGrantTypesSupported(params string[] grantTypesSupported);

        IOpenIdProviderConfigurationBuilder WithAuthenticationContextClassReferencesSupported(params string[] authenticationContextClassReferencesSupported);

        IOpenIdProviderConfigurationBuilder WithIdTokenEncryptionAlgValuesSupported(params string[] idTokenEncryptionAlgValuesSupported);

        IOpenIdProviderConfigurationBuilder WithIdTokenEncryptionEncValuesSupported(params string[] idTokenEncryptionEncValuesSupported);

        IOpenIdProviderConfigurationBuilder WithUserInfoSigningAlgValuesSupported(params string[] userInfoSigningAlgValuesSupported);

        IOpenIdProviderConfigurationBuilder WithUserInfoEncryptionAlgValuesSupported(params string[] userInfoEncryptionAlgValuesSupported);

        IOpenIdProviderConfigurationBuilder WithUserInfoEncryptionEncValuesSupported(params string[] userInfoEncryptionEncValuesSupported);

        IOpenIdProviderConfigurationBuilder WithRequestObjectSigningAlgValuesSupported(params string[] requestObjectSigningAlgValuesSupported);

        IOpenIdProviderConfigurationBuilder WithRequestObjectEncryptionAlgValuesSupported(params string[] requestObjectEncryptionAlgValuesSupported);

        IOpenIdProviderConfigurationBuilder WithRequestObjectEncryptionEncValuesSupported(params string[] requestObjectEncryptionEncValuesSupported);

        IOpenIdProviderConfigurationBuilder WithTokenEndpointAuthenticationMethodsSupported(params string[] tokenEndpointAuthenticationMethodsSupported);

        IOpenIdProviderConfigurationBuilder WithTokenEndpointAuthenticationSigningAlgValuesSupported(params string[] tokenEndpointAuthenticationSigningAlgValuesSupported);

        IOpenIdProviderConfigurationBuilder WithDisplayValuesSupported(params string[] displayValuesSupported);

        IOpenIdProviderConfigurationBuilder WithClaimTypesSupported(params string[] claimTypesSupported);

        IOpenIdProviderConfigurationBuilder WithClaimsSupported(params string[] claimsSupported);

        IOpenIdProviderConfigurationBuilder WithServiceDocumentationEndpoint(Uri serviceDocumentationEndpoint);

        IOpenIdProviderConfigurationBuilder WithClaimsLocalesSupported(params string[] claimsLocalesSupported);

        IOpenIdProviderConfigurationBuilder WithUiLocalesSupported(params string[] uiLocalesSupported);

        IOpenIdProviderConfigurationBuilder WithClaimsParameterSupported(bool claimsParameterSupported);

        IOpenIdProviderConfigurationBuilder WithRequestParameterSupported(bool requestParameterSupported);

        IOpenIdProviderConfigurationBuilder WithRequestUriParameterSupported(bool requestUriParameterSupported);

        IOpenIdProviderConfigurationBuilder WithRequireRequestUriRegistration(bool requireRequestUriRegistration);

        IOpenIdProviderConfigurationBuilder WithRegistrationPolicyEndpoint(Uri registrationPolicyEndpoint);

        IOpenIdProviderConfigurationBuilder WithRegistrationTermsOfServiceEndpoint(Uri registrationTermsOfServiceEndpoint);

        IOpenIdProviderConfiguration Build();
    }
}