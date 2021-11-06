using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class BudgetInfoModel : InfoModelBase
    {
        [Required]
        [Range(typeof(decimal), "0", "99999999")]
        [JsonProperty(Required = Required.Always)]
        public decimal Income { get; set; }

        [Required]
        [Range(typeof(decimal), "0", "99999999")]
        [JsonProperty(Required = Required.Always)]
        public decimal Expenses { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public decimal Budget { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public decimal Posted { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public decimal Available { get; set; }
    }
}