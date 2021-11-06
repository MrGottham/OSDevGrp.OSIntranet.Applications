using System;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;

namespace OSDevGrp.OSIntranet.Domain.Common
{
    public class KeyValueEntry : AuditableBase, IKeyValueEntry
    {
        #region Constructor

        public KeyValueEntry(string key, byte[] value)
        {
            NullGuard.NotNullOrWhiteSpace(key, nameof(key))
                .NotNull(value, nameof(value));

            Key = key;
            Value = value;
        }

        #endregion

        #region Properties

        public string Key { get; }

        public byte[] Value { get; }

        public bool Deletable { get; private set; }

        #endregion

        #region Methods

        public T ToObject<T>() where T : class
        {
            return DomainHelper.FromByteArray<T>(Value);
        }

        public string ToBase64()
        {
            return Convert.ToBase64String(Value);
        }

        public void AllowDeletion()
        {
            Deletable = true;
        }

        public void DisallowDeletion()
        {
            Deletable = false;
        }

        public static IKeyValueEntry Create<T>(string key, T value) where T : class
        {
            NullGuard.NotNullOrWhiteSpace(key, nameof(key))
                .NotNull(value, nameof(value));

            return new KeyValueEntry(key, DomainHelper.ToByteArray(value));
        }

        public static IKeyValueEntry Create(string key, string base64String)
        {
            NullGuard.NotNullOrWhiteSpace(key, nameof(key))
                .NotNullOrWhiteSpace(base64String, nameof(base64String));

            return new KeyValueEntry(key, Convert.FromBase64String(base64String));
        }

        #endregion
    }
}