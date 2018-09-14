using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Amigo.Tenant.Application.DTOs.Responses.Common;

namespace Amigo.Tenant.Application.Services.WebApi.Validation.Fluent
{
    public abstract class ValidateBaseActionFilter : ActionFilterAttribute
    {
        protected void ValidateAction(HttpActionContext actionContext)
        {            
            var arguments = actionContext.ActionArguments;

            if (arguments == null || !arguments.Any()) return;
            if (arguments.Count > 1) return;            

            var resp = actionContext.ModelState.ToResponse();
            if (resp.IsValid) return;

            actionContext.Response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ObjectContent<ResponseDTO>(resp, new JsonMediaTypeFormatter())
            };

        }
    }    
}