using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class BudgetInfoViewModel : InfoViewModelBase
    {
        [DataType(DataType.Text)]
        [Display(Name = "Indtægter", ShortName = "Indtægter", Description = "Indtægter")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        [Required(ErrorMessage = "Der skal angives et beløb for indtægter.")]
        [Range(typeof(decimal), "0", "99999999", ErrorMessage = "Beløbet for indtægter må ikke være mindre end {1}.")]
        public decimal Income { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Udgifter", ShortName = "Udgifter", Description = "Udgifter")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        [Required(ErrorMessage = "Der skal angives et beløb for udgifter.")]
        [Range(typeof(decimal), "0", "99999999", ErrorMessage = "Beløbet for udgifter må ikke være mindre end {1}.")]
        public decimal Expenses { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Budget", ShortName = "Budget", Description = "Budgetbeløb")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        public decimal Budget => Income - Expenses;

        [DataType(DataType.Text)]
        [Display(Name = "Resultat", ShortName = "Resultat", Description = "Resultat")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        public decimal Posted { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Disponibel", ShortName = "Disponibel", Description = "Disponibelt beløb i forhold til budgetbeløb")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        public decimal Available { get; set; }
    }
}