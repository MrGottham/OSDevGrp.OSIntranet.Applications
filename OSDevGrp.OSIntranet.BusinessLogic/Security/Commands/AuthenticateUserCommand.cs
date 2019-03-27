using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Commands
{
    public class AuthenticateUserCommand : IAuthenticateUserCommand
    {
        #region Constructor

        public AuthenticateUserCommand(string externalUserIdentifier)
        {
            NullGuard.NotNullOrWhiteSpace(externalUserIdentifier, nameof(externalUserIdentifier));

            ExternalUserIdentifier = externalUserIdentifier;
        }

        #endregion
        
        #region Properties

        public string ExternalUserIdentifier { get; }

        #endregion
    }
}
