using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public abstract class AccountGroupModelBase
    {
        [JsonProperty(Required = Required.Always)]
        public int Number { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }
    }
}
