using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
	internal class ExternalTokenClaimCreator : IExternalTokenClaimCreator
	{
		#region Private variables

		private readonly IExternalTokenCreator _externalTokenCreator;

		#endregion

		#region Constructor

		public ExternalTokenClaimCreator(IExternalTokenCreator externalTokenCreator)
		{
			NullGuard.NotNull(externalTokenCreator, nameof(externalTokenCreator));

			_externalTokenCreator = externalTokenCreator;
		}

		#endregion

		#region Methods

		public bool CanBuild(IDictionary<string, string> authenticationSessionItems)
		{
			NullGuard.NotNull(authenticationSessionItems, nameof(authenticationSessionItems));

			return authenticationSessionItems.HasExternalTokenClaimType() && _externalTokenCreator.CanBuild(authenticationSessionItems);
		}

		public bool CanBuild(IReadOnlyDictionary<string, string> authenticationSessionItems)
		{
			NullGuard.NotNull(authenticationSessionItems, nameof(authenticationSessionItems));

			return CanBuild((IDictionary<string, string>) authenticationSessionItems.ToDictionary(m => m.Key, m => m.Value));
		}

		public Claim Build(IDictionary<string, string> authenticationSessionItems, Func<string, string> protector)
		{
			NullGuard.NotNull(authenticationSessionItems, nameof(authenticationSessionItems))
				.NotNull(protector, nameof(protector));

			string claimType = authenticationSessionItems.ResolveExternalTokenClaimType();
			IToken token = _externalTokenCreator.Build(authenticationSessionItems);
			IRefreshableToken refreshableToken = token as IRefreshableToken;

			return refreshableToken == null
				? ClaimHelper.CreateTokenClaim(claimType, token, protector)
				: ClaimHelper.CreateTokenClaim(claimType, refreshableToken, protector);
		}

		public Claim Build(IReadOnlyDictionary<string, string> authenticationSessionItems, Func<string, string> protector)
		{
			NullGuard.NotNull(authenticationSessionItems, nameof(authenticationSessionItems))
				.NotNull(protector, nameof(protector));

			return Build((IDictionary<string, string>) authenticationSessionItems.ToDictionary(m => m.Key, m => m.Value), protector);
		}

		#endregion
	}
}