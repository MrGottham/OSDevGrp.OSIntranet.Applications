using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    internal class OpenIdProviderConfigurationBuilder : IOpenIdProviderConfigurationBuilder
    {
        #region Private variables

        private readonly Uri _issuer;
        private readonly Uri _authorizationEndpoint;
        private readonly Uri _tokenEndpoint;
        private readonly Uri _jsonWebKeySetEndpoint;
        private readonly string[] _responseTypesSupported;
        private readonly string[] _subjectTypesSupported;
        private readonly string[] _idTokenSigningAlgValuesSupported;
        private Uri _userInfoEndpoint;
        private Uri _registrationEndpoint;
        private string[] _scopesSupported;
        private string[] _responseModesSupported;
        private string[] _grantTypesSupported;
        private string[] _authenticationContextClassReferencesSupported;
        private string[] _idTokenEncryptionAlgValuesSupported;
        private string[] _idTokenEncryptionEncValuesSupported;
        private string[] _userInfoSigningAlgValuesSupported;
        private string[] _userInfoEncryptionAlgValuesSupported;
        private string[] _userInfoEncryptionEncValuesSupported;
        private string[] _requestObjectSigningAlgValuesSupported;
        private string[] _requestObjectEncryptionAlgValuesSupported;
        private string[] _requestObjectEncryptionEncValuesSupported;
        private string[] _tokenEndpointAuthenticationMethodsSupported;
        private string[] _tokenEndpointAuthenticationSigningAlgValuesSupported;
        private string[] _displayValuesSupported;
        private string[] _claimTypesSupported;
        private string[] _claimsSupported;
        private Uri _serviceDocumentationEndpoint;
        private string[] _claimsLocalesSupported;
        private string[] _uiLocalesSupported;
        private bool? _claimsParameterSupported;
        private bool? _requestParameterSupported;
        private bool? _requestUriParameterSupported;
        private bool? _requireRequestUriRegistration;
        private Uri _registrationPolicyEndpoint;
        private Uri _registrationTermsOfServiceEndpoint;

        #endregion

        #region Constructor

        public OpenIdProviderConfigurationBuilder(Uri issuer, Uri authorizationEndpoint, Uri tokenEndpoint, Uri jsonWebKeySetEndpoint, string[] responseTypesSupported, string[] subjectTypesSupported, string[] idTokenSigningAlgValuesSupported)
        {
            NullGuard.NotNull(issuer, nameof(issuer))
                .NotNull(authorizationEndpoint, nameof(authorizationEndpoint))
                .NotNull(tokenEndpoint, nameof(tokenEndpoint))
                .NotNull(jsonWebKeySetEndpoint, nameof(jsonWebKeySetEndpoint))
                .NotNull(responseTypesSupported, nameof(responseTypesSupported))
                .NotNull(subjectTypesSupported, nameof(subjectTypesSupported))
                .NotNull(idTokenSigningAlgValuesSupported, nameof(idTokenSigningAlgValuesSupported));

            _issuer = issuer;
            _authorizationEndpoint = authorizationEndpoint;
            _tokenEndpoint = tokenEndpoint;
            _jsonWebKeySetEndpoint = jsonWebKeySetEndpoint;
            _responseTypesSupported = RemoveNullEmptyOrWhiteSpaceValues(responseTypesSupported);
            _subjectTypesSupported = RemoveNullEmptyOrWhiteSpaceValues(subjectTypesSupported);
            _idTokenSigningAlgValuesSupported = RemoveNullEmptyOrWhiteSpaceValues(idTokenSigningAlgValuesSupported);
        }

        #endregion

        #region Methods

        public IOpenIdProviderConfigurationBuilder WithUserInfoEndpoint(Uri userInfoEndpoint)
        {
            NullGuard.NotNull(userInfoEndpoint, nameof(userInfoEndpoint));

            _userInfoEndpoint = userInfoEndpoint;

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithRegistrationEndpoint(Uri registrationEndpoint)
        {
            NullGuard.NotNull(registrationEndpoint, nameof(registrationEndpoint));

            _registrationEndpoint = registrationEndpoint;

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithScopesSupported(params string[] scopesSupported)
        {
            NullGuard.NotNull(scopesSupported, nameof(scopesSupported));

            _scopesSupported = RemoveNullEmptyOrWhiteSpaceValues(scopesSupported);

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithResponseModesSupported(params string[] responseModesSupported)
        {
            NullGuard.NotNull(responseModesSupported, nameof(responseModesSupported));

            _responseModesSupported = RemoveNullEmptyOrWhiteSpaceValues(responseModesSupported);

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithGrantTypesSupported(params string[] grantTypesSupported)
        {
            NullGuard.NotNull(grantTypesSupported, nameof(grantTypesSupported));

            _grantTypesSupported = RemoveNullEmptyOrWhiteSpaceValues(grantTypesSupported);

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithAuthenticationContextClassReferencesSupported(params string[] authenticationContextClassReferencesSupported)
        {
            NullGuard.NotNull(authenticationContextClassReferencesSupported, nameof(authenticationContextClassReferencesSupported));

            _authenticationContextClassReferencesSupported = RemoveNullEmptyOrWhiteSpaceValues(authenticationContextClassReferencesSupported);

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithIdTokenEncryptionAlgValuesSupported(params string[] idTokenEncryptionAlgValuesSupported)
        {
            NullGuard.NotNull(idTokenEncryptionAlgValuesSupported, nameof(idTokenEncryptionAlgValuesSupported));

            _idTokenEncryptionAlgValuesSupported = RemoveNullEmptyOrWhiteSpaceValues(idTokenEncryptionAlgValuesSupported);

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithIdTokenEncryptionEncValuesSupported(params string[] idTokenEncryptionEncValuesSupported)
        {
            NullGuard.NotNull(idTokenEncryptionEncValuesSupported, nameof(idTokenEncryptionEncValuesSupported));

            _idTokenEncryptionEncValuesSupported = RemoveNullEmptyOrWhiteSpaceValues(idTokenEncryptionEncValuesSupported);

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithUserInfoSigningAlgValuesSupported(params string[] userInfoSigningAlgValuesSupported)
        {
            NullGuard.NotNull(userInfoSigningAlgValuesSupported, nameof(userInfoSigningAlgValuesSupported));

            _userInfoSigningAlgValuesSupported = RemoveNullEmptyOrWhiteSpaceValues(userInfoSigningAlgValuesSupported);

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithUserInfoEncryptionAlgValuesSupported(params string[] userInfoEncryptionAlgValuesSupported)
        {
            NullGuard.NotNull(userInfoEncryptionAlgValuesSupported, nameof(userInfoEncryptionAlgValuesSupported));

            _userInfoEncryptionAlgValuesSupported = RemoveNullEmptyOrWhiteSpaceValues(userInfoEncryptionAlgValuesSupported);

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithUserInfoEncryptionEncValuesSupported(params string[] userInfoEncryptionEncValuesSupported)
        {
            NullGuard.NotNull(userInfoEncryptionEncValuesSupported, nameof(userInfoEncryptionEncValuesSupported));

            _userInfoEncryptionEncValuesSupported = RemoveNullEmptyOrWhiteSpaceValues(userInfoEncryptionEncValuesSupported);

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithRequestObjectSigningAlgValuesSupported(params string[] requestObjectSigningAlgValuesSupported)
        {
            NullGuard.NotNull(requestObjectSigningAlgValuesSupported, nameof(requestObjectSigningAlgValuesSupported));

            _requestObjectSigningAlgValuesSupported = RemoveNullEmptyOrWhiteSpaceValues(requestObjectSigningAlgValuesSupported);

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithRequestObjectEncryptionAlgValuesSupported(params string[] requestObjectEncryptionAlgValuesSupported)
        {
            NullGuard.NotNull(requestObjectEncryptionAlgValuesSupported, nameof(requestObjectEncryptionAlgValuesSupported));

            _requestObjectEncryptionAlgValuesSupported = RemoveNullEmptyOrWhiteSpaceValues(requestObjectEncryptionAlgValuesSupported);

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithRequestObjectEncryptionEncValuesSupported(params string[] requestObjectEncryptionEncValuesSupported)
        {
            NullGuard.NotNull(requestObjectEncryptionEncValuesSupported, nameof(requestObjectEncryptionEncValuesSupported));

            _requestObjectEncryptionEncValuesSupported = RemoveNullEmptyOrWhiteSpaceValues(requestObjectEncryptionEncValuesSupported);

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithTokenEndpointAuthenticationMethodsSupported(params string[] tokenEndpointAuthenticationMethodsSupported)
        {
            NullGuard.NotNull(tokenEndpointAuthenticationMethodsSupported, nameof(tokenEndpointAuthenticationMethodsSupported));

            _tokenEndpointAuthenticationMethodsSupported = RemoveNullEmptyOrWhiteSpaceValues(tokenEndpointAuthenticationMethodsSupported);

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithTokenEndpointAuthenticationSigningAlgValuesSupported(params string[] tokenEndpointAuthenticationSigningAlgValuesSupported)
        {
            NullGuard.NotNull(tokenEndpointAuthenticationSigningAlgValuesSupported, nameof(tokenEndpointAuthenticationSigningAlgValuesSupported));

            _tokenEndpointAuthenticationSigningAlgValuesSupported = RemoveNullEmptyOrWhiteSpaceValues(tokenEndpointAuthenticationSigningAlgValuesSupported);

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithDisplayValuesSupported(params string[] displayValuesSupported)
        {
            NullGuard.NotNull(displayValuesSupported, nameof(displayValuesSupported));

            _displayValuesSupported = RemoveNullEmptyOrWhiteSpaceValues(displayValuesSupported);

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithClaimTypesSupported(params string[] claimTypesSupported)
        {
            NullGuard.NotNull(claimTypesSupported, nameof(claimTypesSupported));

            _claimTypesSupported = RemoveNullEmptyOrWhiteSpaceValues(claimTypesSupported);

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithClaimsSupported(params string[] claimsSupported)
        {
            NullGuard.NotNull(claimsSupported, nameof(claimsSupported));

            _claimsSupported = RemoveNullEmptyOrWhiteSpaceValues(claimsSupported);

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithServiceDocumentationEndpoint(Uri serviceDocumentationEndpoint)
        {
            NullGuard.NotNull(serviceDocumentationEndpoint, nameof(serviceDocumentationEndpoint));

            _serviceDocumentationEndpoint = serviceDocumentationEndpoint;

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithClaimsLocalesSupported(params string[] claimsLocalesSupported)
        {
            NullGuard.NotNull(claimsLocalesSupported, nameof(claimsLocalesSupported));

            _claimsLocalesSupported = claimsLocalesSupported;

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithUiLocalesSupported(params string[] uiLocalesSupported)
        {
            NullGuard.NotNull(uiLocalesSupported, nameof(uiLocalesSupported));

            _uiLocalesSupported = uiLocalesSupported;

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithClaimsParameterSupported(bool claimsParameterSupported)
        {
            _claimsParameterSupported = claimsParameterSupported;

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithRequestParameterSupported(bool requestParameterSupported)
        {
            _requestParameterSupported = requestParameterSupported;

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithRequestUriParameterSupported(bool requestUriParameterSupported)
        {
            _requestUriParameterSupported = requestUriParameterSupported;

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithRequireRequestUriRegistration(bool requireRequestUriRegistration)
        {
            _requireRequestUriRegistration = requireRequestUriRegistration;

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithRegistrationPolicyEndpoint(Uri registrationPolicyEndpoint)
        {
            NullGuard.NotNull(registrationPolicyEndpoint, nameof(registrationPolicyEndpoint));

            _registrationPolicyEndpoint = registrationPolicyEndpoint;

            return this;
        }

        public IOpenIdProviderConfigurationBuilder WithRegistrationTermsOfServiceEndpoint(Uri registrationTermsOfServiceEndpoint)
        {
            NullGuard.NotNull(registrationTermsOfServiceEndpoint, nameof(registrationTermsOfServiceEndpoint));

            _registrationTermsOfServiceEndpoint = registrationTermsOfServiceEndpoint;

            return this;
        }

        public IOpenIdProviderConfiguration Build()
        {
            return new OpenIdProviderConfiguration(
                _issuer,
                _authorizationEndpoint,
                _tokenEndpoint,
                _userInfoEndpoint,
                _jsonWebKeySetEndpoint,
                _registrationEndpoint,
                _scopesSupported,
                _responseTypesSupported,
                _responseModesSupported,
                _grantTypesSupported,
                _authenticationContextClassReferencesSupported,
                _subjectTypesSupported,
                _idTokenSigningAlgValuesSupported,
                _idTokenEncryptionAlgValuesSupported,
                _idTokenEncryptionEncValuesSupported,
                _userInfoSigningAlgValuesSupported,
                _userInfoEncryptionAlgValuesSupported,
                _userInfoEncryptionEncValuesSupported,
                _requestObjectSigningAlgValuesSupported,
                _requestObjectEncryptionAlgValuesSupported,
                _requestObjectEncryptionEncValuesSupported,
                _tokenEndpointAuthenticationMethodsSupported,
                _tokenEndpointAuthenticationSigningAlgValuesSupported,
                _displayValuesSupported,
                _claimTypesSupported,
                _claimsSupported,
                _serviceDocumentationEndpoint,
                _claimsLocalesSupported,
                _uiLocalesSupported,
                _claimsParameterSupported,
                _requestParameterSupported,
                _requestUriParameterSupported,
                _requireRequestUriRegistration,
                _registrationPolicyEndpoint,
                _registrationTermsOfServiceEndpoint);
        }

        private static string[] RemoveNullEmptyOrWhiteSpaceValues(string[] values)
        {
            NullGuard.NotNull(values, nameof(values));

            return values.Where(value => string.IsNullOrWhiteSpace(value) == false).ToArray();
        }

        #endregion
    }
}