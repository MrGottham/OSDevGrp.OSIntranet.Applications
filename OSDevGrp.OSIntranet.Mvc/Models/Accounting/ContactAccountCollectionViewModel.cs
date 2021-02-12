namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class ContactAccountCollectionViewModel : AccountCollectionViewModelBase<ContactAccountViewModel>
    {
        public ContactAccountCollectionValuesViewModel ValuesAtStatusDate { get; set; }

        public ContactAccountCollectionValuesViewModel ValuesAtEndOfLastMonthFromStatusDate { get; set; }

        public ContactAccountCollectionValuesViewModel ValuesAtEndOfLastYearFromStatusDate { get; set; }
    }
}