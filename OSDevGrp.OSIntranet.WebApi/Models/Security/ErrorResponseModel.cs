using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Security
{
    public class ErrorResponseModel
    {
        [Required]
        [RegularExpression("^(invalid_request|invalid_client|invalid_grant|invalid_scope|unauthorized_client|access_denied|unsupported_response_type|unsupported_grant_type|server_error|temporarily_unavailable){1}$")]
        [JsonRequired]
        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("error_description")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ErrorDescription { get; set; }

        [JsonPropertyName("error_uri")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ErrorUri { get; set; }

        [JsonPropertyName("state")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string State { get; set; }
    }
}