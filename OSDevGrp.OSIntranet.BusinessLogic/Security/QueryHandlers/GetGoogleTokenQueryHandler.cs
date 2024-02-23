using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.QueryHandlers
{
	internal class GetGoogleTokenQueryHandler : GetExternalTokenQueryHandlerBase<IGetGoogleTokenQuery, IToken>
	{
		#region Constructor

		public GetGoogleTokenQueryHandler(IClaimResolver claimResolver)
			: base(claimResolver)
		{
		}

		#endregion

		#region Methods

		protected override IToken GetExternalToken(Func<string, string> unprotect)
		{
			NullGuard.NotNull(unprotect, nameof(unprotect));

			return ClaimResolver.GetGoogleToken(unprotect);
		}

		protected override IToken GetExternalToken(ClaimsPrincipal claimsPrincipal, Func<string, string> unprotect)
		{
			NullGuard.NotNull(claimsPrincipal, nameof(claimsPrincipal))
				.NotNull(unprotect, nameof(unprotect));

			return ClaimResolver.GetGoogleToken(claimsPrincipal, unprotect);
		}

		#endregion
	}
}