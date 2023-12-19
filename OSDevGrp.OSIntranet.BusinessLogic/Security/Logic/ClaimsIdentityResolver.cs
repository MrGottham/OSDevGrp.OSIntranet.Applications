using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
	internal class ClaimsIdentityResolver : IClaimsIdentityResolver
	{
		#region Private variables

		private readonly IPrincipalResolver _principalResolver;

		#endregion

		#region Constructor

		public ClaimsIdentityResolver(IPrincipalResolver principalResolver)
		{
			NullGuard.NotNull(principalResolver, nameof(principalResolver));

			_principalResolver = principalResolver;
		}

		#endregion

		#region Methods

		public ClaimsIdentity GetCurrentClaimsIdentity()
		{
			ClaimsPrincipal claimsPrincipal = _principalResolver.GetCurrentPrincipal() as ClaimsPrincipal;

			return claimsPrincipal?.Identity as ClaimsIdentity;
		}

		#endregion
	}
}