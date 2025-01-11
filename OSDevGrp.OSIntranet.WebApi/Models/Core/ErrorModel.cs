using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Core
{
    public class ErrorModel
    {
        [Required]
        [Range(1000, 9999)]
        [JsonRequired]
        public int ErrorCode { get; set; }

        [Required]
        [MinLength(1)]
        [JsonRequired]
        public string ErrorType { get; set; }

        [Required]
        [MinLength(1)]
        [JsonRequired]
        public string ErrorMessage { get; set; }

        [JsonRequired]
        public string Method { get; set; }

        [JsonRequired]
        public string ValidatingType { get; set; }

        [JsonRequired]
        public string ValidatingField { get; set; }
    }
}