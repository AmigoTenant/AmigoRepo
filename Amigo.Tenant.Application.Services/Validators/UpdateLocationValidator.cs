using FluentValidation;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;

namespace Amigo.Tenant.Application.Services.Validators
{
    

    public class UpdateLocationValidator : AbstractValidator<UpdateLocationRequest>
    {
        public UpdateLocationValidator()
        {
            RuleFor(x => x.Name).NotEmpty();

            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
