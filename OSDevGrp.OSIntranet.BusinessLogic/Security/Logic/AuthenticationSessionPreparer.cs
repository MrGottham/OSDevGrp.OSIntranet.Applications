using OSDevGrp.OSIntranet.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
	public static class AuthenticationSessionPreparer
	{
		#region Methods

		public static Task<IDictionary<string, string>> PrepareAsync(this IDictionary<string, string> items, string externalTokenClaimType, string tokenType, string accessToken, string refreshToken, TimeSpan? expiresIn)
		{
			NullGuard.NotNull(items, nameof(items))
				.NotNullOrWhiteSpace(externalTokenClaimType, nameof(externalTokenClaimType))
				.NotNullOrWhiteSpace(tokenType, nameof(tokenType))
				.NotNullOrWhiteSpace(accessToken, nameof(accessToken));

			if (items.ContainsKey(AuthenticationSessionKeys.ExternalTokenClaimTypeKey) == false)
			{
				items.Add(AuthenticationSessionKeys.ExternalTokenClaimTypeKey, externalTokenClaimType);
			}

			if (items.ContainsKey(AuthenticationSessionKeys.TokenTypeKey) == false)
			{
				items.Add(AuthenticationSessionKeys.TokenTypeKey, tokenType);
			}

			if (items.ContainsKey(AuthenticationSessionKeys.AccessTokenKey) == false)
			{
				items.Add(AuthenticationSessionKeys.AccessTokenKey, accessToken);
			}

			if (items.ContainsKey(AuthenticationSessionKeys.RefreshTokenKey) == false && string.IsNullOrWhiteSpace(refreshToken) == false)
			{
				items.Add(AuthenticationSessionKeys.RefreshTokenKey, refreshToken);
			}

			if (items.ContainsKey(AuthenticationSessionKeys.ExpiresInKey) == false && expiresIn != null)
			{
				items.Add(AuthenticationSessionKeys.ExpiresInKey, expiresIn.Value.TotalSeconds.ToString(CultureInfo.InvariantCulture));
			}

			return Task.FromResult(items);
		}

		#endregion
	}
}