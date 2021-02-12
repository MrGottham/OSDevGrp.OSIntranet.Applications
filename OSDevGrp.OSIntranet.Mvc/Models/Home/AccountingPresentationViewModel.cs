using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;

namespace OSDevGrp.OSIntranet.Mvc.Models.Home
{
    public class AccountingPresentationViewModel : AccountingIdentificationViewModel
    {
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:d. MMMM yyyy}")]
        public DateTime StatusDate { get; set; }

        public AccountCollectionValuesViewModel ValuesAtStatusDateForAccounts { get; set; }

        public BudgetInfoValuesViewModel ValuesForMonthOfStatusDateForBudgetAccounts { get; set; }

        public ContactAccountCollectionValuesViewModel ValuesAtStatusDateForContactAccounts { get; set; }

        public ReadOnlyCollection<ContactAccountPresentationViewModel> Debtors { get; set; }

        public ReadOnlyCollection<ContactAccountPresentationViewModel> Creditors { get; set; }
    }
}