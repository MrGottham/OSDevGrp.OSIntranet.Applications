using System;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
	internal static class ValueConverter
	{
		#region Methods

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