using FluentValidation;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.Services.Validators.Common;

namespace Amigo.Tenant.Application.Services.Validators
{
    public class ProductSearchRequestValidator: PagedRequestValidator<CostCenterSearchRequest>
    {
        public ProductSearchRequestValidator()
        {
            RuleFor(x => x.Name)                
                .Length(0,50);
        }
    }
}
