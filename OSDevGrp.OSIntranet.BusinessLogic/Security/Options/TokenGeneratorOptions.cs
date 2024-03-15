using Microsoft.IdentityModel.Tokens;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using System;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Options
{
    public class TokenGeneratorOptions
	{
		public JsonWebKey Key { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }

    public static class TokenGeneratorOptionsExtensions
    {
        #region Method

        public static ISecurityKeyBuilder ToSecurityKeyBuilder(this TokenGeneratorOptions tokenGeneratorOptions)
        {
            NullGuard.NotNull(tokenGeneratorOptions, nameof(tokenGeneratorOptions));

            return new SecurityKeyBuilder(tokenGeneratorOptions.Key);
        }

        public static TokenValidationParameters ToTokenValidationParameters(this TokenGeneratorOptions tokenGeneratorOptions)
        {
            NullGuard.NotNull(tokenGeneratorOptions, nameof(tokenGeneratorOptions));

            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = tokenGeneratorOptions.Issuer,
                ValidIssuers =
                [
                    tokenGeneratorOptions.Issuer
                ],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = tokenGeneratorOptions.ToSecurityKeyBuilder().Build(),
                ValidateAudience = true,
                ValidAudience = tokenGeneratorOptions.Audience,
                ValidAudiences =
                [
                    tokenGeneratorOptions.Audience
                ],
                ValidateLifetime = true,
                LifetimeValidator = LocalLifetimeValidator,
                RequireExpirationTime = true,
                ValidateTokenReplay = true,
                IncludeTokenOnFailedValidation = false,
                NameClaimType = ClaimTypes.Name,
                RoleClaimType = ClaimTypes.Role
            };
        }

        private static bool LocalLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters tokenValidationParameters)
        {
            NullGuard.NotNull(securityToken, nameof(securityToken))
                .NotNull(tokenValidationParameters, nameof(tokenValidationParameters));

            if (tokenValidationParameters.ValidateLifetime == false)
            {
                return true;
            }

            if (notBefore == null || expires == null)
            {
                return false;
            }

            if ((notBefore.Value.Kind == DateTimeKind.Utc ? notBefore.Value : notBefore.Value.ToUniversalTime()) > DateTime.UtcNow)
            {
                return false;
            }

            return (expires.Value.Kind == DateTimeKind.Utc ? expires.Value : expires.Value.ToUniversalTime()) > DateTime.UtcNow;
        }

        #endregion
    }
}