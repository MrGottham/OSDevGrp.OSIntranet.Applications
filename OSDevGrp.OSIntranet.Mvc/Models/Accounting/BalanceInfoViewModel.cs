using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class BalanceInfoViewModel : InfoViewModelBase
    {
        [DataType(DataType.Text)]
        [Display(Name = "Saldo", ShortName = "Saldo", Description = "Saldo")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        public decimal Balance { get; set; }
    }
}