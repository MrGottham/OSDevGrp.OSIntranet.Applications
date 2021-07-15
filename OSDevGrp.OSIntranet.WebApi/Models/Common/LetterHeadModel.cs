using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Common
{
    public class LetterHeadModel : LetterHeadIdentificationModel
    {
        [Required]
        [StringLength(64, MinimumLength = 1)]
        [JsonProperty(Required = Required.Always)]
        public string Line1 { get; set; }

        [StringLength(64, MinimumLength = 1)]
        [JsonProperty(Required = Required.Default)]
        public string Line2 { get; set; }

        [StringLength(64, MinimumLength = 1)]
        [JsonProperty(Required = Required.Default)]
        public string Line3 { get; set; }

        [StringLength(64, MinimumLength = 1)]
        [JsonProperty(Required = Required.Default)]
        public string Line4 { get; set; }

        [StringLength(64, MinimumLength = 1)]
        [JsonProperty(Required = Required.Default)]
        public string Line5 { get; set; }

        [StringLength(64, MinimumLength = 1)]
        [JsonProperty(Required = Required.Default)]
        public string Line6 { get; set; }

        [StringLength(64, MinimumLength = 1)]
        [JsonProperty(Required = Required.Default)]
        public string Line7 { get; set; }

        [StringLength(32, MinimumLength = 1)]
        [JsonProperty(Required = Required.Default)]
        public string CompanyIdentificationNumber { get; set; }
    }
}