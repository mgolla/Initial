using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace QR.IPrism.Caching.Adapters.Http
{
    /// <summary>
    /// HttpCache - Underlying provider is System.Web.Caching.Cache
    /// Use only in web scenarios.
    /// </summary>
    public class HttpCache : IGenericCache
    {
        private Cache _cache;

        public HttpCache()
        {
            //Check if HttpContext.Current is available.
            _cache = HttpContext.Current != null ? HttpContext.Current.Cache : HttpRuntime.Cache;
        }

        public object Get(string cacheKey)
        {
            return _cache.Get(cacheKey);
        }

        public T Get<T>(string cacheKey) where T : class
        {
            //Will return null if the entry doesn't exists in the cache.
            return _cache.Get(cacheKey) as T;

        }

        public void Add(string cacheKey, DateTime absoluteExpiry, object value)
        {
            if (value != null)
            {
                _cache.Add(cacheKey, value, null, absoluteExpiry, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }
        }

        public void Add(string cacheKey, TimeSpan slidingExpiry, object value)
        {
            if (value != null)
            {
                _cache.Add(cacheKey, value, null, Cache.NoAbsoluteExpiration, slidingExpiry, CacheItemPriority.BelowNormal, null);
            }
        }

        public void Add(string cacheKey, object value)
        {
            if (value != null)
            {
                _cache.Add(cacheKey, value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }
        }

        public void Remove(string cacheKey)
        {
            if (_cache.Get(cacheKey) != null)
            {
                _cache.Remove(cacheKey);
            }
        }

        public void RemoveAll()
        {
            if (_cache.Count > 0)
            {
                foreach (var item in _cache)
                {
                    DictionaryEntry entry = (DictionaryEntry)item;
                    if (entry.Key != null && !string.IsNullOrWhiteSpace(entry.Key.ToString()))
                    {
                        _cache.Remove(entry.Key.ToString());
                    }
                }
            }
        }

        public long Count
        {
            get { return _cache.Count; }
        }

        public bool Exists(string cacheKey)
        {
            return _cache[cacheKey] != null;
        }
    }
}
