using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using CacheManager.Core;

namespace Amigo.Tenant.Caching.Provider
{
    public class CacheConfiguration
    {
        public static ICacheManager<T> Build<T>()
        {
            var cacheConfig = ConfigurationBuilder.BuildConfiguration(settings =>
            {
                settings.WithSystemRuntimeCacheHandle("inprocess");                
            });
            return CacheManager.Core.CacheFactory.FromConfiguration<T>(cacheConfig);
        }

        public static ICacheManager<object> Build()
        {
            return Build<object>();
        }
    }
}