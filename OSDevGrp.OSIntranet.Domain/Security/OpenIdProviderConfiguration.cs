using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    internal class OpenIdProviderConfiguration : IOpenIdProviderConfiguration
    {
        #region Constructor

        public OpenIdProviderConfiguration(Uri issuer, Uri authorizationEndpoint, Uri tokenEndpoint, Uri userInfoEndpoint, Uri jsonWebKeySetEndpoint, Uri registrationEndpoint, string[] scopesSupported, string[] responseTypesSupported, string[] responseModesSupported, string[] grantTypesSupported, string[] authenticationContextClassReferencesSupported, string[] subjectTypesSupported, string[] idTokenSigningAlgValuesSupported, string[] idTokenEncryptionAlgValuesSupported, string[] idTokenEncryptionEncValuesSupported, string[] userInfoSigningAlgValuesSupported, string[] userInfoEncryptionAlgValuesSupported, string[] userInfoEncryptionEncValuesSupported, string[] requestObjectSigningAlgValuesSupported, string[] requestObjectEncryptionAlgValuesSupported, string[] requestObjectEncryptionEncValuesSupported, string[] tokenEndpointAuthenticationMethodsSupported, string[] tokenEndpointAuthenticationSigningAlgValuesSupported, string[] displayValuesSupported, string[] claimTypesSupported, string[] claimsSupported, Uri serviceDocumentationEndpoint, string[] claimsLocalesSupported, string[] uiLocalesSupported, bool? claimsParameterSupported, bool? requestParameterSupported, bool? requestUriParameterSupported, bool? requireRequestUriRegistration, Uri registrationPolicyEndpoint, Uri registrationTermsOfServiceEndpoint)
        {
            NullGuard.NotNull(issuer, nameof(issuer))
                .NotNull(authorizationEndpoint, nameof(authorizationEndpoint))
                .NotNull(tokenEndpoint, nameof(tokenEndpoint))
                .NotNull(jsonWebKeySetEndpoint, nameof(jsonWebKeySetEndpoint))
                .NotNull(responseTypesSupported, nameof(responseTypesSupported))
                .NotNull(subjectTypesSupported, nameof(subjectTypesSupported))
                .NotNull(idTokenSigningAlgValuesSupported, nameof(idTokenSigningAlgValuesSupported));

            Issuer = issuer;
            AuthorizationEndpoint = authorizationEndpoint;
            TokenEndpoint = tokenEndpoint;
            UserInfoEndpoint = userInfoEndpoint;
            JsonWebKeySetEndpoint = jsonWebKeySetEndpoint;
            RegistrationEndpoint = registrationEndpoint;
            ScopesSupported = scopesSupported;
            ResponseTypesSupported = responseTypesSupported;
            ResponseModesSupported = responseModesSupported;
            GrantTypesSupported = grantTypesSupported;
            AuthenticationContextClassReferencesSupported = authenticationContextClassReferencesSupported;
            SubjectTypesSupported = subjectTypesSupported;
            IdTokenSigningAlgValuesSupported = idTokenSigningAlgValuesSupported;
            IdTokenEncryptionAlgValuesSupported = idTokenEncryptionAlgValuesSupported;
            IdTokenEncryptionEncValuesSupported = idTokenEncryptionEncValuesSupported;
            UserInfoSigningAlgValuesSupported = userInfoSigningAlgValuesSupported;
            UserInfoEncryptionAlgValuesSupported = userInfoEncryptionAlgValuesSupported;
            UserInfoEncryptionEncValuesSupported = userInfoEncryptionEncValuesSupported;
            RequestObjectSigningAlgValuesSupported = requestObjectSigningAlgValuesSupported;
            RequestObjectEncryptionAlgValuesSupported = requestObjectEncryptionAlgValuesSupported;
            RequestObjectEncryptionEncValuesSupported = requestObjectEncryptionEncValuesSupported;
            TokenEndpointAuthenticationMethodsSupported = tokenEndpointAuthenticationMethodsSupported;
            TokenEndpointAuthenticationSigningAlgValuesSupported = tokenEndpointAuthenticationSigningAlgValuesSupported;
            DisplayValuesSupported = displayValuesSupported;
            ClaimTypesSupported = claimTypesSupported;
            ClaimsSupported = claimsSupported;
            ServiceDocumentationEndpoint = serviceDocumentationEndpoint;
            ClaimsLocalesSupported = claimsLocalesSupported;
            UiLocalesSupported = uiLocalesSupported;
            ClaimsParameterSupported = claimsParameterSupported;
            RequestParameterSupported = requestParameterSupported;
            RequestUriParameterSupported = requestUriParameterSupported;
            RequireRequestUriRegistration = requireRequestUriRegistration;
            RegistrationPolicyEndpoint = registrationPolicyEndpoint;
            RegistrationTermsOfServiceEndpoint = registrationTermsOfServiceEndpoint;
        }

        #endregion

        #region Properties

        public Uri Issuer { get; }

        public Uri AuthorizationEndpoint { get; }

        public Uri TokenEndpoint { get; }

        public Uri UserInfoEndpoint { get; }

        public Uri JsonWebKeySetEndpoint { get; }

        public Uri RegistrationEndpoint { get; }

        public IEnumerable<string> ScopesSupported { get; }

        public IEnumerable<string> ResponseTypesSupported { get; }

        public IEnumerable<string> ResponseModesSupported { get; }

        public IEnumerable<string> GrantTypesSupported { get; }

        public IEnumerable<string> AuthenticationContextClassReferencesSupported { get; }

        public IEnumerable<string> SubjectTypesSupported { get; }

        public IEnumerable<string> IdTokenSigningAlgValuesSupported { get; }

        public IEnumerable<string> IdTokenEncryptionAlgValuesSupported { get; }

        public IEnumerable<string> IdTokenEncryptionEncValuesSupported { get; }

        public IEnumerable<string> UserInfoSigningAlgValuesSupported { get; }

        public IEnumerable<string> UserInfoEncryptionAlgValuesSupported { get; }

        public IEnumerable<string> UserInfoEncryptionEncValuesSupported { get; }

        public IEnumerable<string> RequestObjectSigningAlgValuesSupported { get; }

        public IEnumerable<string> RequestObjectEncryptionAlgValuesSupported { get; }

        public IEnumerable<string> RequestObjectEncryptionEncValuesSupported { get; }

        public IEnumerable<string> TokenEndpointAuthenticationMethodsSupported { get; }

        public IEnumerable<string> TokenEndpointAuthenticationSigningAlgValuesSupported { get; }

        public IEnumerable<string> DisplayValuesSupported { get; }

        public IEnumerable<string> ClaimTypesSupported { get; }

        public IEnumerable<string> ClaimsSupported { get; }

        public Uri ServiceDocumentationEndpoint { get; }

        public IEnumerable<string> ClaimsLocalesSupported { get; }

        public IEnumerable<string> UiLocalesSupported { get; }

        public bool? ClaimsParameterSupported { get; }

        public bool? RequestParameterSupported { get; }

        public bool? RequestUriParameterSupported { get; }

        public bool? RequireRequestUriRegistration { get; }

        public Uri RegistrationPolicyEndpoint { get; }

        public Uri RegistrationTermsOfServiceEndpoint { get; }

        #endregion
    }
}