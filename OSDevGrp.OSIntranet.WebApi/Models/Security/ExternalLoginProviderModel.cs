using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.WebApi.Models.Security
{
    public class ExternalLoginProviderModel
    {
        #region Properties

        [Required]
        [RegularExpression($"^({MicrosoftAccountDefaults.AuthenticationScheme}|{GoogleDefaults.AuthenticationScheme})$")]
        [JsonProperty(Required = Required.Always)]
        public string AuthenticationScheme { get; set; }

        [Required]
        [MinLength(1)]
        [JsonProperty(Required = Required.Always)]
        public string AuthenticationState { get; set; }

        #endregion
    }
}