using System.Web.Http;
using Amigo.Tenant.Application.Services.WebApi.Helpers.Configuration;
using Amigo.Tenant.Application.Services.WebApi.Helpers.Identity;
using Amigo.Tenant.Application.Services.WebApi.Validation.Fluent;

namespace Amigo.Tenant.Application.Services.WebApi
{
    public static class FilterConfig
    {
        public static void Configure(HttpConfiguration configuration)
        {
            configuration.Filters.Add(new AutoValidateActionFilter());
            configuration.Filters.Add(new AuditFilter());
            if (Settings.SecurityEnabled)
            configuration.Filters.Add(new AuthorizeAttribute());
        }
    }
}