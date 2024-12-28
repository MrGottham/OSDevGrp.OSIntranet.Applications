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

        public static IAuthenticateAuthorizationCodeCommand BuildAuthenticateAuthorizationCodeCommand(string authorizationCode, string clientId, string clientSecret, Uri redirectUri, string authenticationType, Func<string, string> protector)
        {
            return new AuthenticateAuthorizationCodeCommand(authorizationCode, clientId, clientSecret, redirectUri, Array.Empty<Claim>(), authenticationType, new ConcurrentDictionary<string, string>(), protector);
        }

		public static IGenerateTokenCommand BuildGenerateTokenCommand()
		{
			return new GenerateTokenCommand();
		}

        public static IGenerateIdTokenCommand BuildGenerateIdTokenCommand(ClaimsIdentity claimsIdentity, DateTimeOffset authenticationTime, string nonce)
        {
            return new GenerateIdTokenCommand(claimsIdentity, authenticationTime, nonce);
        }

        public static IAcmeChallengeCommand BuildAcmeChallengeCommand(string challengeToken)
        {
            return new AcmeChallengeCommand(challengeToken);
        }

        public static IPrepareAuthorizationCodeFlowCommand BuildPrepareAuthorizationCodeFlowCommand(string responseType, string clientId, Uri redirectUri, string[] scopes, string state, string nonce, Func<byte[], byte[]> protector)
        {
            return new PrepareAuthorizationCodeFlowCommand(responseType, clientId, redirectUri, scopes, state, nonce, protector);
        }

        public static IGenerateAuthorizationCodeCommand BuildGenerateAuthorizationCodeCommand(string authorizationState, IReadOnlyCollection<Claim> claims, Func<byte[], byte[]> unprotect)
        {
            return new GenerateAuthorizationCodeCommand(authorizationState, claims, unprotect);
        }

        #endregion
    }
}