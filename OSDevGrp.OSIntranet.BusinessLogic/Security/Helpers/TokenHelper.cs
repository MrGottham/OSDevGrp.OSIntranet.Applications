using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Helpers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Helpers
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

        public string Generate(IClientSecretIdentity clientSecretIdentity)
        {
            NullGuard.NotNull(clientSecretIdentity, nameof(clientSecretIdentity));

            byte[] key = Encoding.Default.GetBytes(_configuration["Security:JWT:Key"]);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = clientSecretIdentity.ToClaimsIdentity(),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }

        #endregion
    }
}
