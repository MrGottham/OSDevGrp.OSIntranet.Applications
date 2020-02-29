using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;

namespace OSDevGrp.OSIntranet.Mvc.Helpers.Security
{
    public class TokenHelperFactory : ITokenHelperFactory
    {
        #region Private variables

        private readonly IDictionary<TokenType, ITokenHelper> _tokenHelperDictionary;

        #endregion

        #region Constructor

        public TokenHelperFactory(IEnumerable<ITokenHelper> tokenHelperCollection)
        {
            NullGuard.NotNull(tokenHelperCollection, nameof(tokenHelperCollection));

            _tokenHelperDictionary = tokenHelperCollection.ToDictionary(tokenHelper => tokenHelper.TokenType, tokenHelper => tokenHelper);
        }

        #endregion

        #region Methods

        public Task<IActionResult> AuthorizeAsync(TokenType tokenType, HttpContext httpContext, string returnUrl)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext))
                .NotNullOrWhiteSpace(returnUrl, nameof(returnUrl));

            return Resolve(tokenType, MethodBase.GetCurrentMethod()).AuthorizeAsync(httpContext, returnUrl);
        }

        public Task<IActionResult> AcquireTokenAsync(TokenType tokenType, HttpContext httpContext, params object[] arguments)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext));

            return Resolve(tokenType, MethodBase.GetCurrentMethod()).AcquireTokenAsync(httpContext, arguments);
        }

        public Task<IActionResult> RefreshTokenAsync(TokenType tokenType, HttpContext httpContext, string returnUrl)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext))
                .NotNullOrWhiteSpace(returnUrl, nameof(returnUrl));

            return Resolve(tokenType, MethodBase.GetCurrentMethod()).RefreshTokenAsync(httpContext, returnUrl);
        }

        public Task<T> GetTokenAsync<T>(TokenType tokenType, HttpContext httpContext) where T : class, IToken
        {
            NullGuard.NotNull(httpContext, nameof(httpContext));

            return Resolve<T>(tokenType, MethodBase.GetCurrentMethod()).GetTokenAsync(httpContext);
        }

        public Task StoreTokenAsync(TokenType tokenType, HttpContext httpContext, string base64Token)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext))
                .NotNullOrWhiteSpace(base64Token, nameof(base64Token));

            return Resolve(tokenType, MethodBase.GetCurrentMethod()).StoreTokenAsync(httpContext, base64Token);
        }

        public async Task HandleLogoutAsync(HttpContext httpContext)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext));

            await Task.WhenAll(_tokenHelperDictionary.Select(m => m.Value.HandleLogoutAsync(httpContext)).ToArray());
        }

        private ITokenHelper Resolve(TokenType tokenType, MethodBase methodBase)
        {
            NullGuard.NotNull(methodBase, nameof(methodBase));

            if (_tokenHelperDictionary.ContainsKey(tokenType) == false)
            {
                throw new NotSupportedException($"The token type '{tokenType}' is not supported within the method '{methodBase.Name}'.");
            }

            return _tokenHelperDictionary[tokenType];
        }

        private ITokenHelper<T> Resolve<T>(TokenType tokenType, MethodBase methodBase) where T : class, IToken
        {
            NullGuard.NotNull(methodBase, nameof(methodBase));

            if (Resolve(tokenType, methodBase) is ITokenHelper<T> tokenHelper)
            {
                return tokenHelper;
            }

            throw new NotSupportedException($"The helper logic for the token type '{tokenType}' does not support the generic type '{typeof(T).Name}' within the method '{methodBase.Name}'.");
        }

        #endregion
    }
}