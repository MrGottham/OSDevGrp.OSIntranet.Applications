using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
	internal class ExternalTokenCreator : IExternalTokenCreator
	{
		#region Methods

		public bool CanBuild(IDictionary<string, string> authenticationSessionItems)
		{
			NullGuard.NotNull(authenticationSessionItems, nameof(authenticationSessionItems));

			return authenticationSessionItems.HasTokenType() &&
			       authenticationSessionItems.HasAccessToken() &&
			       (authenticationSessionItems.HasExpiresAt() || authenticationSessionItems.HasExpiresIn());
		}

		public bool CanBuild(IReadOnlyDictionary<string, string> authenticationSessionItems)
		{
			NullGuard.NotNull(authenticationSessionItems, nameof(authenticationSessionItems));

			return CanBuild((IDictionary<string, string>) authenticationSessionItems.ToDictionary(m => m.Key, m => m.Value));
		}

		public IToken Build(IDictionary<string, string> authenticationSessionItems)
		{
			NullGuard.NotNull(authenticationSessionItems, nameof(authenticationSessionItems));

			if (authenticationSessionItems.HasRefreshToken() == false)
			{
				return TokenFactory.Create()
					.WithTokenType(authenticationSessionItems.ResolveTokenType())
					.WithAccessToken(authenticationSessionItems.ResolveAccessToken())
					.WithExpires(authenticationSessionItems.ResolveExpires()!.Value)
					.Build();
			}

			return RefreshableTokenFactory.Create()
				.WithTokenType(authenticationSessionItems.ResolveTokenType())
				.WithAccessToken(authenticationSessionItems.ResolveAccessToken())
				.WithRefreshToken(authenticationSessionItems.ResolveRefreshToken())
				.WithExpires(authenticationSessionItems.ResolveExpires()!.Value)
				.Build();
		}

		public IToken Build(IReadOnlyDictionary<string, string> authenticationSessionItems)
		{
			NullGuard.NotNull(authenticationSessionItems, nameof(authenticationSessionItems));

			return Build((IDictionary<string, string>) authenticationSessionItems.ToDictionary(m => m.Key, m => m.Value));
		}

		#endregion
	}
}