using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class CreditInfoViewModel : BalanceInfoViewModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "Kredit", ShortName = "Kredit", Description = "Kreditbeløb")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        [Required(ErrorMessage = "Der skal angives et kreditbeløb.")]
        [Range(typeof(decimal), "0", "99999999", ErrorMessage = "Kreditbeløbet må ikke være mindre end {1}.")]
        public decimal Credit { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Disponibel", ShortName = "Disponibel", Description = "Disponibelt beløb")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        public decimal Available { get; set; }
    }
}