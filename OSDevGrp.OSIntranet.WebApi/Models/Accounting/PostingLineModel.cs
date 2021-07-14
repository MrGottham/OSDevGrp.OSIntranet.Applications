using System;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class PostingLineModel
    {
        [JsonProperty(Required = Required.Default)]
        public Guid? Identifier { get; set; }

        [JsonProperty(Required = Required.Always)]
        public DateTime PostingDate { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string Reference { get; set; }

        [JsonProperty(Required = Required.Always)]
        public AccountIdentificationModel Account { get; set; }

        [JsonProperty(Required = Required.Default)]
        public CreditInfoValuesModel AccountValuesAtPostingDate { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Details { get; set; }

        [JsonProperty(Required = Required.Default)]
        public AccountIdentificationModel BudgetAccount { get; set; }

        [JsonProperty(Required = Required.Default)]
        public BudgetInfoValuesModel BudgetAccountValuesAtPostingDate { get; set; }

        [JsonProperty(Required = Required.Default)]
        public decimal? Debit { get; set; }

        [JsonProperty(Required = Required.Default)]
        public decimal? Credit { get; set; }

        [JsonProperty(Required = Required.Default)]
        public AccountIdentificationModel ContactAccount { get; set; }

        [JsonProperty(Required = Required.Default)]
        public BalanceInfoValuesModel ContactAccountValuesAtPostingDate { get; set; }

        [JsonProperty(Required = Required.Default)]
        public int? SortOrder { get; set; }
    }
}