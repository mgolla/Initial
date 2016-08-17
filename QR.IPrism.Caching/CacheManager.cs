using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using QR.IPrism.Caching.Adapters;
using QR.IPrism.Caching.Config;
using QR.IPrism.Caching.Provider;

namespace QR.IPrism.Caching
{
    /// <summary>
    /// Cache Handler, this provides static methods for adding/removing/querying the underlying cache.
    /// The underlying cache may be in-process/out-process. 
    /// For determining caching strategy please refer hosting application's configuration file.
    /// </summary>
    public sealed class CacheManager
    {
        #region Private Variables
        /// <summary>
        /// Underlying cache provider
        /// </summary>
        private static IGenericCache cache;
        #endregion

        #region Constructors
        /// <summary>
        /// Hint to the compiler to not create a default one for me
        /// </summary>
        private CacheManager() { }

        /// <summary>
        /// Instantiates the caching strategy as per configuration file. 
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1810", Justification = "Don't want to keep config as static", Scope = "Just for this method")]
        static CacheManager()
        {
            var config = (CacheAdapterConfig)ConfigurationManager.GetSection(Constants.CacheSection);
            cache = CacheFactory.GetAdapter(config);
        }
        #endregion

        #region Public Static Caching Methods
        /// <summary>
        /// Get from Cache for the key specified
        /// </summary>
        /// <param name="cacheKey">Unique Key that will identify the cache</param>
        /// <returns>Cached object</returns>
        public static object Get(string cacheKey)
        {
            return cache.Get(cacheKey);
        }

        /// <summary>
        /// Get from Cache for the key specified and converts to Generic
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">Unique Key that will identify the cache</param>
        /// <returns>Cached object of type T</returns>
        public static T Get<T>(string cacheKey) where T : class, new()
        {
            return cache.Get<T>(cacheKey);
        }

        /// <summary>
        /// Save data to cache, this one sets the cache for ever, this will only be cleared once app is recycled
        /// </summary>
        /// <param name="cacheKey">Unique Key that will identify the cache</param>
        /// <param name="oValue">object to be cached</param>
        public static void Set(string cacheKey, object oValue)
        {
            Clear(cacheKey);
            cache.Add(cacheKey, oValue);
        }

        /// <summary>
        /// Save data to cache, this one sets the cache for ever, this will only be cleared once app is recycled
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">Unique Key that will identify the cache</param>
        /// <param name="typeValue">object of type T to be cached</param>
        public static void Set<T>(string cacheKey, T typeValue) where T : class
        {
            Clear(cacheKey);
            cache.Add(cacheKey, typeValue);
        }

        /// <summary>
        /// Caches typeValue for minutesToCache minutes. 
        /// </summary>
        /// <typeparam name="T">Type of the object to be cached</typeparam>
        /// <param name="cacheKey">Unique Key that will identify the cache</param>
        /// <param name="typeValue">Object to be cached</param>
        /// <param name="minutesToCache">How long the data need to be cached in minutes</param>
        /// <param name="expirationStrategy">Expiration Strategy (Either Absolute/Sliding) - Default - Absolute</param>
        public static void Set<T>(string cacheKey, T typeValue, int minutesToCache, ExpirationStrategy expirationStrategy = ExpirationStrategy.Absolute) where T : class
        {
            Set<T>(cacheKey, typeValue, 0, minutesToCache, 0, expirationStrategy);
        }

        /// <summary>
        /// Caches typeValue for hours:minutes:seconds to be cached. 
        /// </summary>
        /// <typeparam name="T">Type of the object to be cached</typeparam>
        /// <param name="cacheKey">Unique Key that will identify the cache</param>
        /// <param name="typeValue">Object to be cached</param>
        /// <param name="hours">hours for forming Timespan</param>
        /// <param name="minutes">minutes for forming Timespan</param>
        /// <param name="seconds">seconds for forming Timespan</param>
        /// <param name="expirationStrategy">Expiration Strategy (Either Absolute/Sliding) - Default - Absolute</param>
        public static void Set<T>(string cacheKey, T typeValue, int hours, int minutes, int seconds, ExpirationStrategy expirationStrategy = ExpirationStrategy.Absolute) where T : class
        {
            Clear(cacheKey);
            TimeSpan slidingTime = new TimeSpan(hours, minutes, seconds);
            if (expirationStrategy == ExpirationStrategy.Absolute)
            {
                DateTime absoluteTime = DateTime.Now.Add(slidingTime);
                cache.Add(cacheKey, absoluteTime, typeValue);
            }
            else
            {
                cache.Add(cacheKey, slidingTime, typeValue);
            }
        }

        /// <summary>
        /// Returns total number of items available in Cache
        /// </summary>
        /// <returns></returns>
        public static long Count()
        {
            return cache.Count;
        }

        /// <summary>
        /// Clears the specified key from cache.
        /// </summary>
        /// <param name="cacheKey">The key.</param>
        public static void Clear(string cacheKey)
        {
            cache.Remove(cacheKey);
        }

        /// <summary>
        /// Clears all cached entries 
        /// </summary>
        public static void ClearAll()
        {
            cache.RemoveAll();
        }
        #endregion
    }
}
