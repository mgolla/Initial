using System;

namespace QR.IPrism.Caching.Config
{
    public interface ICacheAdapterConfig
    {
        string CacheType { get; set; }
        string Server { get; set; }
        int Port { get; set; }
        Boolean UseSsl { get; set; }
        int ChannelOpenTimeout { get; set; }
        string SecurityMode { get; set; }
        string MessageSecurityAuthorizationInfo { get; set; }
        string CacheName { get; set; }
    }
}
