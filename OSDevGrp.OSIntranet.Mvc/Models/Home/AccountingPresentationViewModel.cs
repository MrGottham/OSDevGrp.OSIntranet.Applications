using System;
using System.ComponentModel.DataAnnotations;
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;

namespace OSDevGrp.OSIntranet.Mvc.Models.Home
{
    public class AccountingPresentationViewModel : AccountingIdentificationViewModel
    {
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:d. MMMM yyyy}")]
        public DateTime StatusDate { get; set; }
    }
}