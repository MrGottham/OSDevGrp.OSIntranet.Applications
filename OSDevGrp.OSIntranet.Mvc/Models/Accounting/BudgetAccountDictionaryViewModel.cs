namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class BudgetAccountDictionaryViewModel : AccountDictionaryViewModelBase<BudgetAccountGroupViewModel, BudgetAccountCollectionViewModel, BudgetAccountViewModel>
    {
        public BudgetInfoValuesViewModel ValuesForMonthOfStatusDate { get; set; }

        public BudgetInfoValuesViewModel ValuesForLastMonthOfStatusDate { get; set; }

        public BudgetInfoValuesViewModel ValuesForYearToDateOfStatusDate { get; set; }

        public BudgetInfoValuesViewModel ValuesForLastYearOfStatusDate { get; set; }
    }
}