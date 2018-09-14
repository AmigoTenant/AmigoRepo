using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace Amigo.Tenant.IdentityServer.Infrastructure.DependencyInjection.Modules
{
    public class ApplicationServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("Amigo.Tenant.IdentityServer.ApplicationServices"))
                .Where(x => x.IsClass)
                .AsImplementedInterfaces()
                .AsSelf();            
        }
    }
}