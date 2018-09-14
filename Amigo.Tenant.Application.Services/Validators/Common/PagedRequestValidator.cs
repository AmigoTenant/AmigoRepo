using FluentValidation;
using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.Services.Validators.Common
{
    public abstract class PagedRequestValidator<T>: AbstractValidator<T> where T:PagedRequest
    {
        protected PagedRequestValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .LessThan(200);
        }
    }
}
