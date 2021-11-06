using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class CreditInfoValuesModel : BalanceInfoValuesModel
    {
        [Required]
        [JsonProperty(Required = Required.Always)]
        public decimal Credit { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public decimal Available { get; set; }
    }
}