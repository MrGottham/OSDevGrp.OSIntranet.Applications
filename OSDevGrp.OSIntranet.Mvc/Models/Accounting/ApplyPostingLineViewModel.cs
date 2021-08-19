using System;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class ApplyPostingLineViewModel
    {
        public Guid? Identifier { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Dato", ShortName = "Dato", Description = "Posteringsdato")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [Required(ErrorMessage = "Posteringsdatoen skal udfyldes.")]
        public DateTime PostingDate { get; set; }

        [Display(Name = "Bilag", ShortName = "Bilag", Description = "Bilag")]
        [StringLength(16, MinimumLength = 0, ErrorMessage = "Længden på bilaget skal være mellem {2} og {1} tegn.")]
        public string Reference { get; set; }

        [Display(Name = "Konto", ShortName = "Konto", Description = "Kontonummer")]
        [Required(ErrorMessage = "Kontonummeret skal udfyldes.", AllowEmptyStrings = false)]
        [StringLength(16, MinimumLength = 1, ErrorMessage = "Længden på kontonummeret skal være mellem {2} og {1} tegn.")]
        [RegularExpression(ValidationRegexPatterns.AccountNumberRegexPattern, ErrorMessage = "Kontonummeret følger ikke det lovlige mønster: {1}")]
        public string AccountNumber { get; set; }

        [Display(Name = "Tekst", ShortName = "Tekst", Description = "Tekst for posteringen")]
        [Required(ErrorMessage = "Teksten for posteringen skal udfyldes.")]
        [StringLength(256, MinimumLength = 1, ErrorMessage = "Teksten for posteringen skal være mellem {2} og {1} tegn.")]
        public string Details { get; set; }

        [Display(Name = "Budgetkonto", ShortName = "Budgetkonto", Description = "Kontonummer for budgetkonto")]
        [StringLength(16, MinimumLength = 0, ErrorMessage = "Længden på kontonummeret for budgetkontoen skal være mellem {2} og {1} tegn.")]
        [RegularExpression(ValidationRegexPatterns.AccountNumberRegexPattern, ErrorMessage = "Kontonummeret for budgetkontoen følger ikke det lovlige mønster: {1}")]
        public string BudgetAccountNumber { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Debit", ShortName = "Debit", Description = "Beløb, der skal debiteres")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        [Range(typeof(decimal), "0", "99999999", ErrorMessage = "Beløbet, der skal debiteres, må ikke være mindre end {1}.")]
        public decimal? Debit { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Kredit", ShortName = "Kredit", Description = "Beløb, der skal krediteres")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        [Range(typeof(decimal), "0", "99999999", ErrorMessage = "Beløbet, der skal krediteres, må ikke være mindre end {1}.")]
        public decimal? Credit { get; set; }

        [Display(Name = "Kontaktkonto", ShortName = "Kontaktkonto", Description = "Kontonummer for kontaktkonto")]
        [StringLength(16, MinimumLength = 0, ErrorMessage = "Længden på kontonummeret for kontaktkontoen skal være mellem {2} og {1} tegn.")]
        [RegularExpression(ValidationRegexPatterns.AccountNumberRegexPattern, ErrorMessage = "Kontonummeret for kontaktkontoen følger ikke det lovlige mønster: {1}")]
        public string ContactAccountNumber { get; set; }

        public int? SortOrder { get; set; }
    }
}