using System.Web.Http;
using Autofac;
using Amigo.Tenant.Application.Services.WebApi.Validation.Fluent;

namespace Amigo.Tenant.Application.Services.WebApi
{
    public static class ValidationConfig
    {
        public static void Configure(HttpConfiguration configuration, IContainer container)
        {
            configuration.ConfigFluentValidators(container);
        }
    }
}