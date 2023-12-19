using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using System;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Queries
{
	internal class GetMicrosoftTokenQuery : GetExternalTokenQueryBase, IGetMicrosoftTokenQuery
	{
		#region Constructors

		public GetMicrosoftTokenQuery(Func<string, string> unprotect) 
			: base(unprotect)
		{
		}

		public GetMicrosoftTokenQuery(ClaimsPrincipal claimsPrincipal, Func<string, string> unprotect)
			: base(claimsPrincipal, unprotect)
		{
		}

		#endregion
	}
}