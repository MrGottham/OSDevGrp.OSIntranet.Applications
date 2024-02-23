using AutoFixture;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.ExternalTokenCreator
{
	public abstract class ExternalTokenCreatorTestBase
	{
		#region Methods

		protected IExternalTokenCreator CreateSut()
		{
			return new BusinessLogic.Security.Logic.ExternalTokenCreator();
		}

		protected static IDictionary<string, string> CreateAuthenticationSessionItems(Fixture fixture, Random random, bool hasTokenType = true, string tokenType = null, bool hasAccessToken = true, string accessToken = null, bool hasRefreshToken = true, string refreshToken = null, bool hasExpiresAt = true, DateTime? expiresAt = null, bool hasExpiresIn = true, TimeSpan? expiresIn = null)
		{
			NullGuard.NotNull(fixture, nameof(fixture))
				.NotNull(random, nameof(random));

			IDictionary<string, string> authenticationSessionItems = new ConcurrentDictionary<string, string>();

			if (hasTokenType)
			{
				authenticationSessionItems.Add(AuthenticationSessionKeys.TokenTypeKey, tokenType ?? fixture.Create<string>());
			}

			if (hasAccessToken)
			{
				authenticationSessionItems.Add(AuthenticationSessionKeys.AccessTokenKey, accessToken ?? fixture.Create<string>());
			}

			if (hasRefreshToken)
			{
				authenticationSessionItems.Add(AuthenticationSessionKeys.RefreshTokenKey, refreshToken ?? fixture.Create<string>());
			}

			if (hasExpiresAt)
			{
				authenticationSessionItems.Add(AuthenticationSessionKeys.ExpiresAtKey, (expiresAt ?? DateTime.UtcNow.AddSeconds(random.Next(60, 3600))).ToString("R", CultureInfo.InvariantCulture));
			}

			if (hasExpiresIn)
			{
				authenticationSessionItems.Add(AuthenticationSessionKeys.ExpiresInKey, Convert.ToString((expiresIn ?? TimeSpan.FromSeconds(random.Next(60, 3600))).TotalSeconds, CultureInfo.InvariantCulture));
			}

			return authenticationSessionItems;
		}

		#endregion
	}
}