using FluentValidation;
using Amigo.Tenant.Application.DTOs.Requests.Security;

namespace Amigo.Tenant.Application.Services.Validators
{

    public class UpdateModuleValidator : AbstractValidator<UpdateModuleRequest>
    {
        public UpdateModuleValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();

        }
    }

}
