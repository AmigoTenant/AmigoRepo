using FluentValidation;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;

namespace Amigo.Tenant.Application.Services.Validators
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.ShortName).NotEmpty();
            RuleFor(x => x.IsHazardous);
        }
    }
}
