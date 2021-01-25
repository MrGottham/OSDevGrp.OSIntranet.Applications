using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public abstract class InfoModelBase
    {
        [JsonProperty(Required = Required.Always)]
        public short Year { get; set; }

        [JsonProperty(Required = Required.Always)]
        public short Month { get; set; }
    }
}