using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands
{
	public interface IAuthenticateCommand : ICommand
	{
		IReadOnlyCollection<Claim> Claims { get; }

		string AuthenticationType { get; }

		IReadOnlyDictionary<string, string> AuthenticationSessionItems { get; }

		Func<string, string> Protector { get; }
	}
}