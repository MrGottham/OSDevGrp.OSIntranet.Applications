using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class CreditInfoViewModel : BalanceInfoViewModel
    {
        [Display(Name = "Kredit", ShortName = "Kredit", Description = "Kreditbeløb")]
        [Required(ErrorMessage = "Der skal angives et kreditbeløb.")]
        [Range(typeof(decimal), "0", "99999999", ErrorMessage = "Kreditbeløbet må ikke være mindre end {1}.")]
        public decimal Credit { get; set; }

        [Display(Name = "Disponibel", ShortName = "Disponibel", Description = "Disponibelt beløb")]
        public decimal Available { get; set; }
    }
}