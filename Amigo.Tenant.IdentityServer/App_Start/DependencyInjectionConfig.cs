using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Amigo.Tenant.IdentityServer.Configuration;
using Amigo.Tenant.IdentityServer.Infrastructure.DependencyInjection.Modules;
using Amigo.Tenant.IdentityServer.Infrastructure.ExternalAuthentication.Windows;

namespace Amigo.Tenant.IdentityServer
{
    public class DependencyInjectionConfig
    {
        public static void Configure(HttpConfiguration configuration)
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterModule<ApplicationServiceModule>();            
            builder.RegisterModule<InfrastructureModule>();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            
            builder.RegisterWebApiFilterProvider(configuration);

            builder.RegisterType<WindowsUserValidator>()
                .WithParameter("dsstoreName", Settings.WindowsDSStoreName)
                .WithParameter("emaildomain", Settings.WindowsEmailDomain);
           
            var container = builder.Build();
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);            
        }
    }
}