using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using OSDevGrp.OSIntranet.WebApi.Models.Common;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class AccountingModel : AccountingIdentificationModel
    {
        [Required]
        [JsonProperty(Required = Required.Always)]
        public LetterHeadIdentificationModel LetterHead { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public BalanceBelowZeroType BalanceBelowZero { get; set; }

        [Required]
        [Range(0, 365)]
        [JsonProperty(Required = Required.Always)]
        public int BackDating { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public DateTimeOffset StatusDate { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public AccountCollectionModel Accounts { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public BudgetAccountCollectionModel BudgetAccounts { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public ContactAccountCollectionModel ContactAccounts { get; set; }
    }
}