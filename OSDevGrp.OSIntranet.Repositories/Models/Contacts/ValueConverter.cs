using OSDevGrp.OSIntranet.Core;
using System.Text;

namespace OSDevGrp.OSIntranet.Repositories.Models.Contacts
{
	internal static class ValueConverter
	{
		#region Methods

		internal static string ByteArrayToExternalIdentifier(byte[] value)
		{
			NullGuard.NotNull(value, nameof(value));

			return GetExternalIdentifierEncoding().GetString(value);
		}

		internal static byte[] ExternalIdentifierToByteArray(string value)
		{
			NullGuard.NotNullOrWhiteSpace(value, nameof(value));

			return GetExternalIdentifierEncoding().GetBytes(value);
		}

		private static Encoding GetExternalIdentifierEncoding()
		{
			return Encoding.ASCII;
		}

		#endregion
	}
}