using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Security
{
    public class AccessTokenModel
    {
        [Required]
        [MinLength(1)]
        [JsonProperty(Required = Required.Always)]
        public string TokenType { get; set; }

        [Required]
        [MinLength(1)]
        [JsonProperty(Required = Required.Always)]
        public string AccessToken { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public DateTimeOffset Expires { get; set; }
    }
}