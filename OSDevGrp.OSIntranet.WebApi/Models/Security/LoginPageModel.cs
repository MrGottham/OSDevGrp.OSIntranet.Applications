using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OSDevGrp.OSIntranet.WebApi.Helpers.Extensions;
using OSDevGrp.OSIntranet.WebApi.Security;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.WebApi.Helpers.Resolvers;

namespace OSDevGrp.OSIntranet.WebApi.Models.Security
{
    public class LoginPageModel : PageModel
    {
        #region Properties

        public ExternalLoginProviderModel Microsoft { get; private set; }

        public ExternalLoginProviderModel Google { get; private set; }

        #endregion

        #region Methods

        public Task<IActionResult> OnGetAsync(string authorizationState)
        {
            return Task.Run<IActionResult>(() =>
            {
                if (string.IsNullOrWhiteSpace(authorizationState))
                {
                    return BadRequest(ErrorResponseModelResolver.Resolve("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "authorizationState"), null, authorizationState));
                }

                Microsoft = new ExternalLoginProviderModel
                {
                    AuthenticationScheme = MicrosoftAccountDefaults.AuthenticationScheme,
                    AuthenticationState = authorizationState
                };
                Google = new ExternalLoginProviderModel
                {
                    AuthenticationScheme = GoogleDefaults.AuthenticationScheme,
                    AuthenticationState = authorizationState
                };

                return Page();
            });
        }

        public Task<IActionResult> OnPostAsync(ExternalLoginProviderModel externalLoginProviderModel)
        {
            return Task.Run<IActionResult>(() =>
            {
                if (externalLoginProviderModel == null)
                {
                    return BadRequest(ErrorResponseModelResolver.Resolve("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNull, "externalLoginProviderModel"), null, null));
                }

                if (ModelState.IsValid == false)
                {
                    return BadRequest(ErrorResponseModelResolver.Resolve("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.SubmittedMessageInvalid, "externalLoginProviderModel"), null, null));
                }

                AuthenticationProperties authenticationProperties = new AuthenticationProperties
                {
                    RedirectUri = Url.AbsoluteAction("AuthorizeCallback", "Security").ToString(),
                };
                authenticationProperties.Items.Add(KeyNames.AuthorizationStateKey, externalLoginProviderModel.AuthenticationState);

                return Challenge(authenticationProperties, externalLoginProviderModel.AuthenticationScheme);
            });
        }

        #endregion
    }
}