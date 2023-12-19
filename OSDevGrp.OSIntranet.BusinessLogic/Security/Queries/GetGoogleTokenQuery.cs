using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using System;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Queries
{
	internal class GetGoogleTokenQuery : GetExternalTokenQueryBase, IGetGoogleTokenQuery
	{
		#region Constructors

		public GetGoogleTokenQuery(Func<string, string> unprotect)
			: base(unprotect)
		{
		}

		public GetGoogleTokenQuery(ClaimsPrincipal claimsPrincipal, Func<string, string> unprotect)
			: base(claimsPrincipal, unprotect)
		{
		}

		#endregion
	}
}