using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.ApplicationServer.Caching;
using QR.IPrism.Logging;
using QR.IPrism.Caching.Config;

namespace QR.IPrism.Caching.Adapters.Distributed
{
    /// <summary>
    /// Distributed Caching - Currently hooked to AppFabric
    /// Requires related configuration entries, AppFabric Cache service running and in reach.
    /// </summary>
    public class DistributedCache : IGenericCache
    {
        #region Private Variables
        /// <summary>
        /// Distributed Cache Handle, 
        /// Declared as static as instantiating each time is a taxing operation and it is required one per app domain.
        /// </summary>
        private static DataCache _cache;

        /// <summary>
        /// Only one factory should stay in memory, as creating one on every request is a resource intensive operation
        /// and leaving too many will probably exhaust all the TCP Ports 
        /// </summary>
        private static DataCacheFactory _cacheFactory;
        #endregion

        #region Private Constructors
        /// <summary>
        /// Hint: Please do not construct default constructor for this class
        /// </summary>
        private DistributedCache() { }
        #endregion

        #region Overridden Methods
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
            if (absoluteExpiry > DateTime.Now && value != null)
            {
                TimeSpan timeout = absoluteExpiry - DateTime.Now;
                _cache.Put(cacheKey, value, timeout);
            }
        }

        public void Add(string cacheKey, TimeSpan slidingExpiry, object value)
        {
            if (value != null)
            {
                _cache.Put(cacheKey, value, slidingExpiry);
            }
        }

        public void Add(string cacheKey, object value)
        {
            if (value != null)
            {
                _cache.Put(cacheKey, value);
            }
        }

        public void Remove(string cacheKey)
        {
            _cache.Remove(cacheKey);
        }

        public void RemoveAll()
        {
            IEnumerable<string> regions = _cache.GetSystemRegions();
            foreach (string regionName in regions)
            {
                if (!string.IsNullOrWhiteSpace(regionName))
                {
                    _cache.ClearRegion(regionName);
                }
            }
        }

        public long Count
        {
            get
            {
                long totalItemCount = 0;
                foreach (string regionName in _cache.GetSystemRegions())
                {
                    totalItemCount += _cache.GetObjectsInRegion(regionName).Count();
                }
                return totalItemCount;
            }
        }

        public bool Exists(string cacheKey)
        {
            return _cache[cacheKey] != null;
        }
        #endregion

        #region Builder for DistributedCache
        /// <summary>
        /// Builder for Distributed Cache, encapsulates Distributed cache construction process
        /// </summary>
        internal class Builder
        {
            #region Private Variables
            private ICacheAdapterConfig config;
            private Object lockHolder = new Object();//Locking object for controlling  static instantiation 
            #endregion

            #region Public Constructors
            public Builder(ICacheAdapterConfig config)
            {
                this.config = config;
                if (_cache == null)
                {
                    lock (lockHolder)
                    {
                        if (_cache == null) //If still null, double checking
                        {
                            _cache = ConfigureDistibutedCache();
                        }
                    }
                    Log.Info(this, "App Fabric Data Cache instantiated");
                }
            }
            #endregion

            #region Public Methods
            /// <summary>
            /// Builds Distributed Cache (ensures one and only once DataCache is constructed) 
            /// </summary>
            /// <returns>Distributed Cache instantiated with underlying AppFabric DataCache</returns>
            public DistributedCache BuildCache()
            {
                DistributedCache distributedCache = new DistributedCache();
                //At this level we know for sure that the cache has been instantiated as the Builder constructor would have taken care of it
                return distributedCache;
            }
            #endregion

            #region Private Methods
            private DataCache ConfigureDistibutedCache()
            {
                Log.Info(this, "Cache Factory initialized with following Configuration");
                Log.Info(this, config);

                DataCacheFactoryConfiguration distributedCacheConfiguration = new DataCacheFactoryConfiguration();
                List<DataCacheServerEndpoint> serverEndPoints = new List<DataCacheServerEndpoint>();

                serverEndPoints.Add(new DataCacheServerEndpoint(config.Server, config.Port));

                distributedCacheConfiguration.Servers = serverEndPoints;
                distributedCacheConfiguration.RequestTimeout = new TimeSpan(0, 0, config.ChannelOpenTimeout);

                if (_cacheFactory == null)
                {
                    lock (lockHolder)
                    {
                        if (_cacheFactory == null)
                        {
                            _cacheFactory = new DataCacheFactory(distributedCacheConfiguration);
                        }
                    }
                    Log.Info(this, "Cache Factory Instantiated");
                }

                if (string.IsNullOrWhiteSpace(config.CacheName))
                {
                    throw new ConfigurationErrorsException(Constants.CacheNameMissing);
                }

                Log.Info(this, "Cache object instantiated");
                return _cacheFactory.GetCache(config.CacheName);
            }
            #endregion
        }
        #endregion
    }
}
