using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class BalanceInfoValuesModel
    {
        [Required]
        [JsonProperty(Required = Required.Always)]
        public decimal Balance { get; set; }
    }
}