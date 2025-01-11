using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Common
{
    public class LetterHeadModel : LetterHeadIdentificationModel
    {
        [Required]
        [StringLength(64, MinimumLength = 1)]
        [JsonRequired]
        public string Line1 { get; set; }

        [StringLength(64, MinimumLength = 1)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Line2 { get; set; }

        [StringLength(64, MinimumLength = 1)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Line3 { get; set; }

        [StringLength(64, MinimumLength = 1)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Line4 { get; set; }

        [StringLength(64, MinimumLength = 1)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Line5 { get; set; }

        [StringLength(64, MinimumLength = 1)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Line6 { get; set; }

        [StringLength(64, MinimumLength = 1)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Line7 { get; set; }

        [StringLength(32, MinimumLength = 1)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string CompanyIdentificationNumber { get; set; }
    }
}