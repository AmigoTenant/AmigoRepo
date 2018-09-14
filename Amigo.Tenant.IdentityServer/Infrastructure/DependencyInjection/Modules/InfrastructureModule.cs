using System.Configuration;
using System.Data.Entity;
using System.Reflection;
using Autofac;
using Amigo.Tenant.IdentityServer.Infrastructure.Storage;
using Amigo.Tenant.IdentityServer.Infrastructure.Users;
using Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage;
using Module = Autofac.Module;

namespace Amigo.Tenant.IdentityServer.Infrastructure.DependencyInjection.Modules
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("Amigo.Tenant.IdentityServer.Infrastructure"))
                .Where(x => x.IsClass)
                .AsImplementedInterfaces()
                .AsSelf();

            var connString = ConfigurationManager.ConnectionStrings[Constants.ConnectionName].ConnectionString;

            builder.RegisterType<UsersDbContext>()
                .WithParameter("connString", connString)
                .As<DbContext>()
                .AsSelf();
        }
    }
}