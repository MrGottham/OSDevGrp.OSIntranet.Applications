using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class ApplyPostingJournalModel
    {
        [Required]
        [Range(1, 99)]
        [JsonRequired]
        public int AccountingNumber { get; set; }

        [Required]
        [JsonRequired]
        public ApplyPostingLineCollectionModel ApplyPostingLines { get; set; }
    }
}