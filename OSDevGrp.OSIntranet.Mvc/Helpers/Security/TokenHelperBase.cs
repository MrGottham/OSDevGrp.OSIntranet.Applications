using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;

namespace OSDevGrp.OSIntranet.Mvc.Helpers.Security
{
    public abstract class TokenHelperBase<T> : ITokenHelper<T> where T : class, IToken
    {
        #region Constructor

        protected TokenHelperBase(IQueryBus queryBus, ICommandBus commandBus, ITrustedDomainHelper trustedDomainHelper, IDataProtectionProvider dataProtectionProvider)
        {
            NullGuard.NotNull(queryBus, nameof(queryBus))
                .NotNull(commandBus, nameof(commandBus))
                .NotNull(trustedDomainHelper, nameof(trustedDomainHelper))
                .NotNull(dataProtectionProvider, nameof(dataProtectionProvider));

            QueryBus = queryBus;
            CommandBus = commandBus;
            TrustedDomainHelper = trustedDomainHelper;
            DataProtectionProvider = dataProtectionProvider;
        }

        #endregion

        #region Properties

        public abstract TokenType TokenType { get; }

        protected IQueryBus QueryBus { get; }

        protected ICommandBus CommandBus { get; }

        protected ITrustedDomainHelper TrustedDomainHelper { get; }

        protected IDataProtectionProvider DataProtectionProvider { get; }

        protected abstract string TokenCookieName { get; }

        #endregion

        #region Methods

