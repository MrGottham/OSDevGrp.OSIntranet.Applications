using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Security
{
    public class ExternalLoginProviderModel
    {
        #region Properties

        [Required]
        [RegularExpression($"^({MicrosoftAccountDefaults.AuthenticationScheme}|{GoogleDefaults.AuthenticationScheme})$")]
        [JsonRequired]
        public string AuthenticationScheme { get; set; }

        [Required]
        [MinLength(1)]
        [JsonRequired]
        public string AuthenticationState { get; set; }

        #endregion
    }
}