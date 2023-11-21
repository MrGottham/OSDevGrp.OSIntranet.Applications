using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
			return new AuthenticateClientSecretCommand(clientId, clientSecret, Array.Empty<Claim>(), authenticationType, new ReadOnlyDictionary<string, string>(new ConcurrentDictionary<string, string>()), protector);
		}

		#endregion
	}
}