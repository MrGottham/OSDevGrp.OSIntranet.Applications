using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Security
{
    public class JsonWebKeyModel
    {
        [Required]
        [RegularExpression("^(EC|RSA|oct)$")]
        [JsonPropertyName("kty")]
        [JsonProperty("kty", Required = Required.Always)]
        public string Kty { get; set; }

        [Required]
        [RegularExpression("^(sig|enc)$")]
        [JsonPropertyName("use")]
        [JsonProperty("use", Required = Required.Always)]
        public string Use { get; set; }

        [Required]
        [RegularExpression("^(HS256|HS384|HS512|RS256|RS384|RS512|ES256|ES384|ES512|PS256|PS384|PS512|none)$")]
        [JsonPropertyName("alg")]
        [JsonProperty("alg", Required = Required.Always)]
        public string Alg { get; set; }

        [Required]
        [MinLength(1)]
        [JsonPropertyName("kid")]
        [JsonProperty("kid", Required = Required.Always)]
        public string Kid { get; set; }

        [Required]
        [MinLength(1)]
        [JsonPropertyName("n")]
        [JsonProperty("n", Required = Required.Always)]
        public string N { get; set; }

        [Required]
        [MinLength(1)]
        [JsonPropertyName("e")]
        [JsonProperty("e", Required = Required.Always)]
        public string E { get; set; }

        [JsonPropertyName("d")]
        [JsonProperty("d", Required = Required.Default)]
        public string D { get; set; }

        [JsonPropertyName("dp")]
        [JsonProperty("dp", Required = Required.Default)]
        public string DP { get; set; }

        [JsonPropertyName("dq")]
        [JsonProperty("dq", Required = Required.Default)]
        public string DQ { get; set; }

        [JsonPropertyName("p")]
        [JsonProperty("p", Required = Required.Default)]
        public string P { get; set; }

        [JsonPropertyName("q")]
        [JsonProperty("q", Required = Required.Default)]
        public string Q { get; set; }

        [JsonPropertyName("qi")]
        [JsonProperty("qi", Required = Required.Default)]
        public string QI { get; set; }
    }
}