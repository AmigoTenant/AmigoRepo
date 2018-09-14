using FluentValidation;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.Services.Validators.Common;

namespace Amigo.Tenant.Application.Services.Validators.Security
{
    public class ModuleSearchRequestValidator : PagedRequestValidator<ModuleSearchRequest>
    {
        public ModuleSearchRequestValidator()
        {
            RuleFor(x => x.OnlyParents).NotNull();
        }
    }
}
