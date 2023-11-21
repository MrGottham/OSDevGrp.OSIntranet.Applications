using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Configuration;
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

        private readonly IConfiguration _configuration;

        #endregion

        #region Constructor

        public TokenGenerator(IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            _configuration = configuration;
        }

        #endregion

        #region Methods

        public IToken Generate(ClaimsIdentity claimsIdentity)
        {
            NullGuard.NotNull(claimsIdentity, nameof(claimsIdentity));

            //TODO: Handle this
            byte[] key = Encoding.Default.GetBytes(_configuration[SecurityConfigurationKeys.JwtKey]);

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