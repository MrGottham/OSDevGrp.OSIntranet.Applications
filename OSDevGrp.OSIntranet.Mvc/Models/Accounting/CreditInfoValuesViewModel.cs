using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class CreditInfoValuesViewModel : BalanceInfoValuesViewModel
    {
        [Display(Name = "Kredit", ShortName = "Kredit", Description = "Kreditbeløb")]
        public decimal Credit { get; set; }

        [Display(Name = "Disponibel", ShortName = "Disponibel", Description = "Disponibelt beløb")]
        public decimal Available { get; set; }
    }
}