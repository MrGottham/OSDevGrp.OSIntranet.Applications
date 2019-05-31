using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Security
{
    public class ClaimViewModel
    {
        public string ClaimType { get; set; }

        [Display(Name = "Rettighedsværdi", ShortName = "Rettighedsværdi", Description = "Rettighedsværdi")]
        [StringLength(256, MinimumLength = 0, ErrorMessage = "Længden på rettighedsværdien skal være mellem {2} og {1} tegn.")]
        public string ActualValue { get; set; }

        public string DefaultValue { get; set; }

        public bool HasDefaultValue => string.IsNullOrWhiteSpace(DefaultValue) == false;

        public bool IsSelected { get; set; }
    }
}