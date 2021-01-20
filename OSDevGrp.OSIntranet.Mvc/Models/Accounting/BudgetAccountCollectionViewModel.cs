using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class BudgetAccountCollectionViewModel : AccountCollectionViewModelBase<BudgetAccountViewModel>
    {
        [Display(Name = "Budgetoplysninger pr. dags dato", ShortName = "Budgetopl. pr. dags dato", Description = "Budgetoplysninger pr. dags dato")]
        public BudgetInfoValuesViewModel ValuesForMonthOfStatusDate { get; set; }

        [Display(Name = "Budgetoplysninger ved sidste måneds afslutning", ShortName = "Budgetopl. ved sidste måneds afslutning", Description = "Budgetoplysninger ved sidste måneds afslutning")]
        public BudgetInfoValuesViewModel ValuesForLastMonthOfStatusDate { get; set; }

        [Display(Name = "Budgetoplysninger for år til dato", ShortName = "Budgetopl. for år til dato", Description = "Budgetoplysninger for år til dato")]
        public BudgetInfoValuesViewModel ValuesForYearToDateOfStatusDate { get; set; }

        [Display(Name = "Budgetoplysninger ved sidste års afslutning", ShortName = "Budgetopl. ved sidste års afslutning", Description = "Budgetoplysninger ved sidste års afslutning")]
        public BudgetInfoValuesViewModel ValuesForLastYearOfStatusDate { get; set; }
    }
}