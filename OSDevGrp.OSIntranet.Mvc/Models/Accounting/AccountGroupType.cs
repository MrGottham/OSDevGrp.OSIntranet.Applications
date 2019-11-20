using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public enum AccountGroupType
    {
        [Display(Name = "Aktiver")]
        Assets,

        [Display(Name = "Passiver")]
        Liabilities
    }
}