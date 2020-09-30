using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Core.Interfaces;

namespace OSDevGrp.OSIntranet.Core
{
    public class ConverterCache : IConverterCache
    {
        #region Private variabels

        private readonly IDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();

        #endregion

        #region Properties

        public object SyncRoot { get; } = new object();

        #endregion

        #region Methods

        public T FromMemory<T>(string key) where T : class
        {
            NullGuard.NotNullOrWhiteSpace(key, nameof(key));

            lock (SyncRoot)
            {
                if (_cache.TryGetValue(GenerateCacheKey<T>(key), out object value) == false)
                {
                    return default;
                }

                return value as T;
            }
        }

        public void Remember<T>(T obj, Func<T, string> keyGenerator) where T : class
        {
            NullGuard.NotNull(obj, nameof(obj))
                .NotNull(keyGenerator, nameof(keyGenerator));

            lock (SyncRoot)
            {
                _cache.Add(GenerateCacheKey<T>(keyGenerator(obj)), obj);
            }
        }

        public void Forget<T>(string key) where T : class
        {
            NullGuard.NotNullOrWhiteSpace(key, nameof(key));

            lock (SyncRoot)
            {
                string cacheKey = GenerateCacheKey<T>(key);
                while (_cache.ContainsKey(cacheKey))
                {
                    _cache.Remove(cacheKey);
                }
            }
        }

        private static string GenerateCacheKey<T>(string key)
        {
            NullGuard.NotNullOrWhiteSpace(key, nameof(key));

            return $"{typeof(T).FullName}|{key}";
        }

        #endregion
    }
}