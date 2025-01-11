using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class AccountIdentificationModel
    {
        [Required]
        [JsonRequired]
        public AccountingIdentificationModel Accounting { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 1)]
        [RegularExpression(ValidationRegexPatterns.AccountNumberRegexPattern)]
        [JsonRequired]
        public string AccountNumber { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        [JsonRequired]
        public string AccountName { get; set; }
    }
}