        public async Task<IActionResult> AuthorizeAsync(HttpContext httpContext, string returnUrl)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext))
                .NotNullOrWhiteSpace(returnUrl, nameof(returnUrl));

            Uri returnUri = ConvertToTrustedDomainUri(returnUrl);
            if (returnUri == null)
            {
                return new BadRequestResult();
            }

            if (string.Compare(httpContext.Request.Method, HttpMethods.Get, StringComparison.OrdinalIgnoreCase) != 0)
            {
                return new BadRequestResult();
            }

            Guid stateIdentifier = Guid.NewGuid();

            StoreStateCookie(httpContext, stateIdentifier, returnUri);
            try
            {
                return new RedirectResult((await GenerateAuthorizeUriAsync(httpContext, stateIdentifier)).AbsoluteUri);
            }
            catch
            {
                DeleteStateCookie(httpContext, stateIdentifier);
                throw;
            }
        }

        public async Task<IActionResult> AcquireTokenAsync(HttpContext httpContext, params object[] arguments)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext));

            Guid? stateIdentifier = await GetStateIdentifierAsync(httpContext, arguments);
            if (stateIdentifier.HasValue == false)
            {
                return new BadRequestResult();
            }

            Uri returnUri = RestoreStateCookie(httpContext, stateIdentifier.Value);
            if (returnUri == null)
            {
                return new BadRequestResult();
            }

            T token = await DoAcquireTokenAsync(httpContext, arguments);
            if (token == null)
            {
                return new UnauthorizedResult();
            }

            StoreTokenCookie(httpContext, token);
            try
            {
                return new RedirectResult(returnUri.AbsoluteUri);
            }
            catch
            {
                DeleteTokenCookie(httpContext);
                throw;
            }
        }

        public async Task<IActionResult> RefreshTokenAsync(HttpContext httpContext, string returnUrl)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext))
                .NotNullOrWhiteSpace(returnUrl, nameof(returnUrl));

            Uri returnUri = ConvertToTrustedDomainUri(returnUrl);
            if (returnUri == null)
            {
                return new BadRequestResult();
            }

            T token = RestoreTokenCookie(httpContext);
            if (token == null)
            {
                return new BadRequestResult();
            }

            T refreshedToken = await DoRefreshTokenAsync(httpContext, token);
            if (refreshedToken == null)
            {
                return new UnauthorizedResult();
            }

            StoreTokenCookie(httpContext, refreshedToken);

            return new RedirectResult(returnUri.AbsoluteUri);
        }

        public async Task StoreTokenAsync(HttpContext httpContext, string base64Token)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext))
                .NotNullOrWhiteSpace(base64Token, nameof(base64Token));

            T token = await GenerateTokenAsync(httpContext, base64Token);
            if (token == null)
            {
                return;
            }

            StoreTokenCookie(httpContext, token);
        }

        public Task HandleLogoutAsync(HttpContext httpContext)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext));

            return Task.Run(() => DeleteTokenCookie(httpContext));
        }

        public Task<T> GetTokenAsync(HttpContext httpContext)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext));

            return Task.Run(() => RestoreTokenCookie(httpContext));
        }

        protected abstract Task<Uri> GenerateAuthorizeUriAsync(HttpContext httpContext, Guid stateIdentifier);

        protected abstract Task<Guid?> GetStateIdentifierAsync(HttpContext httpContext, params object[] arguments);

        protected abstract Task<T> DoAcquireTokenAsync(HttpContext httpContext, params object[] arguments);

        protected abstract Task<T> DoRefreshTokenAsync(HttpContext httpContext, T expiredToken);

        protected abstract Task<T> GenerateTokenAsync(HttpContext httpContext, string base64Token);

        private Uri ConvertToTrustedDomainUri(string returnUrl)
        {
            NullGuard.NotNullOrWhiteSpace(returnUrl, nameof(returnUrl));

            if (Uri.TryCreate(returnUrl, UriKind.Absolute, out Uri returnUri) == false)
            {
                return null;
            }

            return TrustedDomainHelper.IsTrustedDomain(returnUri) == false ? null : returnUri;
        }

        private void StoreTokenCookie(HttpContext httpContext, T token)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext))
                .NotNull(token, nameof(token));

            IDataProtector dataProtector = DataProtectionProvider.CreateProtector("TokenProtection");
            byte[] tokenByteArray = dataProtector.Protect(token.ToByteArray());

            TimeSpan expireTimeSpan = (token.Expires.Kind == DateTimeKind.Utc ? token.Expires.ToLocalTime() : token.Expires).Subtract(DateTime.Now);
            httpContext.Response.Cookies.Append(TokenCookieName, Convert.ToBase64String(tokenByteArray), new CookieOptions {Expires = DateTimeOffset.Now.Add(expireTimeSpan).AddDays(1), Secure = IsHttpRequestSecure(httpContext.Request), SameSite = SameSiteMode.None});
        }

        private T RestoreTokenCookie(HttpContext httpContext)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext));

            if (httpContext.Request.Cookies.TryGetValue(TokenCookieName, out string tokenValue) == false)
            {
                return default;
            }

            if (string.IsNullOrWhiteSpace(tokenValue))
            {
                return default;
            }

            IDataProtector dataProtector = DataProtectionProvider.CreateProtector("TokenProtection");
            byte[] tokenByteArray = dataProtector.Unprotect(Convert.FromBase64String(tokenValue));

            return Token.Create<T>(tokenByteArray);
        }

        private void DeleteTokenCookie(HttpContext httpContext)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext));

            httpContext.Response.Cookies.Delete(TokenCookieName);
        }

        private void StoreStateCookie(HttpContext httpContext, Guid stateIdentifier, Uri returnUri)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext))
                .NotNull(returnUri, nameof(returnUri));

            IDataProtector dataProtector = DataProtectionProvider.CreateProtector("StateProtection");
            byte[] state = dataProtector.Protect(Encoding.UTF8.GetBytes(returnUri.AbsoluteUri));

            string stateCookieName = GetStateCookieName(stateIdentifier);
            httpContext.Response.Cookies.Append(stateCookieName, Convert.ToBase64String(state), new CookieOptions {Expires = DateTimeOffset.Now.AddMinutes(15), Secure = IsHttpRequestSecure(httpContext.Request), SameSite = SameSiteMode.None});
        }

        private Uri RestoreStateCookie(HttpContext httpContext, Guid stateIdentifier)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext));

            string stateCookieName = GetStateCookieName(stateIdentifier);
            if (httpContext.Request.Cookies.TryGetValue(stateCookieName, out string stateValue) == false)
            {
                return null;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(stateValue))
                {
                    return null;
                }

                IDataProtector dataProtector = DataProtectionProvider.CreateProtector("StateProtection");
                byte[] state = dataProtector.Unprotect(Convert.FromBase64String(stateValue));

                return ConvertToTrustedDomainUri(Encoding.UTF8.GetString(state));
            }
            finally
            {
                httpContext.Response.Cookies.Delete(stateCookieName);
            }
        }

        private void DeleteStateCookie(HttpContext httpContext, Guid stateIdentifier)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext));

            string stateCookieName = GetStateCookieName(stateIdentifier);
            httpContext.Response.Cookies.Delete(stateCookieName);
        }

        private static string GetStateCookieName(Guid stateIdentifier)
        {
            return $"OSDevGrp.OSIntranet.State.{stateIdentifier:D}";
        }

        private static bool IsHttpRequestSecure(HttpRequest httpRequest)
        {
            NullGuard.NotNull(httpRequest, nameof(httpRequest));

            bool isHttps = httpRequest.IsHttps;
            string scheme = httpRequest.Scheme;

            if (string.IsNullOrWhiteSpace(scheme))
            {
                return isHttps;
            }

            return isHttps || scheme.ToLower().EndsWith("s");
        }

        #endregion
    }
}