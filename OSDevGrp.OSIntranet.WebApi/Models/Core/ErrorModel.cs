using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Core
{
    public class ErrorModel
    {
        [Required]
        [Range(1000, 9999)]
        [JsonProperty(Required = Required.Always)]
        public int ErrorCode { get; set; }

        [Required]
        [MinLength(1)]
        [JsonProperty(Required = Required.Always)]
        public string ErrorType { get; set; }

        [Required]
        [MinLength(1)]
        [JsonProperty(Required = Required.Always)]
        public string ErrorMessage { get; set; }

        [JsonProperty(Required = Required.DisallowNull)]
        public string Method { get; set; }

        [JsonProperty(Required = Required.DisallowNull)]
        public string ValidatingType { get; set; }

        [JsonProperty(Required = Required.DisallowNull)]
        public string ValidatingField { get; set; }
    }
}