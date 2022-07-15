using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Configuration;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    public class TokenHelper : ITokenHelper
    {
        #region Private variables

        private readonly IConfiguration _configuration;

        #endregion

        #region Constructor

        public TokenHelper(IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            _configuration = configuration;
        }

        #endregion

        #region Methods

        public IToken Generate(IClientSecretIdentity clientSecretIdentity)
        {
            NullGuard.NotNull(clientSecretIdentity, nameof(clientSecretIdentity));

            byte[] key = Encoding.Default.GetBytes(_configuration[SecurityConfigurationKeys.JwtKey]);

            DateTime expires = DateTime.UtcNow.AddHours(1);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = clientSecretIdentity.ToClaimsIdentity(),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return new Token("Bearer", tokenHandler.WriteToken(securityToken), expires);
        }

        #endregion
    }
}