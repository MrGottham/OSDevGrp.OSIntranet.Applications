using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class CreditInfoModel : BalanceInfoModel
    {
        [Required]
        [Range(typeof(decimal), "0", "99999999")]
        [JsonProperty(Required = Required.Always)]
        public decimal Credit { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public decimal Available { get; set; }
    }
}