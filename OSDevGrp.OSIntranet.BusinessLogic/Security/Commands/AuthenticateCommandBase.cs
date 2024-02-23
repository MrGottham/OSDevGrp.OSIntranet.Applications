using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Core;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Commands
{
	internal abstract class AuthenticateCommandBase : IAuthenticateCommand
	{
		#region Constructor

		protected AuthenticateCommandBase(IReadOnlyCollection<Claim> claims, string authenticationType, IReadOnlyDictionary<string, string> authenticationSessionItems, Func<string, string> protector)
		{
			NullGuard.NotNull(claims, nameof(claims))
				.NotNullOrWhiteSpace(authenticationType, nameof(authenticationType))
				.NotNull(authenticationSessionItems, nameof(authenticationSessionItems))
				.NotNull(protector, nameof(protector));

			Claims = claims;
			AuthenticationType = authenticationType;
			AuthenticationSessionItems = authenticationSessionItems;
			Protector = protector;
		}

		#endregion

		#region Properties

		public IReadOnlyCollection<Claim> Claims { get; }

		public string AuthenticationType { get; }

		public IReadOnlyDictionary<string, string> AuthenticationSessionItems { get; }

		public Func<string, string> Protector { get; }

		#endregion
	}
}