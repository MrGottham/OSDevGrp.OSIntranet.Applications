using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public enum BalanceBelowZeroType
    {
        [Display(Name = "Debitorer")]
        Debtors,

        [Display(Name = "Kreditorer")]
        Creditors
    }
}