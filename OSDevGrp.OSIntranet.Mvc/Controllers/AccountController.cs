using System;
using System.Linq;
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
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;

namespace OSDevGrp.OSIntranet.Mvc.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        #region Private variables

        private readonly ICommandBus _commandBus;
        private readonly ITrustedDomainHelper _trustedDomainHelper;
        private readonly ITokenHelperFactory _tokenHelperFactory;

        #endregion

        #region Constructor

        public AccountController(ICommandBus commandBus, ITrustedDomainHelper trustedDomainHelper, ITokenHelperFactory tokenHelperFactory)
        {
            NullGuard.NotNull(commandBus, nameof(commandBus))
                .NotNull(trustedDomainHelper, nameof(trustedDomainHelper))
                .NotNull(tokenHelperFactory, nameof(tokenHelperFactory));

            _commandBus = commandBus;
            _trustedDomainHelper = trustedDomainHelper;
            _tokenHelperFactory = tokenHelperFactory;
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

            Uri returnUri = ConvertToAbsoluteUri(returnUrl);
            if (returnUri == null || _trustedDomainHelper.IsTrustedDomain(returnUri) == false)
            {
                return BadRequest();
            }

            return View("Login", returnUri);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult LoginWithMicrosoftAccount(string returnUrl = null)
        {
            Uri returnUri = ConvertToAbsoluteUri(string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("Index", "Home") : returnUrl);
            if (returnUri == null || _trustedDomainHelper.IsTrustedDomain(returnUri) == false)
            {
                return BadRequest();
            }

            return new ChallengeResult(MicrosoftAccountDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.AbsoluteAction(nameof(LoginCallback), "Account", new {returnUrl = returnUri.AbsoluteUri})
                });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult LoginWithGoogleAccount(string returnUrl = null)
        {
            Uri returnUri = ConvertToAbsoluteUri(string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("Index", "Home") : returnUrl);
            if (returnUri == null || _trustedDomainHelper.IsTrustedDomain(returnUri) == false)
            {
                return BadRequest();
            }

            return new ChallengeResult(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.AbsoluteAction(nameof(LoginCallback), "Account", new {returnUrl = returnUri.AbsoluteUri})
                });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginCallback(string returnUrl = null)
        {
            Uri returnUri = null;
            if (string.IsNullOrWhiteSpace(returnUrl) == false)
            {
                returnUri = ConvertToAbsoluteUri(returnUrl);
                if (returnUri == null || _trustedDomainHelper.IsTrustedDomain(returnUri) == false)
                {
                    return Unauthorized();
                }
            }

            AuthenticateResult authenticateResult = await HttpContext.AuthenticateAsync("OSDevGrp.OSIntranet.External");
            if (authenticateResult.Succeeded == false || authenticateResult.Ticket == null)
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

            AuthenticationProperties authenticationProperties = authenticateResult.Ticket.Properties;
            if (authenticationProperties?.Items != null)
            {
                foreach (TokenType tokenType in Enum.GetValues(typeof(TokenType)).Cast<TokenType>())
                {
                    string tokenTypeKey = $".{tokenType}";
                    if (authenticationProperties.Items.ContainsKey(tokenTypeKey) == false)
                    {
                        continue;
                    }

                    await _tokenHelperFactory.StoreTokenAsync(tokenType, HttpContext, authenticationProperties.Items[tokenTypeKey]);
                }
            }

            if (returnUri != null)
            {
                return Redirect(returnUri.AbsoluteUri);
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
            await _tokenHelperFactory.HandleLogoutAsync(HttpContext);

            await HttpContext.SignOutAsync("OSDevGrp.OSIntranet.Internal");

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return LocalRedirect("/Home/Index");
            }

            Uri returnUri = ConvertToAbsoluteUri(returnUrl);
            if (returnUri == null || _trustedDomainHelper.IsTrustedDomain(returnUri) == false)
            {
                return BadRequest();
            }

            return Redirect(returnUri.AbsoluteUri);
        }

        [HttpGet]
        public async Task<IActionResult> MicrosoftGraphCallback(string code, string state)
        {
            NullGuard.NotNullOrWhiteSpace(code, nameof(code))
                .NotNullOrWhiteSpace(state, nameof(state));

            if (Guid.TryParse(state, out Guid stateIdentifier) == false)
            {
                return BadRequest();
            }

            return await _tokenHelperFactory.AcquireTokenAsync(TokenType.MicrosoftGraphToken, HttpContext, stateIdentifier, code);
        }

        private Uri ConvertToAbsoluteUri(string returnUrl)
        {
            NullGuard.NotNullOrWhiteSpace(returnUrl, nameof(returnUrl));

            if (Uri.TryCreate(returnUrl, UriKind.RelativeOrAbsolute, out Uri returnUri) == false)
            {
                return null;
            }

            if (returnUri.IsAbsoluteUri)
            {
                return returnUri;
            }

            return ConvertToAbsoluteUri(Url.AbsoluteContent(returnUri.OriginalString));
        }

        #endregion
    }
}