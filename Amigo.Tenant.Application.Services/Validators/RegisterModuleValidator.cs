using FluentValidation;
using Amigo.Tenant.Application.DTOs.Requests.Security;

namespace Amigo.Tenant.Application.Services.Validators
{
    public class RegisterModuleValidator : AbstractValidator<RegisterModuleRequest>
    {
        public RegisterModuleValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
