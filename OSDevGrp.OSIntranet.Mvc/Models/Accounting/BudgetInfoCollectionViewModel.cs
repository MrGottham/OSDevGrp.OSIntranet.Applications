using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class BudgetInfoCollectionViewModel : InfoCollectionViewModelBase<BudgetInfoViewModel>
    {
        [Display(Name = "Indtægter", ShortName = "Indtægter", Description = "Indtægter")]
        public decimal Income => Items.AsParallel().Sum(budgetInfoViewModel => budgetInfoViewModel.Income);

        [Display(Name = "Udgifter", ShortName = "Udgifter", Description = "Udgifter")]
        public decimal Expenses => Items.AsParallel().Sum(budgetInfoViewModel => budgetInfoViewModel.Expenses);

        [Display(Name = "Budget", ShortName = "Budget", Description = "Budgetbeløb")]
        public decimal Budget => Income - Expenses;

        [Display(Name = "Bogført", ShortName = "Bogført", Description = "Bogført")]
        public decimal Posted => Items.AsParallel().Sum(budgetInfoViewModel => budgetInfoViewModel.Posted);

        [Display(Name = "Disponibel", ShortName = "Disponibel", Description = "Disponibelt beløb i forhold til budgetbeløb")]
        public decimal Available => Items.AsParallel().Sum(budgetInfoViewModel => budgetInfoViewModel.Available);
    }
}