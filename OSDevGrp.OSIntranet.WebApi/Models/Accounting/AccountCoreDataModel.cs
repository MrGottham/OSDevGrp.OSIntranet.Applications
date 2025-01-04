using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class AccountCoreDataModel : AccountIdentificationModel
    {
        [StringLength(512, MinimumLength = 1)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Description { get; set; }

        [StringLength(4096, MinimumLength = 1)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Note { get; set; }

        [Required]
        [JsonRequired]
        public DateTimeOffset StatusDate { get; set; }
    }
}