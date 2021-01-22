using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class ContactInfoValuesModel
    {
        [JsonProperty(Required = Required.Always)]
        public decimal Balance { get; set; }
    }
}