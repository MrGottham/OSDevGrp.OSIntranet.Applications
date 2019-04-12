using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Core
{
    public class ErrorModel
    {
        [JsonProperty(Required = Required.Always)]
        public int ErrorCode { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string ErrorType { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string ErrorMessage { get; set; }

        [JsonProperty(Required = Required.DisallowNull)]
        public string Method { get; set; }

        [JsonProperty(Required = Required.DisallowNull)]
        public string ValidatingType { get; set; }

        [JsonProperty(Required = Required.DisallowNull)]
        public string ValidatingField { get; set; }
    }
}
