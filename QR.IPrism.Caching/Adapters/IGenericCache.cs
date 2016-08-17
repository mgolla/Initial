using System;
using System.Diagnostics.CodeAnalysis;

namespace QR.IPrism.Caching.Adapters
{
    /// <summary>
    /// Parent Interface for all Caching Strategies
    /// </summary>
    public interface IGenericCache
    {
        [SuppressMessage("Microsoft.Naming", "CA1716", Justification = "This convention is being used at a lot many place and we find it intuitive", Scope = "Just for this method")]
        T Get<T>(string cacheKey) where T : class;

        [SuppressMessage("Microsoft.Naming", "CA1716", Justification = "This convention is being used at a lot many place and we find it intuitive", Scope = "Just for this method")]
        object Get(string cacheKey);

        void Add(string cacheKey, DateTime absoluteExpiry, object value);

        void Add(string cacheKey, TimeSpan slidingExpiry, object value);

        void Add(string cacheKey, object value);

        void Remove(string cacheKey);

        long Count { get; }

        void RemoveAll();

        bool Exists(string cacheKey);
    }
}
