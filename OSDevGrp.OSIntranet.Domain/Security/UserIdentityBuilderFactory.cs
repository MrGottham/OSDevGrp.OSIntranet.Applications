using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Domain.Security
{
	public static class UserIdentityBuilderFactory
	{
		#region Methods

		public static IUserIdentityBuilder Create(string externalUserIdentifier, IEnumerable<Claim> claims = null)
		{
			NullGuard.NotNullOrWhiteSpace(externalUserIdentifier, nameof(externalUserIdentifier));

			return new UserIdentityBuilder(externalUserIdentifier, claims);
		}

		#endregion
	}
}