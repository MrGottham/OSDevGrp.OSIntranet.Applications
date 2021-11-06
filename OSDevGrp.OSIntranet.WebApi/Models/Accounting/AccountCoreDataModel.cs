using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class AccountCoreDataModel : AccountIdentificationModel
    {
        [StringLength(512, MinimumLength = 1)]
        [JsonProperty(Required = Required.Default)]
        public string Description { get; set; }

        [StringLength(4096, MinimumLength = 1)]
        [JsonProperty(Required = Required.Default)]
        public string Note { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public DateTimeOffset StatusDate { get; set; }
    }
}