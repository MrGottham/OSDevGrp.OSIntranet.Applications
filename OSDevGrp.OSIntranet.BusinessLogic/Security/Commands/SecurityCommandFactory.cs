using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Commands
{
    public static class SecurityCommandFactory
	{
		#region Methods

		public static IAuthenticateUserCommand BuildAuthenticateUserCommand(string externalUserIdentifier, IReadOnlyCollection<Claim> claims, string authenticationType, IReadOnlyDictionary<string, string> authenticationSessionItems, Func<string, string> protector)
		{
			return new AuthenticateUserCommand(externalUserIdentifier, claims, authenticationType, authenticationSessionItems, protector);
		}

		public static IAuthenticateClientSecretCommand BuildAuthenticateClientSecretCommand(string clientId, string clientSecret, string authenticationType, Func<string, string> protector)
		{
			return new AuthenticateClientSecretCommand(clientId, clientSecret, Array.Empty<Claim>(), authenticationType, new ConcurrentDictionary<string, string>(), protector);
		}

		public static IGenerateTokenCommand BuildGenerateTokenCommand()
		{
			return new GenerateTokenCommand();
		}

        public static IAcmeChallengeCommand BuildAcmeChallengeCommand(string challengeToken)
        {
            return new AcmeChallengeCommand(challengeToken);
        }

        public static IPrepareAuthorizationCodeFlowCommand BuildPrepareAuthorizationCodeFlowCommand(string responseType, string clientId, Uri redirectUri, string[] scopes, string state, Func<byte[], byte[]> protector)
        {
            return new PrepareAuthorizationCodeFlowCommand(responseType, clientId, redirectUri, scopes, state, protector);
        }

        #endregion
    }
}