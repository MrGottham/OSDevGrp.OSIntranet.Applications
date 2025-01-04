using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class ApplyPostingLineModel
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Guid? Identifier { get; set; }

        [Required]
        [JsonRequired]
        public DateTimeOffset PostingDate { get; set; }

        [StringLength(16, MinimumLength = 0)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Reference { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 1)]
        [RegularExpression(ValidationRegexPatterns.AccountNumberRegexPattern)]
        [JsonRequired]
        public string AccountNumber { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        [JsonRequired]
        public string Details { get; set; }

        [StringLength(16, MinimumLength = 0)]
        [RegularExpression(ValidationRegexPatterns.AccountNumberRegexPattern)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string BudgetAccountNumber { get; set; }

        [Range(typeof(decimal), "0", "99999999")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Debit { get; set; }

        [Range(typeof(decimal), "0", "99999999")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Credit { get; set; }

        [StringLength(16, MinimumLength = 0)]
        [RegularExpression(ValidationRegexPatterns.AccountNumberRegexPattern)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ContactAccountNumber { get; set; }

        [Range(typeof(int), "0", "9999999")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? SortOrder { get; set; }
    }
}