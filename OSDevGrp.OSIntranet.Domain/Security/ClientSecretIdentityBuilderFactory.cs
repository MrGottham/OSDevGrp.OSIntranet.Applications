using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Domain.Security
{
	public static class ClientSecretIdentityBuilderFactory
	{
		#region Methods

		public static IClientSecretIdentityBuilder Create(string friendlyName, IEnumerable<Claim> claims = null)
		{
			NullGuard.NotNullOrWhiteSpace(friendlyName, nameof(friendlyName));

			return new ClientSecretIdentityBuilder(friendlyName, claims);
		}

		#endregion
	}
}