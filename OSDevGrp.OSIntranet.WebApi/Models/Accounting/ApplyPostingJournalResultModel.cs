using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class ApplyPostingJournalResultModel
    {
        [Required]
        [JsonProperty(Required = Required.Always)]
        public PostingLineCollectionModel PostingLines { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public PostingWarningCollectionModel PostingWarnings { get; set; }
    }
}