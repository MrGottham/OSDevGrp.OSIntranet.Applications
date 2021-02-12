using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class BudgetInfoValuesViewModel
    {
        [Display(Name = "Budget", ShortName = "Budget", Description = "Budgetbeløb")]
        public decimal Budget { get; set; }

        [Display(Name = "Resultat", ShortName = "Resultat", Description = "Resultat")]
        public decimal Posted { get; set; }

        [Display(Name = "Disponibel", ShortName = "Disponibel", Description = "Disponibelt beløb i forhold til budgetbeløb")]
        public decimal Available { get; set; }
    }
}