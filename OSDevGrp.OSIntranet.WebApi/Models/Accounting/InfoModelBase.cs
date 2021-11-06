using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public abstract class InfoModelBase
    {
        [Required]
        [Range(1950, 2199)]
        [JsonProperty(Required = Required.Always)]
        public short Year { get; set; }

        [Required]
        [Range(1, 12)]
        [JsonProperty(Required = Required.Always)]
        public short Month { get; set; }
    }
}