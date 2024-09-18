using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.QueryHandlers
{
    internal class GetJsonWebKeySetQueryHandler : IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet>
    {
        #region Private constants

        private static readonly Guid TokenSecurityKeyId = Guid.Parse("F6498959-0000-41A3-BC87-B82EF23D69A1");

        #endregion

        #region Private variables

        private readonly IOptions<TokenGeneratorOptions> _tokenGeneratorOptions;
        private readonly ITokenGenerator _tokenGenerator;

        #endregion

        #region Constructor

        public GetJsonWebKeySetQueryHandler(IOptions<TokenGeneratorOptions> tokenGeneratorOptions, ITokenGenerator tokenGenerator)
        {
            NullGuard.NotNull(tokenGeneratorOptions, nameof(tokenGeneratorOptions))
                .NotNull(tokenGenerator, nameof(tokenGenerator));

            _tokenGeneratorOptions = tokenGeneratorOptions;
            _tokenGenerator = tokenGenerator;
        }

        #endregion

        #region Methods

        public Task<JsonWebKeySet> QueryAsync(IGetJsonWebKeySetQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return Task.Run(() =>
            {
                using ISecurityKeyBuilder tokenSecurityKeyBuilder = new SecurityKeyBuilder(_tokenGeneratorOptions.Value.Key);

                JsonWebKeySet jsonWebKeySet = new JsonWebKeySet();
                jsonWebKeySet.Keys.Add(BuildJsonWebKey(tokenSecurityKeyBuilder.Build(), false, JsonWebKeyUseNames.Sig, _tokenGenerator.SigningAlgorithm, TokenSecurityKeyId));
                return jsonWebKeySet;
            });
        }

        private static JsonWebKey BuildJsonWebKey(SecurityKey securityKey, bool includePrivateParameters, string publicKeyUse, string algorithm, Guid keyId)
        {
            NullGuard.NotNull(securityKey, nameof(securityKey))
                .NotNullOrWhiteSpace(publicKeyUse, nameof(publicKeyUse))
                .NotNullOrWhiteSpace(algorithm, nameof(algorithm));

            return BuildJsonWebKey(securityKey as RsaSecurityKey, includePrivateParameters, publicKeyUse, algorithm, keyId);
        }

        private static JsonWebKey BuildJsonWebKey(RsaSecurityKey securityKey, bool includePrivateParameters, string publicKeyUse, string algorithm, Guid keyId)
        {
            NullGuard.NotNull(securityKey, nameof(securityKey))
                .NotNullOrWhiteSpace(publicKeyUse, nameof(publicKeyUse))
                .NotNullOrWhiteSpace(algorithm, nameof(algorithm));

            RSAParameters rsaParameters = securityKey.Rsa.ExportParameters(includePrivateParameters);
            RsaSecurityKey rsaSecurityKey = new RsaSecurityKey(rsaParameters);

            JsonWebKey jsonWebKey = JsonWebKeyConverter.ConvertFromSecurityKey(rsaSecurityKey);
            jsonWebKey.Use ??= publicKeyUse;
            jsonWebKey.Alg ??= algorithm;
            jsonWebKey.Kid ??= keyId.ToString("D").ToLower();
            return jsonWebKey;
        }

        #endregion
    }
}