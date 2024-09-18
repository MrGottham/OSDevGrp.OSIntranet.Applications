using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Security
{
    public class JsonWebKeySetModel
    {
        [Required]
        [JsonPropertyName("keys")]
        [JsonProperty("keys", Required = Required.Always)]
        public IEnumerable<JsonWebKeyModel> Keys { get; set; }
    }
}