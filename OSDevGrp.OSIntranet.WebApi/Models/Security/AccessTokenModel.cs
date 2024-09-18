using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Security
{
    public class AccessTokenModel
    {
        [Required]
        [MinLength(1)]
        [JsonPropertyName("token_type")]
        [JsonProperty("token_type", Required = Required.Always)]
        public string TokenType { get; set; }

        [Required]
        [MinLength(1)]
        [JsonPropertyName("access_token")]
        [JsonProperty("access_token", Required = Required.Always)]
        public string AccessToken { get; set; }

        [Required]
        [Range(1, 3600)]
        [JsonPropertyName("expires_in")]
        [JsonProperty("expires_in", Required = Required.Always)]
        public int ExpiresIn { get; set; }
    }
}