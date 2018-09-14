using CacheManager.Core;

namespace Amigo.Tenant.Caching.Provider
{
    public class CacheFactory : ICacheFactory
    {
        public static ICacheFactory Current { get; private set; }

        static CacheFactory()
        {
            Current = new CacheFactory();
        }
        public static void Init(ICacheFactory cacheFactory)
        {
            Current = cacheFactory;
        }

        public ICacheManager<T> CreateCacheManager<T>()
        {
            return CacheConfiguration.Build<T>();            
        }
    }
}