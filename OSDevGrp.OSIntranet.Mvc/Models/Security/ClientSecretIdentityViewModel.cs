using System;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Security
{
    public class ClientSecretIdentityViewModel : IdentityViewModelBase
    {
        #region Private variables

        private string _maskedClientSecret;

        #endregion

        #region Properties

        [Display(Name = "Navn på Web API klient", ShortName = "Web API klient", Description = "Navn på Web API klient")]
        [Required(ErrorMessage = "Navnet på Web API klienten udfyldes.", AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 1, ErrorMessage = "Længden på navnet for Web API klienten skal være mellem {2} og {1} tegn.")]
        public string FriendlyName { get; set; }

        [Display(Name = "Web API klientidentifikation", ShortName = "Klientidentifikation", Description = "Web API klientidentifikation")]
        public string ClientId { get; set; }

        [Display(Name = "Web API klient hemmelighed", ShortName = "Klient hemmelighed", Description = "Web API klient hemmelighed")]
        public string ClientSecret 
        { 
            get => _maskedClientSecret;
            set => _maskedClientSecret = string.IsNullOrWhiteSpace(value) ? null : value.Substring(0, Math.Min(4, value.Length)) + new string('*', value.Length - Math.Min(4, value.Length));
        }

        #endregion
    }
}
