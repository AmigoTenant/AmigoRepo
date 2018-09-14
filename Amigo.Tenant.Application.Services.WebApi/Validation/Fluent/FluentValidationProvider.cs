using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Metadata;
using System.Web.Http.Validation;
using FluentValidation;
using FluentValidation.Attributes;
using FluentValidation.Internal;
using FluentValidation.Validators;
using FluentValidation.WebApi;

namespace Amigo.Tenant.Application.Services.WebApi.Validation.Fluent
{
    public delegate ModelValidator FluentValidationModelValidationFactory(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders, PropertyRule rule, IPropertyValidator validator);
    public class FluentValidationModelValidatorProvider : ModelValidatorProvider
    {
        public IValidatorFactory ValidatorFactory { get; set; }

        public FluentValidationModelValidatorProvider(IValidatorFactory validatorFactory = null)
        {
            ValidatorFactory = validatorFactory ?? new AttributedValidatorFactory();
        }

        /// <summary>
        /// Initializes the FluentValidationModelValidatorProvider using the default options and adds it in to the ModelValidatorProviders collection.
        /// </summary>
        public static void Configure(HttpConfiguration configuration, Action<FluentValidationModelValidatorProvider> configurationExpression = null)
        {
            configurationExpression = configurationExpression ?? delegate { };

            var provider = new FluentValidationModelValidatorProvider();
            configurationExpression(provider);
            configuration.Services.Replace(typeof(IBodyModelValidator), new FluentValidationBodyModelValidator());
            configuration.Services.Add(typeof(ModelValidatorProvider), provider);
        }

        public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders)
        {
            if (IsValidatingProperty(metadata))
            {
                yield break;
            }

            IValidator validator = ValidatorFactory.GetValidator(metadata.ModelType);

            if (validator == null)
            {
                yield break;
            }

            yield return new FluentValidationModelValidator(validatorProviders, validator);
        }

        protected virtual bool IsValidatingProperty(ModelMetadata metadata)
        {
            return metadata.ContainerType != null && !string.IsNullOrEmpty(metadata.PropertyName);
        }
    }
}
