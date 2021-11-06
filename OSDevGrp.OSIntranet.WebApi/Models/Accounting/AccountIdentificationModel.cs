using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class AccountIdentificationModel
    {
        [Required]
        [JsonProperty(Required = Required.Always)]
        public AccountingIdentificationModel Accounting { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 1)]
        [RegularExpression(ValidationRegexPatterns.AccountNumberRegexPattern)]
        [JsonProperty(Required = Required.Always)]
        public string AccountNumber { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        [JsonProperty(Required = Required.Always)]
        public string AccountName { get; set; }
    }
}