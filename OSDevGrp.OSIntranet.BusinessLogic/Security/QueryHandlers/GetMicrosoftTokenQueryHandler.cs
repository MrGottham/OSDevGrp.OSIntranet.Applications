using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.QueryHandlers
{
	internal class GetMicrosoftTokenQueryHandler : GetExternalTokenQueryHandlerBase<IGetMicrosoftTokenQuery, IRefreshableToken>
	{
		#region Constructor

		public GetMicrosoftTokenQueryHandler(IClaimResolver claimResolver)
			: base(claimResolver)
		{
		}

		#endregion

		#region Methods

		protected override IRefreshableToken GetExternalToken(Func<string, string> unprotect)
		{
			NullGuard.NotNull(unprotect, nameof(unprotect));

			return ClaimResolver.GetMicrosoftToken(unprotect);
		}

		protected override IRefreshableToken GetExternalToken(ClaimsPrincipal claimsPrincipal, Func<string, string> unprotect)
		{
			NullGuard.NotNull(claimsPrincipal, nameof(claimsPrincipal))
				.NotNull(unprotect, nameof(unprotect));

			return ClaimResolver.GetMicrosoftToken(claimsPrincipal, unprotect);
		}

		#endregion
	}
}