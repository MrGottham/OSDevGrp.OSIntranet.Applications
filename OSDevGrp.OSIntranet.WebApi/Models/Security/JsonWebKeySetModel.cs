using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Security
{
    public class JsonWebKeySetModel
    {
        [Required]
        [JsonRequired]
        [JsonPropertyName("keys")]
        public IEnumerable<JsonWebKeyModel> Keys { get; set; }
    }
}