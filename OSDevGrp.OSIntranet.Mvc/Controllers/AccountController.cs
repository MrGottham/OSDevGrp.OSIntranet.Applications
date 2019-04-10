using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers;

namespace OSDevGrp.OSIntranet.Mvc.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        #region Private variables

        private readonly ICommandBus _commandBus;

        #endregion

        #region Constructor

        public AccountController(ICommandBus commandBus)
        {
            NullGuard.NotNull(commandBus, nameof(commandBus));

            _commandBus = commandBus;
        }

        #endregion

        #region Methods

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return View("Login");
            }

            if (Uri.TryCreate(returnUrl, UriKind.Absolute, out Uri absoluteUri))
            {
                return View("Login", absoluteUri);
            }

            if (Uri.TryCreate(returnUrl, UriKind.Relative, out Uri relativeUri))
            {
                string absoluteContent = Url.AbsoluteContent(relativeUri.OriginalString);
                return View("Login", new Uri(absoluteContent));
            }

            return View("Login");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult LoginWithMicrosoftAccount(string returnUrl = null)
        {
            return new ChallengeResult(MicrosoftAccountDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.AbsoluteAction(nameof(LoginCallback), new {returnUrl = string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("Index", "Home") : returnUrl})
                });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult LoginWithGoogleAccount(string returnUrl = null)
        {
            return new ChallengeResult(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.AbsoluteAction(nameof(LoginCallback), new {returnUrl = string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("Index", "Home") : returnUrl})
                });
        }

        [HttpGet()]
        [AllowAnonymous]
        public async Task<IActionResult> LoginCallback(string returnUrl = null)
        {
            AuthenticateResult authenticateResult = await HttpContext.AuthenticateAsync("OSDevGrp.OSIntranet.External");
            if (authenticateResult.Succeeded == false)
            {
                return Unauthorized();
            }

            Claim mailClaim = authenticateResult.Principal.FindFirst(ClaimTypes.Email);
            if (mailClaim == null || string.IsNullOrWhiteSpace(mailClaim.Value))
            {
                return Unauthorized();
            }

            IAuthenticateUserCommand authenticateUserCommand = new AuthenticateUserCommand(mailClaim.Value, authenticateResult.Principal.Claims);
            IUserIdentity userIdentity = await _commandBus.PublishAsync<IAuthenticateUserCommand, IUserIdentity>(authenticateUserCommand);
            if (userIdentity == null)
            {
                return Unauthorized();
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userIdentity.ToClaimsIdentity().Claims, "OSDevGrp.OSIntranet.Internal");
            await HttpContext.SignInAsync("OSDevGrp.OSIntranet.Internal", new ClaimsPrincipal(claimsIdentity));

            if (string.IsNullOrWhiteSpace(returnUrl) == false)
            {
                return Redirect(returnUrl);
            }

            return LocalRedirect("/Home/Index");
        }

        [HttpGet]
        public IActionResult Manage()
        {
            return View("Manage", HttpContext?.User);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await HttpContext.SignOutAsync("OSDevGrp.OSIntranet.Internal");

            if (string.IsNullOrWhiteSpace(returnUrl) == false)
            {
                return Redirect(returnUrl);
            }

            return LocalRedirect("/Home/Index");
        }

        #endregion
    }
}
