using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class AccountGroupModel : AccountGroupModelBase
    {
        [JsonProperty(Required = Required.Always)]
        public AccountGroupType AccountGroupType { get; set; }
    }
}
