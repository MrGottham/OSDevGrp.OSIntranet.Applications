using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Common
{
    public class LetterHeadModel : LetterHeadIdentificationModel
    {
        [JsonProperty(Required = Required.Always)]
        public string Line1 { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string Line2 { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string Line3 { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string Line4 { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string Line5 { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string Line6 { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string Line7 { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string CompanyIdentificationNumber { get; set; }
    }
}
