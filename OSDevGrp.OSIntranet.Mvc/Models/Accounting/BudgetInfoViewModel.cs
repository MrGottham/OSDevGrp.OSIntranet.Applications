using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class BudgetInfoViewModel : InfoViewModelBase
    {
        [Display(Name = "Indtægter", ShortName = "Indtægter", Description = "Indtægter")]
        [Required(ErrorMessage = "Der skal angives et beløb for indtægter.")]
        [Range(typeof(decimal), "0", "99999999", ErrorMessage = "Beløbet for indtægter må ikke være mindre end {1}.")]
        public decimal Income { get; set; }

        [Display(Name = "Udgifter", ShortName = "Udgifter", Description = "Udgifter")]
        [Required(ErrorMessage = "Der skal angives et beløb for udgifter.")]
        [Range(typeof(decimal), "0", "99999999", ErrorMessage = "Beløbet for udgifter må ikke være mindre end {1}.")]
        public decimal Expenses { get; set; }

        [Display(Name = "Budget", ShortName = "Budget", Description = "Budgetbeløb")]
        public decimal Budget => Income - Expenses;

        [Display(Name = "Bogført", ShortName = "Bogført", Description = "Bogført")]
        public decimal Posted { get; set; }

        [Display(Name = "Disponibel", ShortName = "Disponibel", Description = "Disponibelt beløb i forhold til budgetbeløb")]
        public decimal Available { get; set; }
    }
}