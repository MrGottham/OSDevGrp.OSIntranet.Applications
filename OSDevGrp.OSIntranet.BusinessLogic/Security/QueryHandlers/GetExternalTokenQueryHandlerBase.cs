using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.QueryHandlers
{
	internal abstract class GetExternalTokenQueryHandlerBase<TGetExternalTokenQuery, TToken> : IQueryHandler<TGetExternalTokenQuery, TToken> where TGetExternalTokenQuery : IGetExternalTokenQuery where TToken : IToken
	{
		#region Constructor

		protected GetExternalTokenQueryHandlerBase(IClaimResolver claimResolver)
		{
			NullGuard.NotNull(claimResolver, nameof(claimResolver));

			ClaimResolver = claimResolver;
		}

		#endregion

		#region Properties

		protected IClaimResolver ClaimResolver { get; }

		#endregion

		#region Methods

		public Task<TToken> QueryAsync(TGetExternalTokenQuery query)
		{
			NullGuard.NotNull(query, nameof(query));

			return Task.Run(() =>
			{
				ClaimsPrincipal principal = query.ClaimsPrincipal;
				Func<string, string> unprotect = query.Unprotect;

				return principal != null
					? GetExternalToken(principal, unprotect)
					: GetExternalToken(unprotect);
			});
		}

		protected abstract TToken GetExternalToken(Func<string, string> unprotect);

		protected abstract TToken GetExternalToken(ClaimsPrincipal claimsPrincipal, Func<string, string> unprotect);

		#endregion
	}
}