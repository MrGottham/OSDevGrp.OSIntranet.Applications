using System;
using System.Collections.Generic;
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

        public IReadOnlyCollection<ContactAccountPresentationViewModel> Debtors { get; set; }

        public IReadOnlyCollection<ContactAccountPresentationViewModel> Creditors { get; set; }

        public IReadOnlyCollection<PostingLinePresentationViewModel> PostingLines { get; set; }
    }
}