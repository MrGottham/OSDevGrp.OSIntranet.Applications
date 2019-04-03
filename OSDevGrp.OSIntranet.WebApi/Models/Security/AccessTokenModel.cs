using System;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Security
{
    public class AccessTokenModel
    {
        [JsonProperty(Required = Required.Always)]
        public string AccessToken { get; set; }

        [JsonProperty(Required = Required.Always)]
        public DateTime Expires { get; set; }
    }
}
