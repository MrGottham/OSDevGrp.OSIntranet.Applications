using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Security
{
    public class AccessTokenModel
    {
        [Required]
        [MinLength(1)]
        [JsonRequired]
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [Required]
        [MinLength(1)]
        [JsonRequired]
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [MinLength(1)]
        [JsonPropertyName("id_token")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string IdToken { get; set; }

        [Required]
        [Range(1, 3600)]
        [JsonRequired]
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}