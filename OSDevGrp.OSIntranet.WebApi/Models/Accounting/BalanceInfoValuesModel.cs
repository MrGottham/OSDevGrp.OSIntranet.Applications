using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class BalanceInfoValuesModel
    {
        [JsonProperty(Required = Required.Always)]
        public decimal Balance { get; set; }
    }
}