using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class BudgetAccountViewModel : AccountCoreDataViewModel
    {
        [Display(Name = "Budgetkontogruppe", ShortName = "Gruppe", Description = "Budgetkontogruppe")]
        [Required(ErrorMessage = "Der skal vælges en budgetkontogruppe.")]
        public BudgetAccountGroupViewModel BudgetAccountGroup { get; set; }

        [Display(Name = "Budgetoplysninger pr. dags dato", ShortName = "Budgetopl. pr. dags dato", Description = "Budgetoplysninger pr. dags dato")]
        public BudgetInfoValuesViewModel ValuesForMonthOfStatusDate { get; set; }

        [Display(Name = "Budgetoplysninger ved sidste måneds afslutning", ShortName = "Budgetopl. ved sidste måneds afslutning", Description = "Budgetoplysninger ved sidste måneds afslutning")]
        public BudgetInfoValuesViewModel ValuesForLastMonthOfStatusDate { get; set; }

        [Display(Name = "Budgetoplysninger for år til dato", ShortName = "Budgetopl. for år til dato", Description = "Budgetoplysninger for år til dato")]
        public BudgetInfoValuesViewModel ValuesForYearToDateOfStatusDate { get; set; }

        [Display(Name = "Budgetoplysninger ved sidste års afslutning", ShortName = "Budgetopl. ved sidste års afslutning", Description = "Budgetoplysninger ved sidste års afslutning")]
        public BudgetInfoValuesViewModel ValuesForLastYearOfStatusDate { get; set; }

        [Display(Name = "Budgetoplysninger", ShortName = "Budgetopl.", Description = "Budgetoplysninger")]
        public BudgetInfoDictionaryViewModel BudgetInfos { get; set; }

        [Display(Name = "Senest bogført", ShortName = "Bogføringslinjer", Description = "Seneste oprettede bogføringslinjer")]
        public PostingLineCollectionViewModel PostingLines { get; set; }

        public IReadOnlyCollection<BudgetAccountGroupViewModel> BudgetAccountGroups { get; set; }
    }

    public static class BudgetAccountViewModelExtensions
    {
        public static string GetAction(this BudgetAccountViewModel budgetAccountViewModel)
        {
            NullGuard.NotNull(budgetAccountViewModel, nameof(budgetAccountViewModel));

            return budgetAccountViewModel.EditMode == EditMode.Create ? "CreateBudgetAccount" : "UpdateBudgetAccount";
        }
    }
}