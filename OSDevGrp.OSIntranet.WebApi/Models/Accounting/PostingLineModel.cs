using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class PostingLineModel
    {
        [Required]
        [JsonRequired]
        public Guid Identifier { get; set; }

        [Required]
        [JsonRequired]
        public DateTimeOffset PostingDate { get; set; }

        [StringLength(16, MinimumLength = 1)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Reference { get; set; }

        [Required]
        [JsonRequired]
        public AccountIdentificationModel Account { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CreditInfoValuesModel AccountValuesAtPostingDate { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        [JsonRequired]
        public string Details { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AccountIdentificationModel BudgetAccount { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public BudgetInfoValuesModel BudgetAccountValuesAtPostingDate { get; set; }

        [Range(typeof(decimal), "0", "99999999")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Debit { get; set; }

        [Range(typeof(decimal), "0", "99999999")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Credit { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AccountIdentificationModel ContactAccount { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public BalanceInfoValuesModel ContactAccountValuesAtPostingDate { get; set; }

        [Required]
        [Range(typeof(int), "0", "9999999")]
        [JsonRequired]
        public int SortOrder { get; set; }
    }
}