using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac;
using FluentValidation;

namespace Amigo.Tenant.Application.Services.WebApi.Validation.Fluent
{
    public class FluentConventionValidationFactory: IValidatorFactory
    {
        private readonly IContainer _container;

        public FluentConventionValidationFactory(IContainer container)
        {
            _container = container;
        }

        public IValidator<T> GetValidator<T>()
        {
            return GetValidator(typeof(T)) as IValidator<T>;
        }

        public IValidator GetValidator(Type type)
        {
            var baseType = typeof(AbstractValidator<>).GetTypeInfo();
            var validatorType = baseType.MakeGenericType(type);
            object validator = null;
            if(_container.TryResolve(validatorType,out validator)) return validator as IValidator;
            return null;
        }
    }
}