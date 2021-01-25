using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class BudgetInfoModel : InfoModelBase
    {
        [JsonProperty(Required = Required.Always)]
        public decimal Income { get; set; }

        [JsonProperty(Required = Required.Always)]
        public decimal Expenses { get; set; }

        [JsonProperty(Required = Required.Always)]
        public decimal Budget { get; set; }

        [JsonProperty(Required = Required.Always)]
        public decimal Posted { get; set; }

        [JsonProperty(Required = Required.Always)]
        public decimal Available { get; set; }
    }
}