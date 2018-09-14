using Autofac;
using Amigo.Tenant.Application.Services.Tracking;
using System.Reflection;
using FluentValidation;
using Amigo.Tenant.Application.Services.Validators;
using Module = Autofac.Module;

namespace Amigo.Tenant.Application.Services.WebApi.DependencyInjection.Modules
{
    public class ApplicationServicesModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(MovesApplicationService).GetTypeInfo().Assembly)                
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(RegisterProductValidator).GetTypeInfo().Assembly)
                .AssignableTo<IValidator>()
                .AsClosedTypesOf(typeof(AbstractValidator<>));
        }
    }
}