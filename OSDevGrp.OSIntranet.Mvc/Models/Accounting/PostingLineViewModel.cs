using System;
using System.ComponentModel.DataAnnotations;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class PostingLineViewModel : AuditableViewModelBase
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

        [Display(Name = "Konto", ShortName = "Konto", Description = "Konto")]
        [Required(ErrorMessage = "Der skal vælges en konto.")]
        public AccountIdentificationViewModel Account { get; set; }

        [Display(Name = "Kreditoplysninger ved posteringsdato", ShortName = "Kreditopl. ved posteringsdato", Description = "Kreditoplysninger ved posteringsdato")]
        public CreditInfoValuesViewModel AccountValuesAtPostingDate { get; set; }

        [Display(Name = "Tekst", ShortName = "Tekst", Description = "Tekst for posteringen")]
        [Required(ErrorMessage = "Teksten for posteringen skal udfyldes.")]
        [StringLength(256, MinimumLength = 1, ErrorMessage = "Teksten for posteringen skal være mellem {2} og {1} tegn.")]
        public string Details { get; set; }

        [Display(Name = "Budgetkonto", ShortName = "Budgetkonto", Description = "Budgetkonto")]
        public AccountIdentificationViewModel BudgetAccount { get; set; }

        [Display(Name = "Budgetoplysninger ved posteringsdato", ShortName = "Budgetopl. ved posteringsdato", Description = "Budgetoplysninger ved posteringsdato")]
        public BudgetInfoValuesViewModel BudgetAccountValuesAtPostingDate { get; set; }

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

        [DataType(DataType.Text)]
        [Display(Name = "Beløb", ShortName = "Beløb", Description = "Beløb, der skal posteres.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        public decimal PostingValue => (Debit ?? 0M) - (Credit ?? 0M);

        [Display(Name = "Kontaktkonto", ShortName = "Kontaktkonto", Description = "Kontaktkonto")]
        public AccountIdentificationViewModel ContactAccount { get; set; }

        [Display(Name = "Saldooplysninger ved posteringsdato", ShortName = "Saldoopl. ved posteringsdato", Description = "Saldooplysninger ved posteringsdato")]
        public BalanceInfoValuesViewModel ContactAccountValuesAtPostingDate { get; set; }

        public int? SortOrder { get; set; }
    }
}