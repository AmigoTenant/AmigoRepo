using Autofac;
using Autofac.Builder;
using CacheManager.Core;
using Amigo.Tenant.Caching.Provider;

namespace Amigo.Tenant.Caching.Autofac.Configuration
{
    public class CachingModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Provider.CacheFactory>().As<ICacheFactory>();
        }
    }
}