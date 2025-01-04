using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Security
{
    public class OpenIdProviderConfigurationModel
    {
        [Required]
        [MinLength(1)]
        [JsonRequired]
        [JsonPropertyName("issuer")]
        public string Issuer { get; set; }

        [Required]
        [MinLength(1)]
        [JsonRequired]
        [JsonPropertyName("authorization_endpoint")]
        public string AuthorizationEndpoint { get; set; }

        [Required]
        [MinLength(1)]
        [JsonRequired]
        [JsonPropertyName("token_endpoint")]
        public string TokenEndpoint { get; set; }

        [MinLength(1)]
        [JsonPropertyName("userinfo_endpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserInfoEndpoint { get; set; }

        [Required]
        [MinLength(1)]
        [JsonRequired]
        [JsonPropertyName("jwks_uri")]
        public string JsonWebKeySetEndpoint { get; set; }

        [MinLength(1)]
        [JsonPropertyName("registration_endpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RegistrationEndpoint { get; set; }

        [JsonPropertyName("scopes_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> ScopesSupported { get; set; }

        [Required]
        [JsonRequired]
        [JsonPropertyName("response_types_supported")]
        public IEnumerable<string> ResponseTypesSupported { get; set; }

        [JsonPropertyName("response_modes_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> ResponseModesSupported { get; set; }

        [JsonPropertyName("grant_types_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> GrantTypesSupported { get; set; }

        [JsonPropertyName("acr_values_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> AuthenticationContextClassReferencesSupported { get; set; }

        [Required]
        [JsonRequired]
        [JsonPropertyName("subject_types_supported")]
        public IEnumerable<string> SubjectTypesSupported { get; set; }

        [Required]
        [JsonRequired]
        [JsonPropertyName("id_token_signing_alg_values_supported")]
        public IEnumerable<string> IdTokenSigningAlgValuesSupported { get; set; }

        [JsonPropertyName("id_token_encryption_alg_values_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> IdTokenEncryptionAlgValuesSupported { get; set; }

        [JsonPropertyName("id_token_encryption_enc_values_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> IdTokenEncryptionEncValuesSupported { get; set; }

        [JsonPropertyName("userinfo_signing_alg_values_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> UserInfoSigningAlgValuesSupported { get; set; }

        [JsonPropertyName("userinfo_encryption_alg_values_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> UserInfoEncryptionAlgValuesSupported { get; set; }

        [JsonPropertyName("userinfo_encryption_enc_values_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> UserInfoEncryptionEncValuesSupported { get; set; }

        [JsonPropertyName("request_object_signing_alg_values_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> RequestObjectSigningAlgValuesSupported { get; set; }

        [JsonPropertyName("request_object_encryption_alg_values_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> RequestObjectEncryptionAlgValuesSupported { get; set; }

        [JsonPropertyName("request_object_encryption_enc_values_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> RequestObjectEncryptionEncValuesSupported { get; set; }

        [JsonPropertyName("token_endpoint_auth_methods_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> TokenEndpointAuthenticationMethodsSupported { get; set; }

        [JsonPropertyName("token_endpoint_auth_signing_alg_values_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> TokenEndpointAuthenticationSigningAlgValuesSupported { get; set; }

        [JsonPropertyName("display_values_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> DisplayValuesSupported { get; set; }

        [JsonPropertyName("claim_types_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> ClaimTypesSupported { get; set; }

        [JsonPropertyName("claims_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> ClaimsSupported { get; set; }

        [MinLength(1)]
        [JsonPropertyName("service_documentation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ServiceDocumentationEndpoint { get; set; }

        [JsonPropertyName("claims_locales_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> ClaimsLocalesSupported { get; set; }

        [JsonPropertyName("ui_locales_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> UiLocalesSupported { get; set; }

        [JsonPropertyName("claims_parameter_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ClaimsParameterSupported { get; set; }

        [JsonPropertyName("request_parameter_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? RequestParameterSupported { get; set; }

        [JsonPropertyName("request_uri_parameter_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? RequestUriParameterSupported { get; set; }

        [JsonPropertyName("require_request_uri_registration")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? RequireRequestUriRegistration { get; set; }

        [MinLength(1)]
        [JsonPropertyName("op_policy_uri")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RegistrationPolicyEndpoint { get; set; }

        [MinLength(1)]
        [JsonPropertyName("op_tos_uri")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RegistrationTermsOfServiceEndpoint { get; set; }
    }
}