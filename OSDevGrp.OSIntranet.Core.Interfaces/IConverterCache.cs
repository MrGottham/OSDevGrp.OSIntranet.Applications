using System;

namespace OSDevGrp.OSIntranet.Core.Interfaces
{
    public interface IConverterCache
    {
        object SyncRoot { get; }

        T FromMemory<T>(string key) where T : class;

        void Remember<T>(T obj, Func<T, string> keyGenerator) where T : class;

        void Forget<T>(string key) where T : class;
    }
}