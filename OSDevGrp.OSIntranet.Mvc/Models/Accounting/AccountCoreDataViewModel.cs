using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class AccountCoreDataViewModel : AccountIdentificationViewModel
    {
        [Display(Name = "Beskrivelse", ShortName = "Beskrivelse", Description = "Beskrivelse")]
        [StringLength(512, MinimumLength = 0, ErrorMessage = "Længden på beskrivelsen skal være mellem {2} og {1} tegn.")]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Note", ShortName = "Note", Description = "Note")]
        [StringLength(4096, MinimumLength = 0, ErrorMessage = "Længden på noten skal være mellem {2} og {1} tegn.")]
        public string Note { get; set; }

        public bool Deletable { get; set; }
    }
}