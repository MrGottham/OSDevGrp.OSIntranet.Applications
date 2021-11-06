using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class ApplyPostingLineModel
    {
        [JsonProperty(Required = Required.Default)]
        public Guid? Identifier { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public DateTimeOffset PostingDate { get; set; }

        [StringLength(16, MinimumLength = 0)]
        [JsonProperty(Required = Required.Default)]
        public string Reference { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 1)]
        [RegularExpression(ValidationRegexPatterns.AccountNumberRegexPattern)]
        [JsonProperty(Required = Required.Always)]
        public string AccountNumber { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        [JsonProperty(Required = Required.Always)]
        public string Details { get; set; }

        [StringLength(16, MinimumLength = 0)]
        [RegularExpression(ValidationRegexPatterns.AccountNumberRegexPattern)]
        [JsonProperty(Required = Required.Default)]
        public string BudgetAccountNumber { get; set; }

        [Range(typeof(decimal), "0", "99999999")]
        [JsonProperty(Required = Required.Default)]
        public decimal? Debit { get; set; }

        [Range(typeof(decimal), "0", "99999999")]
        [JsonProperty(Required = Required.Default)]
        public decimal? Credit { get; set; }

        [StringLength(16, MinimumLength = 0)]
        [RegularExpression(ValidationRegexPatterns.AccountNumberRegexPattern)]
        [JsonProperty(Required = Required.Default)]
        public string ContactAccountNumber { get; set; }

        [Range(typeof(int), "0", "9999999")]
        [JsonProperty(Required = Required.Default)]
        public int? SortOrder { get; set; }
    }
}