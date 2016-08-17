using System;
using System.Configuration;

namespace QR.IPrism.Caching.Config
{
    public class CacheAdapterConfig : ConfigurationSection, ICacheAdapterConfig
    {
        #region Public Configuration Properties
        [ConfigurationProperty("cacheType", DefaultValue = "memory", IsRequired = true)]
        public string CacheType
        {
            get
            {
                if (this["cacheType"] == null)
                {
                    throw GenerateConfigurationException("Cachetype is missing!");
                }
                return (string)this["cacheType"];
            }
            set
            {
                this["cacheType"] = value;
            }
        }


        [ConfigurationProperty("serviceUrl", IsRequired = false)]
        public string Server
        {
            get
            {
                if (this["serviceUrl"] == null)
                {
                    throw GenerateConfigurationException("Service Url is missing!");
                }
                return (string)this["serviceUrl"];
            }
            set
            {
                this["serviceUrl"] = value;
            }
        }

        [ConfigurationProperty("cacheName", IsRequired = false)]
        public string CacheName
        {
            get
            {
                if (this["cacheName"] == null)
                {
                    throw GenerateConfigurationException("Cache Name is missing!");
                }
                return (string)this["cacheName"];
            }
            set
            {
                this["cacheName"] = value;
            }
        }

        [ConfigurationProperty("servicePort", IsRequired = false)]
        public int Port
        {
            get
            {
                int port = 0;
                if (this["servicePort"] != null && !Int32.TryParse(this["servicePort"].ToString(), out port))
                {
                    throw GenerateConfigurationException("Service Port not defined or not an integer");
                }
                return port;
            }
            set
            {
                this["servicePort"] = value;
            }
        }

        [ConfigurationProperty("useSsl", DefaultValue = false, IsRequired = false)]
        public Boolean UseSsl
        {
            get
            {
                return (Boolean)this["useSsl"];
            }
            set
            {
                this["useSsl"] = value;
            }
        }

        [ConfigurationProperty("channelOpenTimeout", DefaultValue = 120, IsRequired = false)]
        public int ChannelOpenTimeout
        {
            get
            {
                return (int)this["channelOpenTimeout"];
            }
            set
            {
                this["channelOpenTimeout"] = value;
            }
        }

        [ConfigurationProperty("securityMode", DefaultValue = "Message", IsRequired = false)]
        public string SecurityMode
        {
            get
            {
                return (string)this["securityMode"];
            }
            set
            {
                this["securityMode"] = value;
            }
        }

        [ConfigurationProperty("messageSecurityAuthorizationInfo", DefaultValue = "", IsRequired = false)]
        public string MessageSecurityAuthorizationInfo
        {
            get
            {
                return (string)this["messageSecurityAuthorizationInfo"];
            }
            set
            {
                this["messageSecurityAuthorizationInfo"] = value;
            }
        }
        #endregion

        #region Private Methods
        private ConfigurationErrorsException GenerateConfigurationException(string message)
        {
            return new ConfigurationErrorsException(message);
        }
        #endregion
    }
}
