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
    public class TokenGenerator : ITokenGenerator
	{
        #region Private variables

        private readonly IOptions<TokenGeneratorOptions> _tokenGeneratorOptions;

        #endregion

        #region Constructor

        public TokenGenerator(IOptions<TokenGeneratorOptions> tokenGeneratorOptions)
        {
            NullGuard.NotNull(tokenGeneratorOptions, nameof(tokenGeneratorOptions));

            _tokenGeneratorOptions = tokenGeneratorOptions;
        }

        #endregion

        #region Methods

        public IToken Generate(ClaimsIdentity claimsIdentity)
        {
            NullGuard.NotNull(claimsIdentity, nameof(claimsIdentity));

            TokenGeneratorOptions tokenGeneratorOptions = _tokenGeneratorOptions.Value;
            using ISecurityKeyBuilder securityKeyBuilder = new SecurityKeyBuilder(tokenGeneratorOptions.Key);
            DateTime expires = DateTime.UtcNow.AddHours(1);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Issuer = tokenGeneratorOptions.Issuer,
                SigningCredentials = new SigningCredentials(securityKeyBuilder.Build(), SecurityAlgorithms.RsaSha256Signature),
                Audience = tokenGeneratorOptions.Audience,
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