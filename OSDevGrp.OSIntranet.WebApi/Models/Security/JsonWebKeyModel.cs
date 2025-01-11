using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Security
{
    public class JsonWebKeyModel
    {
        [Required]
        [RegularExpression("^(EC|RSA|oct)$")]
        [JsonRequired]
        [JsonPropertyName("kty")]
        public string Kty { get; set; }

        [Required]
        [RegularExpression("^(sig|enc)$")]
        [JsonRequired]
        [JsonPropertyName("use")]
        public string Use { get; set; }

        [Required]
        [RegularExpression("^(HS256|HS384|HS512|RS256|RS384|RS512|ES256|ES384|ES512|PS256|PS384|PS512|none)$")]
        [JsonRequired]
        [JsonPropertyName("alg")]
        public string Alg { get; set; }

        [Required]
        [MinLength(1)]
        [JsonRequired]
        [JsonPropertyName("kid")]
        public string Kid { get; set; }

        [Required]
        [MinLength(1)]
        [JsonRequired]
        [JsonPropertyName("n")]
        public string N { get; set; }

        [Required]
        [MinLength(1)]
        [JsonRequired]
        [JsonPropertyName("e")]
        public string E { get; set; }

        [JsonPropertyName("d")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string D { get; set; }

        [JsonPropertyName("dp")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string DP { get; set; }

        [JsonPropertyName("dq")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string DQ { get; set; }

        [JsonPropertyName("p")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string P { get; set; }

        [JsonPropertyName("q")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Q { get; set; }

        [JsonPropertyName("qi")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string QI { get; set; }
    }
}