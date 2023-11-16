using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using OSDevGrp.OSIntranet.Mvc.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Mvc.Controllers
{
	[Authorize]
    public class AccountController : Controller
    {
        #region Private variables

        private readonly ICommandBus _commandBus;
        private readonly ITrustedDomainHelper _trustedDomainHelper;
        private readonly ITokenHelperFactory _tokenHelperFactory;
        private readonly IDataProtectionProvider _dataProtectionProvider;

		#endregion

		#region Constructor

		public AccountController(ICommandBus commandBus, ITrustedDomainHelper trustedDomainHelper, ITokenHelperFactory tokenHelperFactory, IDataProtectionProvider dataProtectionProvider)
		{
			NullGuard.NotNull(commandBus, nameof(commandBus))
				.NotNull(trustedDomainHelper, nameof(trustedDomainHelper))
				.NotNull(tokenHelperFactory, nameof(tokenHelperFactory))
				.NotNull(dataProtectionProvider, nameof(dataProtectionProvider));

            _commandBus = commandBus;
            _trustedDomainHelper = trustedDomainHelper;
            _tokenHelperFactory = tokenHelperFactory;
            _dataProtectionProvider = dataProtectionProvider;
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

            return new ChallengeResult(MicrosoftAccountDefaults.AuthenticationScheme, new AuthenticationProperties
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

            return new ChallengeResult(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
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

            AuthenticateResult authenticateResult = await HttpContext.AuthenticateAsync(Schemas.ExternalAuthenticationSchema);
            if (authenticateResult.Succeeded == false || authenticateResult.Ticket == null || authenticateResult.Principal == null)
            {
                return Unauthorized();
            }

            Claim mailClaim = authenticateResult.Principal.FindFirst(ClaimTypes.Email);
            if (mailClaim == null || string.IsNullOrWhiteSpace(mailClaim.Value))
            {
                return Unauthorized();
            }

            IAuthenticateUserCommand authenticateUserCommand = SecurityCommandFactory.BuildAuthenticateUserCommand(mailClaim.Value, authenticateResult.Principal.Claims.ToArray(), Schemas.InternalAuthenticationSchema, authenticateResult.Properties.Items.AsReadOnly(), value => _dataProtectionProvider.CreateProtector("TokenProtection").Protect(value));
            IUserIdentity userIdentity = await _commandBus.PublishAsync<IAuthenticateUserCommand, IUserIdentity>(authenticateUserCommand);
            if (userIdentity == null)
            {
                return Unauthorized();
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userIdentity.ToClaimsIdentity().Claims, Schemas.InternalAuthenticationSchema);
            await HttpContext.SignInAsync(Schemas.InternalAuthenticationSchema, new ClaimsPrincipal(claimsIdentity));

            AuthenticationProperties authenticationProperties = authenticateResult.Ticket.Properties;
            foreach (TokenType tokenType in Enum.GetValues(typeof(TokenType)).Cast<TokenType>())
            {
                string tokenTypeKey = $".{tokenType}";
                if (authenticationProperties.Items.ContainsKey(tokenTypeKey) == false)
                {
                    continue;
                }

                await _tokenHelperFactory.StoreTokenAsync(tokenType, HttpContext, authenticationProperties.Items[tokenTypeKey]);
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
            return View("Manage", HttpContext.User);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _tokenHelperFactory.HandleLogoutAsync(HttpContext);

            await HttpContext.SignOutAsync(Schemas.InternalAuthenticationSchema);

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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
	        ErrorViewModel errorViewModel = new ErrorViewModel
	        {
		        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ErrorCode = null,
                ErrorMesssage = "Handlingen kan ikke udføres, fordi du ikke har de nødvendige tilladelser."
	        };

	        return View("Error", errorViewModel);
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