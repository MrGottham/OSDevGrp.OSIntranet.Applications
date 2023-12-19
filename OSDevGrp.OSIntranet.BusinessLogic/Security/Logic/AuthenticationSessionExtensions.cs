using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Security;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
	internal static class AuthenticationSessionExtensions
	{
		#region Private variables

		private static readonly Regex ExternalTokenClaimTypeRegex = new($"^({ClaimHelper.MicrosoftTokenClaimType}|{ClaimHelper.GoogleTokenClaimType})$", RegexOptions.Compiled);

		#endregion

		#region Methods

		internal static bool HasExternalTokenClaimType(this IDictionary<string, string> items)
		{
			NullGuard.NotNull(items, nameof(items));

			return items.HasValidItem(AuthenticationSessionKeys.ExternalTokenClaimTypeKey, ExternalTokenClaimTypeRegex.IsMatch);
		}

		internal static bool HasTokenType(this IDictionary<string, string> items)
		{
			NullGuard.NotNull(items, nameof(items));

			return items.HasValidItem(AuthenticationSessionKeys.TokenTypeKey);
		}

		internal static bool HasAccessToken(this IDictionary<string, string> items)
		{
			NullGuard.NotNull(items, nameof(items));

			return items.HasValidItem(AuthenticationSessionKeys.AccessTokenKey);
		}

		internal static bool HasRefreshToken(this IDictionary<string, string> items)
		{
			NullGuard.NotNull(items, nameof(items));

			return items.HasValidItem(AuthenticationSessionKeys.RefreshTokenKey);
		}

		internal static bool HasExpiresIn(this IDictionary<string, string> items)
		{
			NullGuard.NotNull(items, nameof(items));

			return items.HasValidItem(AuthenticationSessionKeys.ExpiresInKey, value => ConvertExpiresInToTimeSpan(value).HasValue);
		}

		internal static bool HasExpiresAt(this IDictionary<string, string> items)
		{
			NullGuard.NotNull(items, nameof(items));

			return items.HasValidItem(AuthenticationSessionKeys.ExpiresAtKey, value => ConvertExpiresAtToDateTime(value).HasValue);
		}

		internal static string ResolveExternalTokenClaimType(this IDictionary<string, string> items)
		{
			NullGuard.NotNull(items, nameof(items));

			return items.HasExternalTokenClaimType()
				? items[AuthenticationSessionKeys.ExternalTokenClaimTypeKey]
				: null;
		}

		internal static string ResolveTokenType(this IDictionary<string, string> items)
		{
			NullGuard.NotNull(items, nameof(items));

			return items.HasTokenType()
				? items[AuthenticationSessionKeys.TokenTypeKey]
				: null;
		}

		internal static string ResolveAccessToken(this IDictionary<string, string> items)
		{
			NullGuard.NotNull(items, nameof(items));

			return items.HasAccessToken()
				? items[AuthenticationSessionKeys.AccessTokenKey]
				: null;
		}

		internal static string ResolveRefreshToken(this IDictionary<string, string> items)
		{
			NullGuard.NotNull(items, nameof(items));

			return items.HasRefreshToken()
				? items[AuthenticationSessionKeys.RefreshTokenKey]
				: null;
		}

		internal static DateTime? ResolveExpires(this IDictionary<string, string> items)
		{
			NullGuard.NotNull(items, nameof(items));

			if (items.HasExpiresAt())
			{
				return ConvertExpiresAtToDateTime(items[AuthenticationSessionKeys.ExpiresAtKey]);
			}

			if (items.HasExpiresIn() == false)
			{
				return null;
			}

			TimeSpan? timeSpan = ConvertExpiresInToTimeSpan(items[AuthenticationSessionKeys.ExpiresInKey]);
			return timeSpan != null ? DateTime.UtcNow.Add(timeSpan.Value) : null;
		}

		private static bool HasValidItem(this IDictionary<string, string> items, string itemKey)
		{
			NullGuard.NotNull(items, nameof(items))
				.NotNullOrWhiteSpace(itemKey, nameof(itemKey));

			return items.HasValidItem(itemKey, _ => true);
		}

		private static bool HasValidItem(this IDictionary<string, string> items, string itemKey, Func<string, bool> validator)
		{
			NullGuard.NotNull(items, nameof(items))
				.NotNullOrWhiteSpace(itemKey, nameof(itemKey))
				.NotNull(validator, nameof(validator));

			if (items.TryGetValue(itemKey, out string value) == false)
			{
				return false;
			}

			return string.IsNullOrWhiteSpace(value) == false && validator(value);
		}

		private static TimeSpan? ConvertExpiresInToTimeSpan(string value)
		{
			NullGuard.NotNullOrWhiteSpace(value, nameof(value));

			if (double.TryParse(value, CultureInfo.InvariantCulture, out double totalSeconds) == false)
			{
				return null;
			}

			return TimeSpan.FromSeconds(totalSeconds);
		}

		private static DateTime? ConvertExpiresAtToDateTime(string value)
		{
			NullGuard.NotNullOrWhiteSpace(value, nameof(value));

			if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out DateTime expiresAt) == false)
			{
				return null;
			}

			return expiresAt.Kind == DateTimeKind.Local ? expiresAt.ToUniversalTime() : expiresAt;
		}

		#endregion
	}
}