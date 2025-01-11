using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public abstract class InfoModelBase
    {
        [Required]
        [Range(1950, 2199)]
        [JsonRequired]
        public short Year { get; set; }

        [Required]
        [Range(1, 12)]
        [JsonRequired]
        public short Month { get; set; }
    }
}