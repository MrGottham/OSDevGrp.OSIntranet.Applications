﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Security
{
    public class OpenIdProviderConfigurationModel
    {
        [Required]
        [MinLength(1)]
        [JsonPropertyName("issuer")]
        [JsonProperty("issuer", Required = Required.Always)]
        public string Issuer { get; set; }

        [Required]
        [MinLength(1)]
        [JsonPropertyName("authorization_endpoint")]
        [JsonProperty("authorization_endpoint", Required = Required.Always)]
        public string AuthorizationEndpoint { get; set; }

        [Required]
        [MinLength(1)]
        [JsonPropertyName("token_endpoint")]
        [JsonProperty("token_endpoint", Required = Required.Always)]
        public string TokenEndpoint { get; set; }

        [MinLength(1)]
        [JsonPropertyName("userinfo_endpoint")]
        [JsonProperty("userinfo_endpoint", Required = Required.Default)]
        public string UserInfoEndpoint { get; set; }

        [Required]
        [MinLength(1)]
        [JsonPropertyName("jwks_uri")]
        [JsonProperty("jwks_uri", Required = Required.Always)]
        public string JsonWebKeySetEndpoint { get; set; }

        [MinLength(1)]
        [JsonPropertyName("registration_endpoint")]
        [JsonProperty("registration_endpoint", Required = Required.Default)]
        public string RegistrationEndpoint { get; set; }

        [JsonPropertyName("scopes_supported")]
        [JsonProperty("scopes_supported", Required = Required.Default)]
        public IEnumerable<string> ScopesSupported { get; set; }

        [Required]
        [JsonPropertyName("response_types_supported")]
        [JsonProperty("response_types_supported", Required = Required.Always)]
        public IEnumerable<string> ResponseTypesSupported { get; set; }

        [JsonPropertyName("response_modes_supported")]
        [JsonProperty("response_modes_supported", Required = Required.Default)]
        public IEnumerable<string> ResponseModesSupported { get; set; }

        [JsonPropertyName("grant_types_supported")]
        [JsonProperty("grant_types_supported", Required = Required.Default)]
        public IEnumerable<string> GrantTypesSupported { get; set; }

        [JsonPropertyName("acr_values_supported")]
        [JsonProperty("acr_values_supported", Required = Required.Default)]
        public IEnumerable<string> AuthenticationContextClassReferencesSupported { get; set; }

        [Required]
        [JsonPropertyName("subject_types_supported")]
        [JsonProperty("subject_types_supported", Required = Required.Always)]
        public IEnumerable<string> SubjectTypesSupported { get; set; }

        [Required]
        [JsonPropertyName("id_token_signing_alg_values_supported")]
        [JsonProperty("id_token_signing_alg_values_supported", Required = Required.Always)]
        public IEnumerable<string> IdTokenSigningAlgValuesSupported { get; set; }

        [JsonPropertyName("id_token_encryption_alg_values_supported")]
        [JsonProperty("id_token_encryption_alg_values_supported", Required = Required.Default)]
        public IEnumerable<string> IdTokenEncryptionAlgValuesSupported { get; set; }

        [JsonPropertyName("id_token_encryption_enc_values_supported")]
        [JsonProperty("id_token_encryption_enc_values_supported", Required = Required.Default)]
        public IEnumerable<string> IdTokenEncryptionEncValuesSupported { get; set; }

        [JsonPropertyName("userinfo_signing_alg_values_supported")]
        [JsonProperty("userinfo_signing_alg_values_supported", Required = Required.Default)]
        public IEnumerable<string> UserInfoSigningAlgValuesSupported { get; set; }

        [JsonPropertyName("userinfo_encryption_alg_values_supported")]
        [JsonProperty("userinfo_encryption_alg_values_supported", Required = Required.Default)]
        public IEnumerable<string> UserInfoEncryptionAlgValuesSupported { get; set; }

        [JsonPropertyName("userinfo_encryption_enc_values_supported")]
        [JsonProperty("userinfo_encryption_enc_values_supported", Required = Required.Default)]
        public IEnumerable<string> UserInfoEncryptionEncValuesSupported { get; set; }

        [JsonPropertyName("request_object_signing_alg_values_supported")]
        [JsonProperty("request_object_signing_alg_values_supported", Required = Required.Default)]
        public IEnumerable<string> RequestObjectSigningAlgValuesSupported { get; set; }

        [JsonPropertyName("request_object_encryption_alg_values_supported")]
        [JsonProperty("request_object_encryption_alg_values_supported", Required = Required.Default)]
        public IEnumerable<string> RequestObjectEncryptionAlgValuesSupported { get; set; }

        [JsonPropertyName("request_object_encryption_enc_values_supported")]
        [JsonProperty("request_object_encryption_enc_values_supported", Required = Required.Default)]
        public IEnumerable<string> RequestObjectEncryptionEncValuesSupported { get; set; }

        [JsonPropertyName("token_endpoint_auth_methods_supported")]
        [JsonProperty("token_endpoint_auth_methods_supported", Required = Required.Default)]
        public IEnumerable<string> TokenEndpointAuthenticationMethodsSupported { get; set; }

        [JsonPropertyName("token_endpoint_auth_signing_alg_values_supported")]
        [JsonProperty("token_endpoint_auth_signing_alg_values_supported", Required = Required.Default)]
        public IEnumerable<string> TokenEndpointAuthenticationSigningAlgValuesSupported { get; set; }

        [JsonPropertyName("display_values_supported")]
        [JsonProperty("display_values_supported", Required = Required.Default)]
        public IEnumerable<string> DisplayValuesSupported { get; set; }

        [JsonPropertyName("claim_types_supported")]
        [JsonProperty("claim_types_supported", Required = Required.Default)]
        public IEnumerable<string> ClaimTypesSupported { get; set; }

        [JsonPropertyName("claims_supported")]
        [JsonProperty("claims_supported", Required = Required.Default)]
        public IEnumerable<string> ClaimsSupported { get; set; }

        [MinLength(1)]
        [JsonPropertyName("service_documentation")]
        [JsonProperty("service_documentation", Required = Required.Default)]
        public string ServiceDocumentationEndpoint { get; set; }

        [JsonPropertyName("claims_locales_supported")]
        [JsonProperty("claims_locales_supported", Required = Required.Default)]
        public IEnumerable<string> ClaimsLocalesSupported { get; set; }

        [JsonPropertyName("ui_locales_supported")]
        [JsonProperty("ui_locales_supported", Required = Required.Default)]
        public IEnumerable<string> UiLocalesSupported { get; set; }

        [JsonPropertyName("claims_parameter_supported")]
        [JsonProperty("claims_parameter_supported", Required = Required.Default)]
        public bool? ClaimsParameterSupported { get; set; }

        [JsonPropertyName("request_parameter_supported")]
        [JsonProperty("request_parameter_supported", Required = Required.Default)]
        public bool? RequestParameterSupported { get; set; }

        [JsonPropertyName("request_uri_parameter_supported")]
        [JsonProperty("request_uri_parameter_supported", Required = Required.Default)]
        public bool? RequestUriParameterSupported { get; set; }

        [JsonPropertyName("require_request_uri_registration")]
        [JsonProperty("require_request_uri_registration", Required = Required.Default)]
        public bool? RequireRequestUriRegistration { get; set; }

        [MinLength(1)]
        [JsonPropertyName("op_policy_uri")]
        [JsonProperty("op_policy_uri", Required = Required.Default)]
        public string RegistrationPolicyEndpoint { get; set; }

        [MinLength(1)]
        [JsonPropertyName("op_tos_uri")]
        [JsonProperty("op_tos_uri", Required = Required.Default)]
        public string RegistrationTermsOfServiceEndpoint { get; set; }
    }
}