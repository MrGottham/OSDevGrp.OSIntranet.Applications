using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class PostingLineModel
    {
        [Required]
        [JsonProperty(Required = Required.Always)]
        public Guid Identifier { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public DateTimeOffset PostingDate { get; set; }

        [StringLength(16, MinimumLength = 1)]
        [JsonProperty(Required = Required.Default)]
        public string Reference { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public AccountIdentificationModel Account { get; set; }

        [JsonProperty(Required = Required.Default)]
        public CreditInfoValuesModel AccountValuesAtPostingDate { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        [JsonProperty(Required = Required.Always)]
        public string Details { get; set; }

        [JsonProperty(Required = Required.Default)]
        public AccountIdentificationModel BudgetAccount { get; set; }

        [JsonProperty(Required = Required.Default)]
        public BudgetInfoValuesModel BudgetAccountValuesAtPostingDate { get; set; }

        [Range(typeof(decimal), "0", "99999999")]
        [JsonProperty(Required = Required.Default)]
        public decimal? Debit { get; set; }

        [Range(typeof(decimal), "0", "99999999")]
        [JsonProperty(Required = Required.Default)]
        public decimal? Credit { get; set; }

        [JsonProperty(Required = Required.Default)]
        public AccountIdentificationModel ContactAccount { get; set; }

        [JsonProperty(Required = Required.Default)]
        public BalanceInfoValuesModel ContactAccountValuesAtPostingDate { get; set; }

        [Required]
        [Range(typeof(int), "0", "9999999")]
        [JsonProperty(Required = Required.Always)]
        public int SortOrder { get; set; }
    }
}