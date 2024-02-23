using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Core;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Commands
{
	internal class AuthenticateUserCommand : AuthenticateCommandBase, IAuthenticateUserCommand
    {
        #region Constructor

        public AuthenticateUserCommand(string externalUserIdentifier, IReadOnlyCollection<Claim> claims, string authenticationType, IReadOnlyDictionary<string, string> authenticationSessionItems, Func<string, string> protector)
	        : base(claims, authenticationType, authenticationSessionItems, protector)
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