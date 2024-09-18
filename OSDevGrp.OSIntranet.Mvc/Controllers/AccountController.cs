using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Queries;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
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
        private readonly IQueryBus _queryBus;
        private readonly ITrustedDomainResolver _trustedDomainResolver;
        private readonly ITokenHelperFactory _tokenHelperFactory;
        private readonly IDataProtectionProvider _dataProtectionProvider;

		#endregion

		#region Constructor

		public AccountController(ICommandBus commandBus, IQueryBus queryBus, ITrustedDomainResolver trustedDomainResolver, ITokenHelperFactory tokenHelperFactory, IDataProtectionProvider dataProtectionProvider)
		{
			NullGuard.NotNull(commandBus, nameof(commandBus))
				.NotNull(queryBus, nameof(queryBus))
				.NotNull(trustedDomainResolver, nameof(trustedDomainResolver))
				.NotNull(tokenHelperFactory, nameof(tokenHelperFactory))
				.NotNull(dataProtectionProvider, nameof(dataProtectionProvider));

            _commandBus = commandBus;
            _queryBus = queryBus;
            _trustedDomainResolver = trustedDomainResolver;
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
            if (returnUri == null || _trustedDomainResolver.IsTrustedDomain(returnUri) == false)
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
            if (returnUri == null || _trustedDomainResolver.IsTrustedDomain(returnUri) == false)
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
            if (returnUri == null || _trustedDomainResolver.IsTrustedDomain(returnUri) == false)
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
                if (returnUri == null || _trustedDomainResolver.IsTrustedDomain(returnUri) == false)
                {
                    return Unauthorized();
                }
            }

            AuthenticateResult authenticateResult = await HttpContext.AuthenticateAsync(Schemes.ExternalAuthenticationScheme);
            if (authenticateResult.Succeeded == false || authenticateResult.Ticket == null || authenticateResult.Principal == null)
            {
                return Unauthorized();
            }

            Claim mailClaim = authenticateResult.Principal.FindFirst(ClaimTypes.Email);
            if (mailClaim == null || string.IsNullOrWhiteSpace(mailClaim.Value))
            {
                return Unauthorized();
            }

            IAuthenticateUserCommand authenticateUserCommand = SecurityCommandFactory.BuildAuthenticateUserCommand(mailClaim.Value, authenticateResult.Principal.Claims.ToArray(), Schemes.InternalAuthenticationScheme, authenticateResult.Properties.Items.AsReadOnly(), value => _dataProtectionProvider.CreateProtector("TokenProtection").Protect(value));
            ClaimsPrincipal authenticatedClaimsPrincipal = await _commandBus.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(authenticateUserCommand);
            if (authenticatedClaimsPrincipal == null)
            {
                return Unauthorized();
            }

            await HttpContext.SignInAsync(Schemes.InternalAuthenticationScheme, authenticatedClaimsPrincipal);
            try
            {
                IRefreshableToken microsoftToken = await _queryBus.QueryAsync<IGetMicrosoftTokenQuery, IRefreshableToken>(SecurityQueryFactory.BuildGetMicrosoftTokenQuery(authenticatedClaimsPrincipal, value => _dataProtectionProvider.CreateProtector("TokenProtection").Unprotect(value)));
                await HandleMicrosoftToken(microsoftToken);

                IToken googleToken = await _queryBus.QueryAsync<IGetGoogleTokenQuery, IToken>(SecurityQueryFactory.BuildGetGoogleTokenQuery(authenticatedClaimsPrincipal, value => _dataProtectionProvider.CreateProtector("TokenProtection").Unprotect(value)));
                await HandleGoogleToken(googleToken);

                if (returnUri != null)
                {
                    return Redirect(returnUri.AbsoluteUri);
                }

                return LocalRedirect("/Home/Index");
            }
            catch
            {
                await HttpContext.SignOutAsync(Schemes.InternalAuthenticationScheme);
                throw;
            }
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

            await HttpContext.SignOutAsync(Schemes.InternalAuthenticationScheme);

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return LocalRedirect("/Home/Index");
            }

            Uri returnUri = ConvertToAbsoluteUri(returnUrl);
            if (returnUri == null || _trustedDomainResolver.IsTrustedDomain(returnUri) == false)
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

        private Task HandleMicrosoftToken(IRefreshableToken microsoftToken)
        {
	        return microsoftToken != null
		        ? _tokenHelperFactory.StoreTokenAsync(TokenType.MicrosoftGraphToken, HttpContext, microsoftToken.ToBase64String())
		        : Task.CompletedTask;
        }

		private static Task HandleGoogleToken(IToken _)
        {
	        return Task.CompletedTask;
        }

        #endregion
    }
}