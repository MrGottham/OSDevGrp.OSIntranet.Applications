using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Security
{
    public class ErrorResponseModel
    {
        [Required]
        [RegularExpression("^(invalid_request|invalid_client|invalid_grant|invalid_scope|unauthorized_client|access_denied|unsupported_response_type|unsupported_grant_type|server_error|temporarily_unavailable){1}$")]
        [JsonPropertyName("error")]
        [JsonProperty("error", Required = Required.Always)]
        public string Error { get; set; }

        [JsonPropertyName("error_description")]
        [JsonProperty("error_description", Required = Required.Default)]
        public string ErrorDescription { get; set; }

        [JsonPropertyName("error_uri")]
        [JsonProperty("error_uri", Required = Required.Default)]
        public string ErrorUri { get; set; }

        [JsonPropertyName("state")]
        [JsonProperty("state", Required = Required.Default)]
        public string State { get; set; }
    }
}