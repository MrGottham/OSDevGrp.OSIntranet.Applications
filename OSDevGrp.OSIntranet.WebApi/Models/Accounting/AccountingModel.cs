using OSDevGrp.OSIntranet.WebApi.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class AccountingModel : AccountingIdentificationModel
    {
        [Required]
        [JsonRequired]
        public LetterHeadIdentificationModel LetterHead { get; set; }

        [Required]
        [JsonRequired]
        public BalanceBelowZeroType BalanceBelowZero { get; set; }

        [Required]
        [Range(0, 365)]
        [JsonRequired]
        public int BackDating { get; set; }

        [Required]
        [JsonRequired]
        public DateTimeOffset StatusDate { get; set; }

        [Required]
        [JsonRequired]
        public AccountCollectionModel Accounts { get; set; }

        [Required]
        [JsonRequired]
        public BudgetAccountCollectionModel BudgetAccounts { get; set; }

        [Required]
        [JsonRequired]
        public ContactAccountCollectionModel ContactAccounts { get; set; }
    }
}