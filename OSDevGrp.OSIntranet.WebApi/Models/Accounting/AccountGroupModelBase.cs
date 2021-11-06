using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public abstract class AccountGroupModelBase
    {
        [Required]
        [Range(1, 99)]
        [JsonProperty(Required = Required.Always)]
        public int Number { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }
    }
}