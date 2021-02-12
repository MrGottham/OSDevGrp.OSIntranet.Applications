using OSDevGrp.OSIntranet.Mvc.Models.Accounting;

namespace OSDevGrp.OSIntranet.Mvc.Models.Home
{
    public class ContactAccountPresentationViewModel : AccountIdentificationViewModel
    {
        public BalanceInfoValuesViewModel ValuesAtStatusDate { get; set; }
    }
}