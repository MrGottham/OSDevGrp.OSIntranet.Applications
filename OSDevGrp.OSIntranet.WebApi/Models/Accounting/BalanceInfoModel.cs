using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class BalanceInfoModel : InfoModelBase
    {
        [Required]
        [JsonProperty(Required = Required.Always)]
        public decimal Balance { get; set; }
    }
}