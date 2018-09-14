using System.Linq;
using FluentValidation.Results;
using Amigo.Tenant.Application.DTOs.Responses.Common;

namespace Amigo.Tenant.Application.Services.WebApi.Validation.Fluent
{
    public static class FluentValidationExtension
    {
        public static ResponseDTO ToResponse(this ValidationResult validationResult)
        {
            var messages = validationResult.Errors.Select(x => new ApplicationMessage(x.PropertyName, x.ErrorMessage));

            return new ResponseDTO(validationResult.IsValid)
            {
                Messages = messages.ToList()
            };
        }
    }
}