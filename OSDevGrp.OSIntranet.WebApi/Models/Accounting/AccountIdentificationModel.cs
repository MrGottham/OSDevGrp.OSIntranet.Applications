using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class AccountIdentificationModel
    {
        [JsonProperty(Required = Required.Always)]
        public AccountingIdentificationModel Accounting { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string AccountNumber { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string AccountName { get; set; }
    }
}