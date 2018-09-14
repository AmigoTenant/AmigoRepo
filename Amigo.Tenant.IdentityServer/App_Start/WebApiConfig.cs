using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Amigo.Tenant.IdentityServer
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config,bool securityEnabled)
        {
            MappingConfig.Configure();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "UserWebApiServices",                
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );            

            config.Routes.MapHttpRoute(
                name: "Welcome",
                routeTemplate: "",
                defaults: new { controller="Welcome",action="Index",id = RouteParameter.Optional }
            );

            if(securityEnabled)
                config.Filters.Add(new AuthorizeAttribute());
        }
    }
}
