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
using System.Text.RegularExpressions;
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
        private readonly Regex _supportedSchemeRegex = new($"^({MicrosoftAccountDefaults.AuthenticationScheme}|{GoogleDefaults.AuthenticationScheme}){{1}}$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(32));

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
        public IActionResult Login(string scheme, string returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(scheme) || _supportedSchemeRegex.IsMatch(scheme) == false)
            {
                return BadRequest();
            }

            Uri returnUri = ConvertToAbsoluteUri(string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("Index", "Home") : returnUrl);
            if (returnUri == null || _trustedDomainResolver.IsTrustedDomain(returnUri) == false)
            {
                return BadRequest();
            }

            AuthenticationProperties authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.AbsoluteAction(nameof(LoginCallback), "Account", new {returnUrl = returnUri.AbsoluteUri})
            };
            return Challenge(authenticationProperties, scheme);
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
                    return BadRequest();
                }
            }

            AuthenticateResult authenticateResult = await HttpContext.AuthenticateAsync(Schemes.External);
            try
            {
                if (authenticateResult.Succeeded == false || authenticateResult.Ticket == null || authenticateResult.Principal?.Identity == null)
                {
                    return Unauthorized();
                }

                ClaimsIdentity externalClaimsIdentity = (ClaimsIdentity)authenticateResult.Principal.Identity;
                if (externalClaimsIdentity.IsAuthenticated == false)
                {
                    return Unauthorized();
                }

                string mailAddress = externalClaimsIdentity.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrWhiteSpace(mailAddress))
                {
                    return Unauthorized();
                }

                IDataProtector dataProtector = _dataProtectionProvider.CreateProtector("TokenProtection");
                IAuthenticateUserCommand authenticateUserCommand = SecurityCommandFactory.BuildAuthenticateUserCommand(mailAddress, authenticateResult.Principal.Claims.ToArray(), Schemes.Internal, authenticateResult.Properties.Items.AsReadOnly(), dataProtector.Protect);
                ClaimsPrincipal authenticatedClaimsPrincipal = await _commandBus.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(authenticateUserCommand);
                if (authenticatedClaimsPrincipal == null)
                {
                    return Unauthorized();
                }

                await HttpContext.SignInAsync(Schemes.Internal, authenticatedClaimsPrincipal);
                try
                {
                    IGetMicrosoftTokenQuery getMicrosoftTokenQuery = SecurityQueryFactory.BuildGetMicrosoftTokenQuery(authenticatedClaimsPrincipal, dataProtector.Unprotect);
                    IRefreshableToken microsoftToken = await _queryBus.QueryAsync<IGetMicrosoftTokenQuery, IRefreshableToken>(getMicrosoftTokenQuery);
                    await HandleMicrosoftToken(microsoftToken);

                    IGetGoogleTokenQuery getGoogleTokenQuery = SecurityQueryFactory.BuildGetGoogleTokenQuery(authenticatedClaimsPrincipal, dataProtector.Unprotect);
                    IToken googleToken = await _queryBus.QueryAsync<IGetGoogleTokenQuery, IToken>(getGoogleTokenQuery);
                    await HandleGoogleToken(googleToken);

                    if (returnUri != null)
                    {
                        return Redirect(returnUri.AbsoluteUri);
                    }

                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    await _tokenHelperFactory.HandleLogoutAsync(HttpContext);
                    await HttpContext.SignOutAsync(Schemes.Internal);
                    return Unauthorized();
                }
            }
            finally
            {
                await HttpContext.SignOutAsync(Schemes.External, authenticateResult.Properties);
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

            await HttpContext.SignOutAsync(Schemes.Internal);
            await HttpContext.SignOutAsync(Schemes.External);

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction("Index", "Home");
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
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(state) || Guid.TryParse(state, out Guid stateIdentifier) == false)
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
                ErrorMessage = "Handlingen kan ikke udføres, fordi du ikke har de nødvendige tilladelser."
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