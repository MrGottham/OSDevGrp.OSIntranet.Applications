using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class CreditInfoValuesModel
    {
        [JsonProperty(Required = Required.Always)]
        public decimal Credit { get; set; }

        [JsonProperty(Required = Required.Always)]
        public decimal Balance { get; set; }

        [JsonProperty(Required = Required.Always)]
        public decimal Available { get; set; }
    }
}