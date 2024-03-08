using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Core.Options;

namespace OSDevGrp.OSIntranet.Core.Resolvers
{
    internal class AcmeChallengeResolver : IAcmeChallengeResolver
    {
        #region Private varibales

        private readonly IOptions<AcmeChallengeOptions> _acmeChallengeOptions;

        #endregion

        #region Constructor

        public AcmeChallengeResolver(IOptions<AcmeChallengeOptions> acmeChallengeOptions)
        {
            NullGuard.NotNull(acmeChallengeOptions, nameof(acmeChallengeOptions));

            _acmeChallengeOptions = acmeChallengeOptions;
        }

        #endregion

        #region Methods

        public string GetWellKnownChallengeToken()
        {
            string wellKnownChallengeToken = _acmeChallengeOptions.Value.WellKnownChallengeToken;
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

            string constructedKeyAuthorization = _acmeChallengeOptions.Value.ConstructedKeyAuthorization;
            return string.IsNullOrWhiteSpace(constructedKeyAuthorization) ? null : constructedKeyAuthorization;
        }

        #endregion
    }
}