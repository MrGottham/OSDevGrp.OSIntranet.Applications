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
using System.Text;

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

            byte[] key = Encoding.Default.GetBytes(_tokenGeneratorOptions.Value.Key);

            //TODO: Handle this
            DateTime expires = DateTime.UtcNow.AddHours(1);

            //TODO: Handle this
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
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