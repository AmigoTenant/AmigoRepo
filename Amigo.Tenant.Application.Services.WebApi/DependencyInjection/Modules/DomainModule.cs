using Autofac;
using System.Reflection;
using Amigo.Tenant.CommandHandlers.Tracking.Moves;
using Module = Autofac.Module;

namespace Amigo.Tenant.Application.Services.WebApi.DependencyInjection.Modules
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Register Command Handlers
            builder.RegisterAssemblyTypes(typeof(RegisterMoveCommandHandler).GetTypeInfo().Assembly)
                .Where(x=>x.Name.EndsWith("Handler"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.Load("Amigo.Tenant.EventHandlers"))
                .Where(x=> x.Name.EndsWith("Handler"))
                .AsImplementedInterfaces();
        }
    }
}