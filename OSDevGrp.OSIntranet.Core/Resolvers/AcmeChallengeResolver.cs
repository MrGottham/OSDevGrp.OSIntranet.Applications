using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core.Interfaces.Configuration;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;

namespace OSDevGrp.OSIntranet.Core.Resolvers
{
    public class AcmeChallengeResolver : IAcmeChallengeResolver
    {
        #region Private varibales

        private readonly IConfiguration _configuration;

        #endregion

        #region Constructor

        public AcmeChallengeResolver(IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            _configuration = configuration;
        }

        #endregion

        #region Methods

        public string GetWellKnownChallengeToken()
        {
            string wellKnownChallengeToken = _configuration[SecurityConfigurationKeys.AcmeChallengeWellKnownChallengeToken];
            return string.IsNullOrWhiteSpace(wellKnownChallengeToken) ? null : wellKnownChallengeToken;
        }

        public string GetConstructedKeyAuthorization(string challengeToken)
        {
            NullGuard.NotNullOrWhiteSpace(challengeToken, nameof(challengeToken));

            string wellKnownChallengeToken = GetWellKnownChallengeToken();
            if (string.IsNullOrWhiteSpace(wellKnownChallengeToken) || string.CompareOrdinal(wellKnownChallengeToken, challengeToken) != 0)
            {
                return null;
            }

            string constructedKeyAuthorization = _configuration[SecurityConfigurationKeys.AcmeChallengeConstructedKeyAuthorization];
            return string.IsNullOrWhiteSpace(constructedKeyAuthorization) ? null : constructedKeyAuthorization;
        }

        #endregion
    }
}