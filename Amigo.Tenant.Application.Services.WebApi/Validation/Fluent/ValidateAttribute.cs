using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace Amigo.Tenant.Application.Services.WebApi.Validation.Fluent
{
    public class ValidateAttribute : ValidateBaseActionFilter
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var arguments = actionContext.ActionArguments;

            if (actionContext.Request.Method == HttpMethod.Get && arguments.Any() && arguments.First().Key != null && arguments.First().Value == null)
            {
                actionContext.ModelState.AddModelError("SearchCriteriaNotFound", "Provide at least one search criteria");
            }

            ValidateAction(actionContext);
        }
    }
}