using OSDevGrp.OSIntranet.Core;
using System.Text.Json;

namespace OSDevGrp.OSIntranet.Domain.Core
{
	internal static class DomainHelper
    {
        internal static byte[] ToByteArray<T>(T value) where T : class
        {
            NullGuard.NotNull(value, nameof(value));

            return JsonSerializer.SerializeToUtf8Bytes(value, value.GetType(), GetJsonSerializerOptions());
        }

        internal static T FromByteArray<T>(byte[] byteArray) where T : class
        {
            NullGuard.NotNull(byteArray, nameof(byteArray));

            return JsonSerializer.Deserialize<T>(byteArray, GetJsonSerializerOptions());
        }

        private static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions
            {
                WriteIndented = false,
            };
        }
    }
}