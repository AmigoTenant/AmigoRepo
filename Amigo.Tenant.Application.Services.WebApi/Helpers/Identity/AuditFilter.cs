using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;
using Amigo.Tenant.Application.DTOs.Requests.Common;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

namespace Amigo.Tenant.Application.Services.WebApi.Helpers.Identity
{
    public class AuditFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
           
            //make sure there is one request object being posted
            if (actionContext.Request.Method != HttpMethod.Post) return;
            var arguments = actionContext.ActionArguments;
            if (arguments == null || !arguments.Any()) return;
            if (arguments.Count != 1) return;

            //set the UserId field in the request object, based on the current user            
            AuditBaseRequest req = arguments.First().Value as AuditBaseRequest;
            if (req == null) return;
            req.UserId = actionContext.RequestContext.Principal.Identity.GetUserId();
            req.Username = actionContext.RequestContext.Principal.Identity.GetUsername();
        }
    }
}