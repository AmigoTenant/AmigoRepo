using FluentValidation;
using Amigo.Tenant.Application.DTOs.Requests.Security;

namespace Amigo.Tenant.Application.Services.Validators
{
    public class UpdateDeviceValidator : AbstractValidator<UpdateDeviceRequest>
    {
        public UpdateDeviceValidator()
        {
            RuleFor(x => x.CellphoneNumber).NotEmpty();
        }
    }
}
