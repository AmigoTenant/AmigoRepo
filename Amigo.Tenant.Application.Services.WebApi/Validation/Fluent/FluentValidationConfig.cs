using System.Web.Http;
using System.Web.Http.Validation;
using Autofac;
using FluentValidation.WebApi;

namespace Amigo.Tenant.Application.Services.WebApi.Validation.Fluent
{
    public static class FluentValidationConfig
    {
        public static void ConfigFluentValidators(this HttpConfiguration configuration,IContainer container)
        {
            var provider = new FluentValidationModelValidatorProvider(new FluentConventionValidationFactory(container));            
            configuration.Services.Replace(typeof(IBodyModelValidator), new FluentValidationBodyModelValidator());
            configuration.Services.Add(typeof(ModelValidatorProvider), provider);
        }
    }
}