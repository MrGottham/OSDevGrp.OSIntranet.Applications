using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class BudgetInfoValuesModel
    {
        [Required]
        [JsonProperty(Required = Required.Always)]
        public decimal Budget { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public decimal Posted { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public decimal Available { get; set;  }
    }
}