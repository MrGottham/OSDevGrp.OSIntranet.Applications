using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.Core;
using System;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Queries
{
	internal abstract class GetExternalTokenQueryBase : IGetExternalTokenQuery
	{
		#region Constructors

		protected GetExternalTokenQueryBase(Func<string, string> unprotect)
		{
			NullGuard.NotNull(unprotect, nameof(unprotect));

			Unprotect = unprotect;
		}

		protected GetExternalTokenQueryBase(ClaimsPrincipal claimsPrincipal, Func<string, string> unprotect)
		{
			NullGuard.NotNull(claimsPrincipal, nameof(claimsPrincipal))
				.NotNull(unprotect, nameof(unprotect));

			ClaimsPrincipal = claimsPrincipal;
			Unprotect = unprotect;
		}

		#endregion

		#region Methods

		public ClaimsPrincipal ClaimsPrincipal { get; }

		public Func<string, string> Unprotect { get; }

		#endregion
	}
}