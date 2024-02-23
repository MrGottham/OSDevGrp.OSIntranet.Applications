using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Core;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Commands
{
	internal class AuthenticateClientSecretCommand : AuthenticateCommandBase, IAuthenticateClientSecretCommand
    {
        #region Constructor

        public AuthenticateClientSecretCommand(string clientId, string clientSecret, IReadOnlyCollection<Claim> claims, string authenticationType, IReadOnlyDictionary<string, string> authenticationSessionItems, Func<string, string> protector)
            : base(claims, authenticationType, authenticationSessionItems, protector)
        {
	        NullGuard.NotNullOrWhiteSpace(clientId, nameof(clientId))
		        .NotNullOrWhiteSpace(clientSecret, nameof(clientSecret));

	        ClientId = clientId;
	        ClientSecret = clientSecret;
        }

        #endregion

        #region Properties

        public string ClientId { get; }

        public string ClientSecret { get; }

        #endregion
    }
}