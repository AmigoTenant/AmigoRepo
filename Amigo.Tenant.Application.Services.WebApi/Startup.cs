using System.Web.Http;
using Microsoft.Owin;
using Owin;
using Amigo.Tenant.Web.Logging;

[assembly: OwinStartup(typeof(Amigo.Tenant.Application.Services.WebApi.Startup))]

namespace Amigo.Tenant.Application.Services.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var http = GlobalConfiguration.Configuration;

            DatabaseConfig.Configure();

            MappingConfig.Configure();

            WebApiConfig.Register(http);

            app.UseSecurity();            

            var container = DependencyConfig.Configure(http);

            ValidationConfig.Configure(http, container);
            
            SwaggerConfig.Register(http);

            FilterConfig.Configure(http);

            GlobalConfiguration.Configuration.EnsureInitialized();

            WorkItem.Init(WorkItem.Work);

            SqlServerTypes.Utilities.LoadNativeAssemblies(System.Web.Hosting.HostingEnvironment.MapPath("~/bin"));
        }
    }
}
