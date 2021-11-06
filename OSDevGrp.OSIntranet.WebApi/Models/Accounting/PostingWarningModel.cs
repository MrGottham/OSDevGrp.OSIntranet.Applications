using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class PostingWarningModel
    {
        [Required]
        [JsonProperty(Required = Required.Always)]
        public PostingWarningReason Reason { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public AccountIdentificationModel Account { get; set; }

        [Required]
        [Range(typeof(decimal), "-99999999", "99999999")]
        [JsonProperty(Required = Required.Always)]
        public decimal Amount { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public PostingLineModel PostingLine { get; set; }
    }
}