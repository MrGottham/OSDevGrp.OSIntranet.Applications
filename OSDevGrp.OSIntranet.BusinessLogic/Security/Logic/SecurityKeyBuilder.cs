using Microsoft.IdentityModel.Tokens;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using System.Security.Cryptography;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal class SecurityKeyBuilder : ISecurityKeyBuilder
    {
        #region Private variables

        private readonly RSA _rsa;
        private bool _disposed;

        #endregion

        #region Constructor

        public SecurityKeyBuilder(JsonWebKey jsonWebKey)
        {
            NullGuard.NotNull(jsonWebKey, nameof(jsonWebKey));

            _rsa = RSA.Create(ConvertToRsaParameters(jsonWebKey));
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _rsa.Dispose();

            _disposed = true;
        }

        public SecurityKey Build()
        {
            return new RsaSecurityKey(_rsa);
        }

        private static RSAParameters ConvertToRsaParameters(JsonWebKey jsonWebKey)
        {
            NullGuard.NotNull(jsonWebKey, nameof(jsonWebKey));

            return new RSAParameters
            {
                Modulus = Base64UrlEncoder.DecodeBytes(jsonWebKey.N),
                Exponent = Base64UrlEncoder.DecodeBytes(jsonWebKey.E),
                D = Base64UrlEncoder.DecodeBytes(jsonWebKey.D),
                DP = Base64UrlEncoder.DecodeBytes(jsonWebKey.DP),
                DQ = Base64UrlEncoder.DecodeBytes(jsonWebKey.DQ),
                P = Base64UrlEncoder.DecodeBytes(jsonWebKey.P),
                Q = Base64UrlEncoder.DecodeBytes(jsonWebKey.Q),
                InverseQ = Base64UrlEncoder.DecodeBytes(jsonWebKey.QI)
            };
        }

        #endregion
    }
}