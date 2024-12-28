using System.Security.Cryptography;

namespace OSDevGrp.OSIntranet.Core.Extensions
{
    public static class ByteArrayExtensions
    {
        #region Methods

        public static byte[] ComputeMd5Hash(this byte[] value)
        {
            NullGuard.NotNull(value, nameof(value));

            using HashAlgorithm hashAlgorithm = MD5.Create();

            return value.ComputeHash(hashAlgorithm);
        }

        public static byte[] ComputeSha256Hash(this byte[] value)
        {
            NullGuard.NotNull(value, nameof(value));

            using HashAlgorithm hashAlgorithm = SHA256.Create();

            return value.ComputeHash(hashAlgorithm);
        }

        public static byte[] ComputeSha384Hash(this byte[] value)
        {
            NullGuard.NotNull(value, nameof(value));

            using HashAlgorithm hashAlgorithm = SHA384.Create();

            return value.ComputeHash(hashAlgorithm);
        }

        public static byte[] ComputeSha512Hash(this byte[] value)
        {
            NullGuard.NotNull(value, nameof(value));

            using HashAlgorithm hashAlgorithm = SHA512.Create();

            return value.ComputeHash(hashAlgorithm);
        }

        private static byte[] ComputeHash(this byte[] value, HashAlgorithm hashAlgorithm)
        {
            NullGuard.NotNull(value, nameof(value))
                .NotNull(hashAlgorithm, nameof(hashAlgorithm));

            return hashAlgorithm.ComputeHash(value);
        }

        #endregion
    }
}