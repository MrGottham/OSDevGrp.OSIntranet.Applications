using System.ComponentModel.DataAnnotations;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class AccountIdentificationViewModel : AuditableViewModelBase
    {
        [Display(Name = "Regnskab", ShortName = "Regnskab", Description = "Regnskab")]
        [Required(ErrorMessage = "Der skal vælges et regnskab.")]
        public AccountingIdentificationViewModel Accounting { get; set; }

        [Display(Name = "Kontonummer", ShortName = "Kontonr.", Description = "Kontonummer")]
        [Required(ErrorMessage = "Kontonummeret skal udfyldes.", AllowEmptyStrings = false)]
        [StringLength(16, MinimumLength = 1, ErrorMessage = "Længden på kontonummeret skal være mellem {2} og {1} tegn.")]
        [RegularExpression(ValidationRegexPatterns.AccountNumberRegexPattern, ErrorMessage = "Kontonummeret følger ikke det lovlige mønster: {1}")]
        public string AccountNumber { get; set; }

        [Display(Name = "Kontonavn", ShortName = "Kontonavn", Description = "Kontonavn")]
        [Required(ErrorMessage = "Kontonavnet skal udfyldes.", AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 1, ErrorMessage = "Længden på kontonavnet skal være mellem {2} og {1} tegn.")]
        public string AccountName { get; set; }
    }
}