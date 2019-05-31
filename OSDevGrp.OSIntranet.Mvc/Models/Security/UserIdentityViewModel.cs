using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Security
{
    public class UserIdentityViewModel : IdentityViewModelBase
    {
        #region Properties

        [Display(Name = "Ekstern brugeridentifikation", ShortName = "Brugeridentifikation", Description = "Ekstern brugeridentifikation")]
        [Required(ErrorMessage = "Den eksterne brugeridentifikation skal udfyldes.", AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 1, ErrorMessage = "Længden på den eksterne brugeridentifikation skal være mellem {2} og {1} tegn.")]
        public string ExternalUserIdentifier { get; set; }

        #endregion
    }
}
