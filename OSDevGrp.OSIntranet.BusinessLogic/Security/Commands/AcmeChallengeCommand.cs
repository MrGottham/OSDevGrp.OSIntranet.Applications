using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Commands
{
    internal class AcmeChallengeCommand : IAcmeChallengeCommand
    {
        #region Constructor

        public AcmeChallengeCommand(string challengeToken)
        {
            NullGuard.NotNull(challengeToken, nameof(challengeToken));

            ChallengeToken = challengeToken;
        }

        #endregion

        #region Properties

        public string ChallengeToken { get; }

        #endregion

        #region Methods

        public IValidator Validate(IValidator validator)
        {
            NullGuard.NotNull(validator, nameof(validator));

            return validator.String.ShouldNotBeNullOrWhiteSpace(ChallengeToken, GetType(), nameof(ChallengeToken))
                .String.ShouldHaveMinLength(ChallengeToken, 1, GetType(), nameof(ChallengeToken));
        }

        #endregion
    }
}