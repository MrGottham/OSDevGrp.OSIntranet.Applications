using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal class TokenGenerator : ITokenGenerator
	{
        #region Private variables

        private readonly IOptions<TokenGeneratorOptions> _tokenGeneratorOptions;
        private readonly TimeProvider _timeProvider;

        #endregion

        #region Constructor

        public TokenGenerator(IOptions<TokenGeneratorOptions> tokenGeneratorOptions, TimeProvider timeProvider)
        {
            NullGuard.NotNull(tokenGeneratorOptions, nameof(tokenGeneratorOptions))
                .NotNull(timeProvider, nameof(timeProvider));

            _tokenGeneratorOptions = tokenGeneratorOptions;
            _timeProvider = timeProvider;
        }

        #endregion

        #region Properties

        public string SigningAlgorithm => SecurityAlgorithms.RsaSha256;

        #endregion

        #region Methods

        public IToken Generate(ClaimsIdentity claimsIdentity, TimeSpan expiresIn, string audience = null)
        {
            NullGuard.NotNull(claimsIdentity, nameof(claimsIdentity));

            TokenGeneratorOptions tokenGeneratorOptions = _tokenGeneratorOptions.Value;
            using ISecurityKeyBuilder securityKeyBuilder = new SecurityKeyBuilder(tokenGeneratorOptions.Key);
            DateTime expires = _timeProvider.GetUtcNow().Add(expiresIn).UtcDateTime;

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Issuer = tokenGeneratorOptions.Issuer,
                SigningCredentials = new SigningCredentials(securityKeyBuilder.Build(), SigningAlgorithm)
                {
                    CryptoProviderFactory = new CryptoProviderFactory
                    {
                        CacheSignatureProviders = false
                    }
                },
                Audience = string.IsNullOrWhiteSpace(audience) ? tokenGeneratorOptions.Audience : audience,
                Expires = expires
            };
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return TokenFactory.Create()
	            .WithTokenType("Bearer")
	            .WithAccessToken(tokenHandler.WriteToken(securityToken))
	            .WithExpires(expires)
	            .Build();
        }

        #endregion
    }
}