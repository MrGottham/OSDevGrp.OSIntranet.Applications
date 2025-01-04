using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class ApplyPostingJournalResultModel
    {
        [Required]
        [JsonRequired]
        public PostingLineCollectionModel PostingLines { get; set; }

        [Required]
        [JsonRequired]
        public PostingWarningCollectionModel PostingWarnings { get; set; }
    }
}