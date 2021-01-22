using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class ContactAccountCollectionValuesViewModel
    {
        [Display(Name = "Debitorer", ShortName = "Debitorer", Description = "Debitorer")]
        public decimal Debtors { get; set; }

        [Display(Name = "Kreditorer", ShortName = "Kreditorer", Description = "Kreditorer")]
        public decimal Creditors { get; set; }
    }
}