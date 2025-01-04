using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class PostingWarningModel
    {
        [Required]
        [JsonRequired]
        public PostingWarningReason Reason { get; set; }

        [Required]
        [JsonRequired]
        public AccountIdentificationModel Account { get; set; }

        [Required]
        [Range(typeof(decimal), "-99999999", "99999999")]
        [JsonRequired]
        public decimal Amount { get; set; }

        [Required]
        [JsonRequired]
        public PostingLineModel PostingLine { get; set; }
    }
}