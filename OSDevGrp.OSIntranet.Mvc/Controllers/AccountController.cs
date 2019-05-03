using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Queries;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers;

namespace OSDevGrp.OSIntranet.Mvc.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        #region Private variables

        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor

        public AccountController(ICommandBus commandBus, IQueryBus queryBus)
        {
            NullGuard.NotNull(commandBus, nameof(commandBus))
                .NotNull(queryBus, nameof(queryBus));

            _commandBus = commandBus;
            _queryBus = queryBus;
        }

        #endregion

        #region Properties
        private Uri RedirectUriForMicrosoftGraph => new Uri(Url.AbsoluteAction("MicrosoftGraphCallback").ToLower());

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

        [HttpGet]
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
            if (HttpContext.Request.Cookies.ContainsKey(GetMicrosoftGraphTokenCookieName()))
            {
                HttpContext.Response.Cookies.Delete(GetMicrosoftGraphTokenCookieName());
            }

            await HttpContext.SignOutAsync("OSDevGrp.OSIntranet.Internal");

            if (string.IsNullOrWhiteSpace(returnUrl) == false)
            {
                return Redirect(returnUrl);
            }

            return LocalRedirect("/Home/Index");
        }

        [HttpGet]
        public async Task<IActionResult> AuthorizeMicrosoftGraph(string returnUrl = null)
        {
            Guid stateIdentifier = Guid.NewGuid();

            string cookieName = GetStateCookieName(stateIdentifier);
            string cookieValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.IsNullOrWhiteSpace(returnUrl) ? string.Empty : returnUrl));
            HttpContext.Response.Cookies.Append(cookieName, cookieValue, new CookieOptions {Expires = DateTimeOffset.Now.AddMinutes(15)});
            try
            {
                IGetAuthorizeUriForMicrosoftGraphQuery query = new GetAuthorizeUriForMicrosoftGraphQuery(RedirectUriForMicrosoftGraph, stateIdentifier);
                Uri authorizeUri = await _queryBus.QueryAsync<IGetAuthorizeUriForMicrosoftGraphQuery, Uri>(query);

                return Redirect(authorizeUri.AbsoluteUri);
            }
            catch
            {
                HttpContext.Response.Cookies.Delete(cookieName);
                throw;
            }
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

            string cookieName = GetStateCookieName(stateIdentifier);
            if (HttpContext.Request.Cookies.ContainsKey(cookieName) == false)
            {
                return BadRequest();
            }

            try
            {
                IAcquireTokenForMicrosoftGraphCommand acquireTokenForMicrosoftGraphCommand = new AcquireTokenForMicrosoftGraphCommand(RedirectUriForMicrosoftGraph, code);
                IRefreshableToken refreshableToken = await _commandBus.PublishAsync<IAcquireTokenForMicrosoftGraphCommand, IRefreshableToken>(acquireTokenForMicrosoftGraphCommand);

                StoreMicrosoftGraphToken(HttpContext, refreshableToken);
                try
                {
                    string returnUrl = Encoding.UTF8.GetString(Convert.FromBase64String(HttpContext.Request.Cookies[cookieName]));
                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return Redirect(returnUrl);
                }
                catch
                {
                    HttpContext.Response.Cookies.Delete(GetMicrosoftGraphTokenCookieName());
                    throw;
                }
            }
            finally
            {
                HttpContext.Response.Cookies.Delete(cookieName);
            }
        }

        public static IRefreshableToken GetMicrosoftGraphToken(HttpContext httpContext)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext));

            if (httpContext.Request.Cookies.ContainsKey(GetMicrosoftGraphTokenCookieName()) == false)
            {
                return null;
            }

            return Token.Create<RefreshableToken>(httpContext.Request.Cookies[GetMicrosoftGraphTokenCookieName()]);
        }

        public static void StoreMicrosoftGraphToken(HttpContext httpContext, IRefreshableToken refreshableToken)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext))
                .NotNull(refreshableToken, nameof(refreshableToken));

            httpContext.Response.Cookies.Append(GetMicrosoftGraphTokenCookieName(), refreshableToken.ToBase64(), new CookieOptions {Expires = DateTimeOffset.Now.AddHours(3)});
        }

        private static string GetMicrosoftGraphTokenCookieName()
        {
            return "OSDevGrp.OSIntranet.Microsoft.Graph.Token";
        }

        private static string GetStateCookieName(Guid stateIdentifier)
        {
            return $"OSDevGrp.OSIntranet.State.{stateIdentifier:D}";
        }

        #endregion
    }
}
