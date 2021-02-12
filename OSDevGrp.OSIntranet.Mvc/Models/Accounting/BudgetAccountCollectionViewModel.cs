namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class BudgetAccountCollectionViewModel : AccountCollectionViewModelBase<BudgetAccountViewModel>
    {
        public BudgetInfoValuesViewModel ValuesForMonthOfStatusDate { get; set; }

        public BudgetInfoValuesViewModel ValuesForLastMonthOfStatusDate { get; set; }

        public BudgetInfoValuesViewModel ValuesForYearToDateOfStatusDate { get; set; }

        public BudgetInfoValuesViewModel ValuesForLastYearOfStatusDate { get; set; }
    }
}