using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class BudgetAccountViewModel : AccountCoreDataViewModel
    {
        [Display(Name = "Budgetkontogruppe", ShortName = "Gruppe", Description = "Budgetkontogruppe")]
        [Required(ErrorMessage = "Der skal vælges en budgetkontogruppe.")]
        public BudgetAccountGroupViewModel BudgetAccountGroup { get; set; }
    }
}