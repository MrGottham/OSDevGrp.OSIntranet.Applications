using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class BalanceInfoModel : InfoModelBase
    {
        [JsonProperty(Required = Required.Always)]
        public decimal Balance { get; set; }
    }
}