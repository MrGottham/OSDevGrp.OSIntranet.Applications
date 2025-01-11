using Microsoft.AspNetCore.Http;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Mvc.Helpers.Security
{
    internal static class TokenHelperFactoryExtensions
    {
        #region Methods

        internal static Task StoreExternalTokensInSecurityToken(this ITokenHelperFactory tokenHelperFactory, HttpContext httpContext, JwtSecurityToken securityToken)
        {
            NullGuard.NotNull(tokenHelperFactory, nameof(tokenHelperFactory))
                .NotNull(httpContext, nameof(httpContext))
                .NotNull(securityToken, nameof(securityToken));

            if (securityToken.Claims == null || securityToken.Claims.Any() == false)
            {
                return Task.CompletedTask;
            }

            IRefreshableToken microsoftToken = ResolveExternalToken(ClaimHelper.MicrosoftTokenClaimType, securityToken.Claims, BuildRefreshableToken);
            if (microsoftToken != null)
            {
                tokenHelperFactory.StoreMicrosoftToken(httpContext, microsoftToken);
            }

            IToken googleToken = ResolveExternalToken(ClaimHelper.GoogleTokenClaimType, securityToken.Claims, BuildToken);
            if (googleToken != null)
            {
                tokenHelperFactory.StoreGoogleToken(httpContext, googleToken);
            }

            return Task.CompletedTask;
        }

        internal static Task StoreMicrosoftToken(this ITokenHelperFactory tokenHelperFactory, HttpContext httpContext, IRefreshableToken microsoftToken)
        {
            NullGuard.NotNull(tokenHelperFactory, nameof(tokenHelperFactory))
                .NotNull(httpContext, nameof(httpContext))
                .NotNull(microsoftToken, nameof(microsoftToken));

            return tokenHelperFactory.StoreTokenAsync(TokenType.MicrosoftGraphToken, httpContext, microsoftToken.ToBase64String());
        }

        internal static Task StoreGoogleToken(this ITokenHelperFactory tokenHelperFactory, HttpContext httpContext, IToken googleToken)
        {
            NullGuard.NotNull(tokenHelperFactory, nameof(tokenHelperFactory))
                .NotNull(httpContext, nameof(httpContext))
                .NotNull(googleToken, nameof(googleToken));

            return Task.CompletedTask;
        }

        private static TToken ResolveExternalToken<TToken>(string externalTokenClaimType, IEnumerable<Claim> claims, Func<string, TToken> tokenBuilder) where TToken : class, IToken
        {
            NullGuard.NotNullOrWhiteSpace(externalTokenClaimType, nameof(externalTokenClaimType))
                .NotNull(claims, nameof(claims))
                .NotNull(tokenBuilder, nameof(tokenBuilder));

            Claim externalTokenClaim = claims.SingleOrDefault(claim => claim.Type == externalTokenClaimType);
            if (externalTokenClaim == null || string.IsNullOrWhiteSpace(externalTokenClaim.Value))
            {
                return null;
            }

            return tokenBuilder(externalTokenClaim.Value);
        }

        private static IToken BuildToken(string tokenAsBase64String)
        {
            NullGuard.NotNullOrWhiteSpace(tokenAsBase64String, tokenAsBase64String);

            return TokenFactory.Create().FromBase64String(tokenAsBase64String);
        }

        private static IRefreshableToken BuildRefreshableToken(string tokenAsBase64String)
        {
            NullGuard.NotNullOrWhiteSpace(tokenAsBase64String, tokenAsBase64String);

            return RefreshableTokenFactory.Create().FromBase64String(tokenAsBase64String);
        }

        #endregion
    }
}