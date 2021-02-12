namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class AccountDictionaryViewModel : AccountDictionaryViewModelBase<AccountGroupViewModel, AccountCollectionViewModel, AccountViewModel>
    {
        public AccountCollectionValuesViewModel ValuesAtStatusDate { get; set; }

        public AccountCollectionValuesViewModel ValuesAtEndOfLastMonthFromStatusDate { get; set; }

        public AccountCollectionValuesViewModel ValuesAtEndOfLastYearFromStatusDate { get; set; }
    }
}