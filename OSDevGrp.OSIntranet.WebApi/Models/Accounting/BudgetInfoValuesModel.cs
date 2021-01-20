using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class BudgetInfoValuesModel
    {
        [JsonProperty(Required = Required.Always)]
        public decimal Budget { get; set; }

        [JsonProperty(Required = Required.Always)]
        public decimal Posted { get; set; }

        [JsonProperty(Required = Required.Always)]
        public decimal Available { get; set;  }
    }
}