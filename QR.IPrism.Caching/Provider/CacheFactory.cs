using Microsoft.ApplicationServer.Caching;
using QR.IPrism.Logging;
using QR.IPrism.Caching.Adapters;
using QR.IPrism.Caching.Adapters.Distributed;
using QR.IPrism.Caching.Adapters.Http;
using QR.IPrism.Caching.Adapters.Memory;
using QR.IPrism.Caching.Config;

namespace QR.IPrism.Caching.Provider
{
    /// <summary>
    /// Factory for choosing caching approach
    /// </summary>
    public sealed class CacheFactory
    {
        #region Private Constructors
        /// <summary>
        /// Hint to compiler
        /// </summary>
        private CacheFactory() { }
        #endregion

        #region Public Static Methods

        /// <summary>
        /// Factory Method, instantiates caching strategy as per config
        /// </summary>
        /// <param name="config">Configuration from App.Config/Web.Config</param>
        /// <returns>Caching Handle (MemoryCache/HttpCache/DistributedCache)</returns>
        public static IGenericCache GetAdapter(ICacheAdapterConfig config)
        {
            IGenericCache cacheAdapter;
            switch (config.CacheType)
            {
                case Constants.Http:
                    cacheAdapter = new HttpCache();
                    break;
                case Constants.AppFabric:
                    cacheAdapter = GetAppFabricCache(config);
                    break;
                default:  //If no configuration is defined in the configuration file in that case use memory cache. As it will work for all types of applications.
                    cacheAdapter = new MemoryCache();
                    break;
            }
            return cacheAdapter;
        }
        #endregion

        #region Private Helper Routines
        /// <summary>
        /// Gets the distributed cache container, if configuration is incorrect then falls back to MemoryCache
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static IGenericCache GetAppFabricCache(ICacheAdapterConfig config)
        {
            IGenericCache cacheAdapter;
            try
            {
                cacheAdapter = new DistributedCache.Builder(config).BuildCache();
            }
            catch (DataCacheException dataCacheException)
            {
                Log.Warn(typeof(CacheFactory), "Distributed Caching can't be instantiated, more in error logs, switching to MemoryCache");
               
                cacheAdapter = new MemoryCache();
            }
            return cacheAdapter;
        }
        #endregion
    } 
}
