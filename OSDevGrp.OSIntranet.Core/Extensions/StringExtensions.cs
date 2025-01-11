using System;
using System.Text;

namespace OSDevGrp.OSIntranet.Core.Extensions
{
    public static class StringExtensions
    {
        #region Methods

        public static string ComputeMd5Hash(this string value)
        {
            NullGuard.NotNullOrWhiteSpace(value, nameof(value));

            return value.ComputeHash(ByteArrayExtensions.ComputeMd5Hash);
        }

        public static string ComputeSha256Hash(this string value)
        {
            NullGuard.NotNullOrWhiteSpace(value, nameof(value));

            return value.ComputeHash(ByteArrayExtensions.ComputeSha256Hash);
        }

        public static string ComputeSha384Hash(this string value)
        {
            NullGuard.NotNullOrWhiteSpace(value, nameof(value));

            return value.ComputeHash(ByteArrayExtensions.ComputeSha384Hash);
        }

        public static string ComputeSha512Hash(this string value)
        {
            NullGuard.NotNullOrWhiteSpace(value, nameof(value));

            return value.ComputeHash(ByteArrayExtensions.ComputeSha512Hash);
        }

        private static string ComputeHash(this string value, Func<byte[], byte[]> hashAlgorithm)
        {
            NullGuard.NotNullOrWhiteSpace(value, nameof(value));

            StringBuilder valueBuilder = new StringBuilder();
            foreach (byte b in hashAlgorithm(Encoding.UTF8.GetBytes(value)))
            {
                valueBuilder.Append(b.ToString("x2"));
            }

            return valueBuilder.ToString();
        }

        #endregion
    }
}