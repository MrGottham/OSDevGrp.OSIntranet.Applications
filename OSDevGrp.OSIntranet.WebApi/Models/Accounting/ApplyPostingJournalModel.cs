using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class ApplyPostingJournalModel
    {
        [Required]
        [Range(1, 99)]
        [JsonProperty(Required = Required.Always)]
        public int AccountingNumber { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public ApplyPostingLineCollectionModel ApplyPostingLines { get; set; }
    }
}