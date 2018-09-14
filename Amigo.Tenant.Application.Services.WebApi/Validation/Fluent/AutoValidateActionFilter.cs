using System.Net.Http;
using System.Web.Http.Controllers;

namespace Amigo.Tenant.Application.Services.WebApi.Validation.Fluent
{
    public class AutoValidateActionFilter: ValidateBaseActionFilter
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
           if (actionContext.Request.Method != HttpMethod.Post)return;
           ValidateAction(actionContext);
        }
    }
}