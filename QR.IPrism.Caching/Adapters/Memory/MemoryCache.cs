using System;
using System.Linq;
using System.Runtime.Caching;

namespace QR.IPrism.Caching.Adapters.Memory
{
    /// <summary>
    /// MemoryCahce underlying provider System.Runtime.Caching.MemoryCache, it's an in-process cache
    /// This is suppose to work for Web as Windows app
    /// </summary>
    public class MemoryCache : IGenericCache
    {
        private System.Runtime.Caching.MemoryCache _cache = System.Runtime.Caching.MemoryCache.Default;

        public object Get(string cacheKey)
        {
            return _cache.Get(cacheKey);
        }

        public T Get<T>(string cacheKey) where T : class
        {
            return _cache.Get(cacheKey) as T;

        }

        public void Add(string cacheKey, DateTime absoluteExpiry, object value)
        {
            if (value != null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = new DateTimeOffset(absoluteExpiry);

                _cache.Add(cacheKey, value, policy);
            }
        }

        public void Add(string cacheKey, TimeSpan slidingExpiry, object value)
        {
            if (value != null)
            {
                _cache.Add(
                    new CacheItem(cacheKey, value),
                    new CacheItemPolicy
                    {
                        SlidingExpiration = slidingExpiry
                    }
                );
            }

        }

        public void Add(string cacheKey, object value)
        {
            if (value != null)
            {
                _cache.Add(cacheKey, value, DateTimeOffset.MaxValue);
            }
        }

        public void Remove(string cacheKey)
        {
            if (_cache.Contains(cacheKey))
            {
                _cache.Remove(cacheKey);
            }
        }

        public void RemoveAll()
        {
            _cache.ToList().ForEach
                (item =>
                {
                    _cache.Remove(item.Key);
                }
            );
        }

        public long Count
        {
            get
            {
                return _cache.GetCount();
            }
        }

        public bool Exists(string cacheKey)
        {
            return _cache[cacheKey] != null;
        }
    }
}
