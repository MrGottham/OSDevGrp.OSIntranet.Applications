using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class AccountCollectionValuesViewModel
    {
        [Display(Name = "Aktiver", ShortName = "Aktiver", Description = "Aktiver")]
        public decimal Assets { get; set; }

        [Display(Name = "Passiver", ShortName = "Passiver", Description = "Passiver")]
        public decimal Liabilities { get; set; }
    }
}