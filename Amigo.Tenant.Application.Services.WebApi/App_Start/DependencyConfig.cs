using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Amigo.Tenant.Application.Services.WebApi.Controllers;
using Amigo.Tenant.Application.Services.WebApi.DependencyInjection.Modules;
using Amigo.Tenant.Caching.Autofac.Configuration;
using Amigo.Tenant.Caching.Provider;
using Amigo.Tenant.Caching.Web.Filters;
using Amigo.Tenant.ServiceAgent.IdentityServer;

namespace Amigo.Tenant.Application.Services.WebApi
{
    public static class DependencyConfig
    {
        public static IContainer Configure(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();


            //Register modules
            builder.RegisterModule<ApplicationServicesModule>();
            builder.RegisterModule<DomainModule>();
            builder.RegisterModule<InfrastructureModule>();
            builder.RegisterModule<CachingModule>();

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            // Register inject Agent Identity Server
           
            builder.RegisterType<IdentitySeverAgent>().As<IIdentitySeverAgent>();

            //Reference:: How to inject filters
            //builder.Register(c => new CachingMasterDataAttribute(c.Resolve<ICacheFactory>()))
            //    .AsWebApiActionFilterFor<EquipmentTypeController>()
            //    .InstancePerRequest();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            
            return container;
        }
    }
}