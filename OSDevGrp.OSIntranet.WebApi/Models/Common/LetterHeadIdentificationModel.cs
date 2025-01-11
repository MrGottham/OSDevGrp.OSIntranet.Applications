using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Common
{
    public class LetterHeadIdentificationModel
    {
        [Required]
        [Range(1, 99)]
        [JsonRequired]
        public int Number { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        [JsonRequired]
        public string Name { get; set; }
    }
}