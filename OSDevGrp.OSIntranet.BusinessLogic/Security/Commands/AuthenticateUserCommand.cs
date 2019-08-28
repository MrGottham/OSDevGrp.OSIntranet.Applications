using System.Collections.Generic;
using System.Security.Claims;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Commands
{
    public class AuthenticateUserCommand : IAuthenticateUserCommand
    {
        #region Constructors

        public AuthenticateUserCommand(string externalUserIdentifier, IEnumerable<Claim> claims)
        {
            NullGuard.NotNullOrWhiteSpace(externalUserIdentifier, nameof(externalUserIdentifier))
                .NotNull(claims, nameof(claims));

            ExternalUserIdentifier = externalUserIdentifier;
            Claims = claims;
        }

        #endregion
        
        #region Properties

        public string ExternalUserIdentifier { get; }

        public IEnumerable<Claim> Claims { get; }

        #endregion
    }
}
