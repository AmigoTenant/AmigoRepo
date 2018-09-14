using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Mindscape.Raygun4Net.WebApi;
using Amigo.Tenant.Application.Services.WebApi.ExceptionHandling;
using Amigo.Tenant.Application.Services.WebApi.Helpers.Cors;

namespace Amigo.Tenant.Application.Services.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {            
            //RaygunWebApiClient.Attach(config);
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));                        
            
            // Web API routes
            config.MapHttpAttributeRoutes();
            
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var formatters = config.Formatters;

            formatters.Remove(formatters.XmlFormatter);

            config.EnableCors(new WebConfigCorsPolicyProvider());

            config.Services.Replace(typeof(IExceptionHandler), new AmigoTenantExceptionHandler());

            config.Services.Add(typeof(IExceptionLogger), new RayGunExceptionLogger());

        }
    }
}
