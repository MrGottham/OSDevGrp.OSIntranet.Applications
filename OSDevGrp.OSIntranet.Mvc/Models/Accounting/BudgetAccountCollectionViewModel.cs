using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class BudgetAccountCollectionViewModel : AccountCollectionViewModelBase<BudgetAccountViewModel>
    {
        [Display(Name = "Budgetoplysninger pr. dags dato", ShortName = "Budgetopl. pr. dags dato", Description = "Budgetoplysninger pr. dags dato")]
        public BudgetInfoValuesViewModel ValuesForMonthOfStatusDate { get; set; }

        [Display(Name = "Budgetoplysninger ved sidste m�neds afslutning", ShortName = "Budgetopl. ved sidste m�neds afslutning", Description = "Budgetoplysninger ved sidste m�neds afslutning")]
        public BudgetInfoValuesViewModel ValuesForLastMonthOfStatusDate { get; set; }

        [Display(Name = "Budgetoplysninger for �r til dato", ShortName = "Budgetopl. for �r til dato", Description = "Budgetoplysninger for �r til dato")]
        public BudgetInfoValuesViewModel ValuesForYearToDateOfStatusDate { get; set; }

        [Display(Name = "Budgetoplysninger ved sidste �rs afslutning", ShortName = "Budgetopl. ved sidste �rs afslutning", Description = "Budgetoplysninger ved sidste �rs afslutning")]
        public BudgetInfoValuesViewModel ValuesForLastYearOfStatusDate { get; set; }
    }
}