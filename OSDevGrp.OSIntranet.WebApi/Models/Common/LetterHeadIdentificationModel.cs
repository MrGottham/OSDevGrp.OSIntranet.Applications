using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Common
{
    public class LetterHeadIdentificationModel
    {
        [JsonProperty(Required = Required.Always)]
        public int Number { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }
    }
}
