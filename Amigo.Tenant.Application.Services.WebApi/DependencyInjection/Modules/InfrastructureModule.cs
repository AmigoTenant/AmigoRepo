using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using Autofac;
using Autofac.Features.Variance;
using MediatR;
using System.Reflection;
using NPoco;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.ExpressMapper;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.EF.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.EF.Context;
using Amigo.Tenant.Infrastructure.Persistence.EF.Implementations.Security;
using Amigo.Tenant.Infrastructure.Persistence.NPoco;
using Amigo.Tenant.Infrastructure.Persistence.NPoco.Abstract;
using Amigo.Tenant.Security.Permissions.Abstract;
using Module = Autofac.Module;

namespace Amigo.Tenant.Application.Services.WebApi.DependencyInjection.Modules
{
    public class InfrastructureModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Mapping
            builder.RegisterType<ExMapper>().AsImplementedInterfaces().SingleInstance();

            //Infrastructure Services registration
            builder.RegisterAssemblyTypes(typeof(IBus).GetTypeInfo().Assembly).AsImplementedInterfaces();

            //Register query repositories
            builder.Register(x=> NPocoDatabaseFactory.DbFactory.GetDatabase()).As<IDatabase>()                
                .InstancePerRequest();

            builder.RegisterGeneric(typeof(NPocoDataAccess<>)).As(typeof(IQueryDataAccess<>)).InstancePerRequest();

            //Register Command repositories
            builder.RegisterType<NPocoRequestLogRepository>().As<IRepository<RequestLog>>();
            builder.RegisterGeneric(typeof(EFBaseRepository<>))
                .As(typeof(IRepository<>)).InstancePerRequest();            

            builder.RegisterType<EFUnitOfWork>().AsImplementedInterfaces().InstancePerRequest();
            
            //Register EF Context
            builder.RegisterType<AmigoTenantDbContext>().As<DbContext>().InstancePerRequest();

            //Register security
            builder.RegisterType<CachingEFPermissionsReader>().As<IPermissionsReader>().SingleInstance();

            //MediatR Bus configuration
            builder.RegisterSource(new ContravariantRegistrationSource());
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();            
            builder.RegisterInstance(Console.Out).As<TextWriter>();            
            builder.Register<SingleInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
            builder.Register<MultiInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t =>(IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            });            
        }
    }
}