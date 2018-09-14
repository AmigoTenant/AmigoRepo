using CacheManager.Core;

namespace Amigo.Tenant.Caching.Provider
{
    public interface ICacheFactory
    {
        ICacheManager<T> CreateCacheManager<T>();
    }
}