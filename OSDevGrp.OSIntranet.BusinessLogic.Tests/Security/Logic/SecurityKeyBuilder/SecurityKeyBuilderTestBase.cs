using Microsoft.IdentityModel.Tokens;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using System.Security.Cryptography;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.SecurityKeyBuilder
{
    public abstract class SecurityKeyBuilderTestBase
    {
        protected static ISecurityKeyBuilder CreateSut(JsonWebKey jsonWebKey = null)
        {
            return new BusinessLogic.Security.Logic.SecurityKeyBuilder(jsonWebKey ?? CreateJsonWebKey());
        }

        protected static JsonWebKey CreateJsonWebKey()
        {
            using RSA rsa = RSA.Create(4096);

            RSAParameters rsaParameters = rsa.ExportParameters(true);
            RsaSecurityKey rsaSecurityKey = new RsaSecurityKey(rsaParameters);

            return JsonWebKeyConverter.ConvertFromRSASecurityKey(rsaSecurityKey);
        }
    }
}