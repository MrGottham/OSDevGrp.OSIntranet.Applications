using OSDevGrp.OSIntranet.Core;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
	internal static class ValueConverter
	{
		#region Methods

		internal static string GuidToString(Guid value)
		{
			return value.ToString("D").ToUpper();
		}

		internal static Guid StringToGuid(string value)
		{
			NullGuard.NotNullOrWhiteSpace(value, nameof(value));

			return Guid.Parse(value);
		}

		internal static string UriToString(Uri value)
		{
			if (value == null || string.IsNullOrWhiteSpace(value.AbsoluteUri))
			{
				return null;
			}

			return value.AbsoluteUri;
		}

		internal static Uri StringToUri(string value)
		{
			return string.IsNullOrWhiteSpace(value) == false ? new Uri(value) : null;
		}

		internal static string ByteArrayToString(byte[] value)
		{
			if (value == null || value.Length == 0)
			{
				return null;
			}

			return Convert.ToBase64String(value);
		}

		internal static byte[] StringToByteArray(string value)
		{
			return string.IsNullOrWhiteSpace(value) == false ? Convert.FromBase64String(value) : Array.Empty<byte>();
		}

		#endregion
	}
}