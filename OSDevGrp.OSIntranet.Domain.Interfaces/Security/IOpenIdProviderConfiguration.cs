using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IOpenIdProviderConfiguration
    {
        Uri Issuer { get; }

        Uri AuthorizationEndpoint { get; }

        Uri TokenEndpoint { get; }

        Uri UserInfoEndpoint { get; }

        Uri JsonWebKeySetEndpoint { get; }

        Uri RegistrationEndpoint { get; }

        IEnumerable<string> ScopesSupported { get; }

        IEnumerable<string> ResponseTypesSupported { get; }

        IEnumerable<string> ResponseModesSupported { get; }

        IEnumerable<string> GrantTypesSupported { get; }

        IEnumerable<string> AuthenticationContextClassReferencesSupported { get; }

        IEnumerable<string> SubjectTypesSupported { get; }

        IEnumerable<string> IdTokenSigningAlgValuesSupported { get; }

        IEnumerable<string> IdTokenEncryptionAlgValuesSupported { get; }

        IEnumerable<string> IdTokenEncryptionEncValuesSupported { get; }

        IEnumerable<string> UserInfoSigningAlgValuesSupported { get; }

        IEnumerable<string> UserInfoEncryptionAlgValuesSupported { get; }

        IEnumerable<string> UserInfoEncryptionEncValuesSupported { get; }

        IEnumerable<string> RequestObjectSigningAlgValuesSupported { get; }

        IEnumerable<string> RequestObjectEncryptionAlgValuesSupported { get; }

        IEnumerable<string> RequestObjectEncryptionEncValuesSupported { get; }

        IEnumerable<string> TokenEndpointAuthenticationMethodsSupported { get; }

        IEnumerable<string> TokenEndpointAuthenticationSigningAlgValuesSupported { get; }

        IEnumerable<string> DisplayValuesSupported { get; }

        IEnumerable<string> ClaimTypesSupported { get; }

        IEnumerable<string> ClaimsSupported { get; }

        Uri ServiceDocumentationEndpoint { get; }

        IEnumerable<string> ClaimsLocalesSupported { get; }

        IEnumerable<string> UiLocalesSupported { get; }

        bool? ClaimsParameterSupported { get; }

        bool? RequestParameterSupported { get; }

        bool? RequestUriParameterSupported { get; }

        bool? RequireRequestUriRegistration { get; }

        Uri RegistrationPolicyEndpoint { get; }

        Uri RegistrationTermsOfServiceEndpoint { get; }
    }
}