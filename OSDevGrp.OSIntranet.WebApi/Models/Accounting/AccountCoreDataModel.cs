using System;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class AccountCoreDataModel : AccountIdentificationModel
    {
        [JsonProperty(Required = Required.Default)]
        public string Description { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string Note { get; set; }

        [JsonProperty(Required = Required.Always)]
        public DateTimeOffset StatusDate { get; set; }
    }
}